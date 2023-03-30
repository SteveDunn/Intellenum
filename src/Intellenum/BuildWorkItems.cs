using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Intellenum.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum;

internal static class BuildWorkItems
{
    public static VoWorkItem? TryBuild(VoTarget target,
        SourceProductionContext context,
        IntellenumConfiguration? globalConfig,
        Compilation compilation)
    {
        TypeDeclarationSyntax voTypeSyntax = target.VoSyntaxInformation;

        INamedTypeSymbol voSymbolInformation = target.VoSymbolInformation;

        if (target.DataForAttributes.Length == 0) return null;

        ImmutableArray<AttributeData> allAttributes = voSymbolInformation.GetAttributes();

        if (target.DataForAttributes.Length != 1)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.DuplicateTypesFound(voTypeSyntax.GetLocation(), voSymbolInformation.Name));
            return null;
        }
        
        if (!voTypeSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.TypeShouldBePartial(voTypeSyntax.GetLocation(), voSymbolInformation.Name));
            return null;
        }

        if (voSymbolInformation.IsAbstract)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.TypeCannotBeAbstract(voSymbolInformation));
        }

        if (ReportErrorsForAnyUserConstructors(context, voSymbolInformation))
        {
            return null;
        }

        AttributeData voAttribute = target.DataForAttributes[0];

        // Build the configuration but only log issues as diagnostics if they would cause additional compilation errors,
        // such as incorrect exceptions, or invalid customizations. For other issues, there are separate analyzers.
        var localBuildResult = ManageAttributes.TryBuildConfigurationFromAttribute(voAttribute);
        foreach (var diagnostic in localBuildResult.Diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }

        IntellenumConfiguration? localConfig = localBuildResult.ResultingConfiguration;
        
        if (localConfig == null)
        {
            return null;
        }

        var config = IntellenumConfiguration.Combine(
            localConfig.Value,
            globalConfig,
            () => compilation.GetSpecialType(SpecialType.System_Int32));

        ReportErrorIfNestedType(target, context, voSymbolInformation);

        var instanceProperties =
            FindInstancesFromInstanceAttribute(allAttributes, voSymbolInformation, context, config.UnderlyingType).ToList();

        instanceProperties.AddRange(FindInstancesFromInstanceMethodInStaticConstructor(voSymbolInformation).ToList());
        instanceProperties.AddRange(FindInstancesFromNewStatements(voSymbolInformation).ToList());

        var toStringInfo = HasToStringOverload(voSymbolInformation);

        ReportErrorIfVoTypeIsSameAsUnderlyingType(context, voSymbolInformation, config.UnderlyingType);

        ReportErrorIfUnderlyingTypeIsCollection(context, voSymbolInformation, config.UnderlyingType);
        
        ReportErrorIfNoInstancesFound(instanceProperties, context, voSymbolInformation);

        var isValueType = IsUnderlyingAValueType(config.UnderlyingType);

        bool isConstant = IsUnderlyingACompileTimeConstant(config.UnderlyingType);
        
        return new VoWorkItem
        {
            IsConstant = isConstant,
            InstanceProperties = instanceProperties.ToList(),
            TypeToAugment = voTypeSyntax,
            IsValueType = isValueType,
            HasToString = toStringInfo.HasToString,
            UnderlyingType = config.UnderlyingType ?? throw new InvalidOperationException("No underlying type"),
            Conversions = config.Conversions,
            DebuggerAttributes = config.DebuggerAttributes,
            Customizations = config.Customizations,
            FullNamespace = voSymbolInformation.FullNamespace()
        };
    }

    private record struct ToStringInfo(bool HasToString, bool IsRecordClass, bool IsSealed, IMethodSymbol? Method);

    private static ToStringInfo HasToStringOverload(ITypeSymbol typeSymbol)
    {
        while (true)
        {
            var toStringMethods = typeSymbol.GetMembers("ToString").OfType<IMethodSymbol>();

            foreach (IMethodSymbol eachMethod in toStringMethods)
            {
                // we could have "public virtual new string ToString() => "xxx" 
                if (!eachMethod.IsOverride && !eachMethod.IsVirtual)
                {
                    continue;
                }

                // can't change access rights
                if (eachMethod.DeclaredAccessibility != Accessibility.Public && eachMethod.DeclaredAccessibility != Accessibility.Protected)
                {
                    continue;
                }

                if (eachMethod.Parameters.Length != 0)
                {
                    continue;
                }

                // records always have an implicitly declared ToString method. In C# 10, the user can differentiate this
                // by making the method sealed.
                if (typeSymbol.IsRecord && eachMethod.IsImplicitlyDeclared)
                {
                    continue;
                }

                // In C# 10, the user can differentiate a ToString overload by making the method sealed.
                // We report back if it's sealed or not so that we can emit an error if it's not sealed.
                // The error stops another compilation error; if unsealed, the generator generates a duplicate ToString() method.
                return new ToStringInfo(HasToString: true, IsRecordClass: typeSymbol.IsRecord && typeSymbol.IsReferenceType, IsSealed: eachMethod.IsSealed, eachMethod);
            }

            INamedTypeSymbol? baseType = typeSymbol.BaseType;

            if (baseType is null)
            {
                return new ToStringInfo(false, false, false, null);
            }
            
            if (baseType.SpecialType == SpecialType.System_Object || baseType.SpecialType == SpecialType.System_ValueType)
            {
                return new ToStringInfo(false, false, false, null);
            }

            typeSymbol = baseType;
        }
    }

    private static bool IsUnderlyingAValueType(INamedTypeSymbol underlyingType) => underlyingType.IsValueType;

    private static bool IsUnderlyingACompileTimeConstant(INamedTypeSymbol underlyingType) =>
        underlyingType.SpecialType switch
        {
            SpecialType.System_Byte => true,
            SpecialType.System_SByte => true,
            SpecialType.System_Int16 => true,
            SpecialType.System_UInt16 => true,
            SpecialType.System_Int32 => true,
            SpecialType.System_UInt32 => true,
            SpecialType.System_Int64 => true,
            SpecialType.System_UInt64 => true,
            SpecialType.System_String => true,
            SpecialType.System_Decimal => true,
            _ => false,
        };

    private static void ReportErrorIfUnderlyingTypeIsCollection(
        SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation,
        INamedTypeSymbol underlyingType)
    {
        if (underlyingType.ImplementsInterfaceOrBaseClass(typeof(ICollection)))
        {
            context.ReportDiagnostic(
                DiagnosticsCatalogue.UnderlyingTypeCannotBeCollection(voSymbolInformation, underlyingType));
        }
    }

    private static void ReportErrorIfNoInstancesFound(List<InstanceProperties> instancePropertiesList,
        SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation)
    {
        if(instancePropertiesList.Count == 0)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.MustHaveInstances(voSymbolInformation));
        }
    }

    private static void ReportErrorIfVoTypeIsSameAsUnderlyingType(
        SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation,
        INamedTypeSymbol underlyingType)
    {
        if (SymbolEqualityComparer.Default.Equals(voSymbolInformation, underlyingType))
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.UnderlyingTypeMustNotBeSameAsValueObjectType(voSymbolInformation));
        }
    }

    private static void ReportErrorIfNestedType(VoTarget target, SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation)
    {
        INamedTypeSymbol? containingType = target.ContainingType;
        if (containingType != null)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.TypeCannotBeNested(voSymbolInformation, containingType));
        }
    }

    private static bool ReportErrorsForAnyUserConstructors(SourceProductionContext context, INamedTypeSymbol voSymbolInformation)
    {
        bool reported = false;

        ImmutableArray<IMethodSymbol> allConstructors = voSymbolInformation.Constructors;

        foreach (IMethodSymbol? eachConstructor in allConstructors)
        {
            if (eachConstructor.IsImplicitlyDeclared) continue;
            if (eachConstructor.IsStatic) continue;

            context.ReportDiagnostic(DiagnosticsCatalogue.CannotHaveUserConstructors(eachConstructor));
            reported = true;
        }

        return reported;
    }

    private static IEnumerable<InstanceProperties> FindInstancesFromInstanceAttribute(
        ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol voClass,
        SourceProductionContext context, 
        INamedTypeSymbol? underlyingType)
    {
        IEnumerable<AttributeData> matchingAttributes =
            attributes.Where(a => a.AttributeClass?.FullName() is "Intellenum.InstanceAttribute");

        var props = BuildInstancePropertiesFromAttributes.Build(matchingAttributes, context, voClass, underlyingType);
        
        return props.Where(a => a is not null)!;
    }

    private static IEnumerable<InstanceProperties> FindInstancesFromInstanceMethodInStaticConstructor(INamedTypeSymbol voClass)
    {
        var constructor = voClass.Constructors.FirstOrDefault(m => m.IsStatic && m.IsPrivate());
        
        if (constructor is null) yield break;

        var decl = constructor.DeclaringSyntaxReferences.SingleOrDefault("Expected exactly one syntax reference for constructor");
        if (decl is null) yield break;
        
        var syntax = decl.GetSyntax();

        var instanceInvocations = syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(IsCallingInstance);
        
        foreach (var eachInvocation in instanceInvocations)
        {
            ArgumentSyntax first = eachInvocation.ArgumentList.Arguments[0];
                
            var firstAsString = (first.Expression as LiteralExpressionSyntax)?.Token.Value as string;
            if (firstAsString is null)
            {
                throw new InvalidOperationException("Expected string literal as name parameter to Instance method");
            }

            ArgumentSyntax second = eachInvocation.ArgumentList.Arguments[1];
            var secondAsString = second.ToString();

            yield return new InstanceProperties(InstanceSource.FromInstanceMethod,  firstAsString, firstAsString, secondAsString, secondAsString);
        }
    }

    private static IEnumerable<InstanceProperties> FindInstancesFromNewStatements(INamedTypeSymbol voClass)
    {
        var publicStaticMembers = voClass.GetMembers().Where(m => m.IsStatic && m.IsPublic());
        
        var hits = publicStaticMembers.Where(symbol => isSameSymbol(symbol));
        
        // we have the field
        foreach (ISymbol each in hits)
        {
            var decl = each.DeclaringSyntaxReferences.SingleOrDefault("Expected zero or at most, one, declaring syntax reference for field");
            
            if (decl is null) continue;
        
            var syntax = decl.GetSyntax();

            var allConstructors = syntax.DescendantNodes().OfType<BaseObjectCreationExpressionSyntax>();
            
            var newExpressions = allConstructors.Where(c => c.ArgumentList?.Arguments.Count >=1).ToList();

            var implicitNews = newExpressions.OfType<ImplicitObjectCreationExpressionSyntax>()
                .Where(x => IsMatch(x, voClass.Name)).ToList();
                
            IEnumerable<BaseObjectCreationExpressionSyntax> explicitNews = newExpressions.OfType<ObjectCreationExpressionSyntax>()
                .Where(c => c.Type is IdentifierNameSyntax ins && ins.Identifier.Text == voClass.Name);
            
            var combined = implicitNews.Concat(explicitNews);
            
            BaseObjectCreationExpressionSyntax? newExpression = combined.SingleOrDefault("Expected exactly one new statement from the combined implicit and explicit new statements");
            //newExpression ??= syntax.DescendantNodes().OfType<ImplicitObjectCreationExpressionSyntax>().SingleOrDefault();
            if(newExpression is null) continue;

            var args = newExpression.ArgumentList;
            if (args is null) continue;
            
            // we could have 1 or 2 arguments here.
            // if it's two, then the first is the "enum name", and the second is the value (the "Field Name" is inferred from the syntax
            // it it's just one, then the "Field Name" is the "enum name", and the value is inferred from the syntax.

            string fieldName = each.Name; // todo: get the field name from the syntax
            string enumName = fieldName;

            bool explicitlyNamed = false;
            if (args.Arguments.Count >2) continue;

            if (args.Arguments.Count == 2)
            {
                explicitlyNamed = true;
                ArgumentSyntax first = args.Arguments[0];

                var firstAsString = (first.Expression as LiteralExpressionSyntax)?.Token.Value as string;

                enumName = firstAsString ?? throw new InvalidOperationException(
                    $"Expected string literal as name parameter to a parameter of the constructor for creating a type of '{voClass.Name}'");
            }
            
            int index = args.Arguments.Count == 2 ? 1 : 0;

            // we don't need the expression - but would it come in handy? // todo: determine
            ArgumentSyntax second = args.Arguments[index];
            var secondAsString = second.ToString();

            // the name field makes no sense here because the name *must* be the name of the field being declared.
            // the only thing that we could do is make it an alias if different, but users might as well declare the alias themselves as the field name.
            // perhaps what we could do is look for a new express
            // SmartEnum allows a different 'name' to be added, and it also allows the same 'value' to be added
            // So we could treat this is an alias, e.g. "Item1" is the field name, and "Fred" is the name in the constructor, so 'FromName' 
            // could look up "Fred" and return the instance with the field name "Item1"

            
            // in the static constructor, we can enumerate these and sets the values on the fields - it just means
            // making FieldName and EnumName with private setters
            
            // actually, none of this will work - an explicit field *must* specify the field name, the enum name, and the value
            // we can probably infer the enum name from the field name with a constructor overload, but that's it.
            
            yield return new InstanceProperties(InstanceSource.FromNewExpression, fieldName, enumName, secondAsString, secondAsString, "",  explicitlyNamed);
        }

        bool isSameSymbol(ISymbol m)
        {
            return SymbolEqualityComparer.Default.Equals(m.GetMemberType(), voClass);
            // return SymbolEqualityComparer.Default.Equals(m.GetMemberType(), config.UnderlyingType);
        }
    }

    private static bool IsMatch(ImplicitObjectCreationExpressionSyntax ine, string expectedType)
    {
        var fieldDeclarationSyntax = ine.Ancestors().OfType<FieldDeclarationSyntax>().FirstOrDefault();
        if (fieldDeclarationSyntax is null) return false;
        string x = fieldDeclarationSyntax.Declaration.Type.ToString();
        return x == expectedType;
    }

    private static bool IsCallingInstance(InvocationExpressionSyntax arg)
    {
        var nodes = arg.DescendantNodes().OfType<IdentifierNameSyntax>();
        var v = nodes.SingleOrDefault(n => n?.Identifier.ToString() == "Instance", "Expected zero or at most, one, instance of IdentifierNameSystem with the text 'Instance'");
        return v is not null;
    }
}

public enum InstanceSource
{
    FromAttribute,
    FromInstanceMethod,
    FromNewExpression
}
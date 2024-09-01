using System.Collections;
using System.Collections.Immutable;
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

        // even if this has issues such as duplicates, we still want to continue
        // otherwise we'd get seemingly unrelated compilation errors, such as
        // 'The name 'Member' does not exist in the current context
        MemberPropertiesCollection discoveredMembers =
            DiscoverMembers.Discover(voSymbolInformation, config.UnderlyingType, compilation);
        
        foreach (var eachDiagnostic in discoveredMembers.AllDiagnostics)
        {
            context.ReportDiagnostic(eachDiagnostic.Diagnostic);
        }
        
        var hasToStringOverload = HasToStringOverload(voSymbolInformation);

        ReportErrorIfEnumTypeIsSameAsUnderlyingType(context, voSymbolInformation, config.UnderlyingType);

        ReportErrorIfUnderlyingTypeIsCollection(context, voSymbolInformation, config.UnderlyingType);

        ReportErrorIfNoMembersFound(discoveredMembers, context, voSymbolInformation);

        var isValueType = IsUnderlyingAValueType(config.UnderlyingType);

        bool isConstant = IsUnderlyingACompileTimeConstant(config.UnderlyingType);


        INamedTypeSymbol? c = compilation.GetTypeByMetadataName($"System.IComparable");
        var isUnderlyingIsIComparableOfT = config.UnderlyingType.ImplementsIComparableOfTInterface(compilation, config.UnderlyingType);
        var isUnderlyingIComparable = config.UnderlyingType.Implements(c);


        return new VoWorkItem
        {
            IsConstant = isConstant,
            MemberProperties = discoveredMembers,
            TypeToAugment = voTypeSyntax,
            IsValueType = isValueType,
            HasToString = hasToStringOverload,
            UnderlyingType = config.UnderlyingType,
            IsUnderlyingIComparable = isUnderlyingIComparable,
            IsUnderlyingIsIComparableOfT = isUnderlyingIsIComparableOfT,
            Conversions = config.Conversions,
            DebuggerAttributes = config.DebuggerAttributes,
            Customizations = config.Customizations,
            FullNamespace = voSymbolInformation.FullNamespace()
        };
    }

    private static bool HasToStringOverload(ITypeSymbol typeSymbol)
    {
        while (true)
        {
            var toStringMethods = typeSymbol.GetMembers("ToString").OfType<IMethodSymbol>();

            foreach (IMethodSymbol eachMethod in toStringMethods)
            {
                // we could have "public virtual new string ToString() => "xxx" 
                if (eachMethod is { IsOverride: false, IsVirtual: false })
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
                return true;
            }

            INamedTypeSymbol? baseType = typeSymbol.BaseType;

            if (baseType is null)
            {
                return false;
            }

            if (baseType.SpecialType == SpecialType.System_Object || baseType.SpecialType == SpecialType.System_ValueType)
            {
                return false;
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

    private static void ReportErrorIfNoMembersFound(MemberPropertiesCollection memberPropertiesList,
        SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation)
    {
        if (memberPropertiesList.IsEmpty)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.MustHaveMembers(voSymbolInformation));
        }
    }

    private static void ReportErrorIfEnumTypeIsSameAsUnderlyingType(
        SourceProductionContext context,
        INamedTypeSymbol voSymbolInformation,
        INamedTypeSymbol underlyingType)
    {
        if (SymbolEqualityComparer.Default.Equals(voSymbolInformation, underlyingType))
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.UnderlyingTypeMustNotBeSameAsEnumType(voSymbolInformation));
        }
    }

    private static void ReportErrorIfNestedType(VoTarget target,
        SourceProductionContext context,
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
}
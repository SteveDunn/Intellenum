using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Intellenum.Extensions;
using Intellenum.MemberBuilding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum;

internal class DiscoverMembers
{
    public static MemberPropertiesCollection Discover(ImmutableArray<AttributeData> allAttributes,
        INamedTypeSymbol voSymbolInformation,
        INamedTypeSymbol underlyingType,
        Compilation compilation) =>
        MemberPropertiesCollection.Combine(
            FromMemberAttributes(allAttributes, voSymbolInformation, underlyingType),
            FromMembersAttribute(allAttributes, voSymbolInformation, underlyingType),
            FromCallsToMemberMethod(voSymbolInformation, underlyingType, compilation),
            FromTheSingleCallToMembersMethod(voSymbolInformation, underlyingType),
            FromNewStatements(voSymbolInformation, underlyingType));

    private static MemberPropertiesCollection FromMemberAttributes(
        ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol voClass, 
        INamedTypeSymbol underlyingType)
    {
        var matchingAttributes = FilterToAttributesNamed(attributes, "Intellenum.MemberAttribute");

        return MemberBuilder.BuildFromMemberAttributes(matchingAttributes, voClass, underlyingType);
    }

    private static IEnumerable<AttributeData> FilterToAttributesNamed(ImmutableArray<AttributeData> attributes, string name) => 
        attributes.Where(a => a.AttributeClass?.FullName() == name);

    private static MemberPropertiesCollection FromMembersAttribute(
        ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType)
    {
        var matchingAttributes = FilterToAttributesNamed(attributes, "Intellenum.MembersAttribute").ToList();

        if (matchingAttributes.Count != 1)
        {
            return MemberPropertiesCollection.Empty();
        }

        AttributeData matchingAttribute = matchingAttributes[0];

        if (!underlyingType.SpecialType.IsStringOrInt())
        {
            return MemberPropertiesCollection.WithDiagnostic(
                DiagnosticsCatalogue.MembersAttributeShouldOnlyBeOnIntOrStringBasedEnums(voClass),
                voClass.Locations[0]);
        }

        return MemberBuilder.TryBuildFromMembersFromCsvInAttribute(matchingAttribute, voClass, underlyingType);
    }

    private static IEnumerable<InvocationExpressionSyntax> GetInvocationsInConstructor(INamedTypeSymbol voClass, string methodName)
    {
        var constructor = voClass.Constructors.FirstOrDefault(m => m.IsStatic && m.IsPrivate());
        
        if (constructor is null)
        {
            return [];
        }

        var ctor = constructor.DeclaringSyntaxReferences.SingleOrDefaultNoThrow();
        if (ctor is null)
        {
            return [];
        }

        var syntax = ctor.GetSyntax();

        return syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(arg => IsCallingMember(arg, methodName));
    }

    private static MemberPropertiesCollection FromCallsToMemberMethod(INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType,
        Compilation compilation)
    {
        MemberPropertiesCollection l = new();
        
        var memberInvocations = GetInvocationsInConstructor(voClass, "Member");

        int index = 0;
        
        foreach (InvocationExpressionSyntax? eachInvocation in memberInvocations)
        {
            SemanticModel m = compilation.GetSemanticModel(eachInvocation.SyntaxTree);

            // Because we're looking for a source-generated method, the IOperation here
            // will be an error because it doesn't really exist.
            // The parameters however, are represented by operations.
            IOperation? op = m.GetOperation(eachInvocation);
            if (op?.ChildOperations is null)
            {
                continue;
            }

            if (op.ChildOperations.Count < 2)
            {
                continue;
            }
            
            var firstArg = op.ChildOperations.ElementAt(1);
            
            string firstAsString = firstArg.ConstantValue.ToString();
            string secondAsString;

            bool isExplicitlyNamed = op.ChildOperations.Count > 2;
            
            if (op.ChildOperations.Count == 2)
            {
                if (!underlyingType.SpecialType.IsStringOrInt())
                {
                    l.Add(DiagnosticsCatalogue.MemberMethodCallCanOnlyOmitValuesForStringsAndInts(voClass), eachInvocation.GetLocation());
                    continue;
                }

                if (underlyingType.SpecialType is SpecialType.System_String)
                {
                    secondAsString = $"\"{firstAsString}\"";
                }
                else
                {
                    secondAsString = $"{index++}";
                }
            }
            else
            {
                
                IOperation secondArg = op.ChildOperations.ElementAt(2);
                secondAsString = secondArg.Syntax.ToString();
            }

            l.Add(
                new MemberProperties(
                    source: MemberSource.FromMemberMethod,
                    fieldName: firstAsString,
                    enumFriendlyName: firstAsString,
                    valueAsText: secondAsString,
                    value: secondAsString,
                    tripleSlashComments: string.Empty,
                    wasExplicitlyNamed: isExplicitlyNamed));
        }

        return l;
    }

    private static MemberPropertiesCollection FromTheSingleCallToMembersMethod(
        INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType)
    {
        MemberPropertiesCollection l = new();
        
        var memberInvocations = GetInvocationsInConstructor(voClass, "Members").ToList();

        if (memberInvocations.Count == 0)
        {
            return l;
        }

        if (memberInvocations.Count > 1)
        {
            l.Add(DiagnosticsCatalogue.CallToMembersMethodShouldOnlyBeCalledOnce(voClass), memberInvocations[1].GetLocation());
            return l;
        }

        var eachInvocation = memberInvocations.Single();
        ArgumentSyntax first = eachInvocation.ArgumentList.Arguments[0];
                
        var firstAsString = (first.Expression as LiteralExpressionSyntax)?.Token.Value as string;
        if (firstAsString is null)
        {
            return l;
        }

        return MemberBuilder.GenerateFromCsv(firstAsString, voClass, underlyingType);
    }

    private static MemberPropertiesCollection FromNewStatements(INamedTypeSymbol voClass, INamedTypeSymbol underlyingType)
    {
        MemberPropertiesCollection l = new();

        var publicStaticMembers = voClass.GetMembers().Where(m => m.IsStatic && m.IsPublic());
        
        var hits = publicStaticMembers.Where(IsSameSymbol);

        int indexForImplicitValues = 0;
        
        // we have the field
        foreach (ISymbol eachMemberSymbol in hits)
        {
            var decl = eachMemberSymbol.DeclaringSyntaxReferences.SingleOrDefaultNoThrow();
            
            if (decl is null)
            {
                continue;
            }

            var syntax = decl.GetSyntax();

            var allConstructors = syntax.DescendantNodes().OfType<BaseObjectCreationExpressionSyntax>();
            
            var newExpressions = allConstructors.ToList();

            List<ImplicitObjectCreationExpressionSyntax> implicitNews = newExpressions.OfType<ImplicitObjectCreationExpressionSyntax>()
                .Where(x => IsMatch(x, voClass.Name)).ToList();
                
            IEnumerable<BaseObjectCreationExpressionSyntax> explicitNews = newExpressions.OfType<ObjectCreationExpressionSyntax>()
                .Where(c => c.Type is IdentifierNameSyntax ins && ins.Identifier.Text == voClass.Name);
            
            var combined = implicitNews.Concat(explicitNews);

            BaseObjectCreationExpressionSyntax? newExpression = combined.FirstOrDefault();

            var args = newExpression?.ArgumentList;
            if (args is null)
            {
                continue;
            }

            // we could have 1 or 2 arguments here.
            // if it's two, then the first is the "enum name", and the second is the value (the "Field Name" is inferred from the syntax)
            // if it's just one, then the "Field Name" is the "enum name", and the value is inferred from the syntax.

            string fieldName = eachMemberSymbol.Name; // todo: get the field name from the syntax
            string enumName = fieldName;

            bool explicitlyNamed = false;
            if (args.Arguments.Count > 2)
            {
                continue;
            }

            if (args.Arguments.Count == 0)
            {
                if (underlyingType.SpecialType is not (SpecialType.System_Int32 or SpecialType.System_String))
                {
                    l.Add(DiagnosticsCatalogue.MemberMethodCallCanOnlyOmitValuesForStringsAndInts(voClass), eachMemberSymbol.Locations[0]);
                    continue;
                }

                string value;

                if (underlyingType.SpecialType is SpecialType.System_String)
                {
                    value = $"\"{enumName}\"";
                }
                else
                {
                    value = indexForImplicitValues.ToString();
                    indexForImplicitValues += 1;
                }

                enumName = fieldName;

                l.Add(new MemberProperties(MemberSource.FromNewExpression, fieldName, enumName, value, value, "", false));
                continue;
            }

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
            // could look up "Fred" and return the member with the field name "Item1"

            
            // in the static constructor, we can enumerate these and sets the values on the fields - it just means
            // making FieldName and EnumName with private setters
            
            // actually, none of this will work - an explicit field *must* specify the field name, the enum name, and the value
            // we can probably infer the enum name from the field name with a constructor overload, but that's it.

            l.Add(
                new MemberProperties(
                    MemberSource.FromNewExpression,
                    fieldName,
                    enumName,
                    secondAsString,
                    secondAsString,
                    "",
                    explicitlyNamed));
        }

        return l;

        bool IsSameSymbol(ISymbol m)
        {
            return SymbolEqualityComparer.Default.Equals(m.GetMemberType(), voClass);
        }
    }

    private static bool IsMatch(ImplicitObjectCreationExpressionSyntax ine, string expectedType)
    {
        var fieldDeclarationSyntax = ine.Ancestors().OfType<FieldDeclarationSyntax>().FirstOrDefault();
        
        if (fieldDeclarationSyntax is null)
        {
            return false;
        }

        string x = fieldDeclarationSyntax.Declaration.Type.ToString();
        return x == expectedType;
    }

    private static bool IsCallingMember(InvocationExpressionSyntax arg, string methodName)
    {
        IEnumerable<IdentifierNameSyntax> nodes = arg.DescendantNodes().OfType<IdentifierNameSyntax>();

        var v = nodes.Where(n => n.Identifier.ToString() == methodName).SingleOrDefaultNoThrow();
        
        return v is not null;
    }
}

public enum MemberSource
{
    FromAttribute,
    FromMemberMethod,
    FromNewExpression
}    

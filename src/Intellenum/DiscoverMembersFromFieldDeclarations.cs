using System;
using System.Collections.Generic;
using System.Linq;
using Intellenum.Diagnostics;
using Intellenum.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ISymbolExtensions = Intellenum.Extensions.ISymbolExtensions;

namespace Intellenum;

internal class DiscoverMembersFromFieldDeclarations
{
    internal static MemberPropertiesCollection Discover(INamedTypeSymbol ieSymbol,
        INamedTypeSymbol underlyingSymbol,
        Counter counter)
    {
        MemberPropertiesCollection l = new();

        var publicStaticMembers = ieSymbol.GetMembers().Where(m => m.IsStatic && m.IsPublic() && m is IFieldSymbol);

        var hits = publicStaticMembers.Where(IsMemberTypeSameAsUnderlyingType);

        // we have the field
        foreach (ISymbol eachMemberSymbol in hits)
        {
            var decl = eachMemberSymbol.DeclaringSyntaxReferences.SingleOrDefaultNoThrow();

            if (decl is null)
            {
                continue;
            }

            // We don't care, here, about stuff that is created using new, as that is done elsewhere
            // in this type.
            var syntax = decl.GetSyntax();
            var newExpressions = syntax.DescendantNodes().OfType<BaseObjectCreationExpressionSyntax>().ToList();
            if (newExpressions.Count > 0)
            {
                ProcessNewExpressions(newExpressions, ieSymbol, l, counter, eachMemberSymbol, underlyingSymbol);
            }
            else
            {
                ProcessNonCreatedExpressions(syntax, underlyingSymbol, counter, l, underlyingSymbol, ieSymbol);
            }
        }

        return l;

        bool IsMemberTypeSameAsUnderlyingType(ISymbol m)
        {
            return SymbolEqualityComparer.Default.Equals(m.GetMemberType(), ieSymbol);
        }
    }

    private static void ProcessNonCreatedExpressions(SyntaxNode syntax,
        INamedTypeSymbol underlyingSymbol,
        Counter counter,
        MemberPropertiesCollection l,
        ISymbol eachMemberSymbol,
        INamedTypeSymbol ieSymbol)
    {
        string enumName = syntax.GetFirstToken().ValueText;

        if (underlyingSymbol.SpecialType is not (SpecialType.System_Int32 or SpecialType.System_String))
        {
            l.Add(DiagnosticsCatalogue.MemberMethodCallCanOnlyOmitValuesForStringsAndInts(ieSymbol), eachMemberSymbol.Locations[0]);
            return;
        }

        string value;

        if (underlyingSymbol.SpecialType is SpecialType.System_String)
        {
            value = $"\"{enumName}\"";
        }
        else
        {
            value = counter.Increment().ValueAsText;
        }

        l.Add(new MemberProperties(MemberSource.FromFieldDeclator, enumName, enumName, value, value, "", false, false));
    }

    private static void ProcessNewExpressions(List<BaseObjectCreationExpressionSyntax> newExpressions,
        INamedTypeSymbol ieSymbol,
        MemberPropertiesCollection l,
        Counter counter,
        ISymbol eachMemberSymbol,
        INamedTypeSymbol underlyingSymbol)
    {
        List<ImplicitObjectCreationExpressionSyntax> implicitNews = newExpressions.OfType<ImplicitObjectCreationExpressionSyntax>()
            .Where(x => IsMatch(x, ieSymbol.Name)).ToList();

        IEnumerable<BaseObjectCreationExpressionSyntax> explicitNews = newExpressions.OfType<ObjectCreationExpressionSyntax>()
            .Where(c => c.Type is IdentifierNameSyntax ins && ins.Identifier.Text == ieSymbol.Name);

        var combined = implicitNews.Concat(explicitNews);

        BaseObjectCreationExpressionSyntax? newExpression = combined.FirstOrDefault();

        var args = newExpression?.ArgumentList;
        if (args is null)
        {
            return;
        }

        // we could have 1 or 2 arguments here.
        // if it's two, then the first is the "enum name", and the second is the value (the "Field Name" is inferred from the syntax)
        // if it's just one, then the "Field Name" is the "enum name", and the value is inferred from the syntax.

        string fieldName = eachMemberSymbol.Name;
        string enumName = fieldName;

        bool explicitlyNamed = false;
        if (args.Arguments.Count > 2)
        {
            return;
        }

        if (args.Arguments.Count == 0)
        {
            if (underlyingSymbol.SpecialType is not (SpecialType.System_Int32 or SpecialType.System_String))
            {
                l.Add(DiagnosticsCatalogue.MemberMethodCallCanOnlyOmitValuesForStringsAndInts(ieSymbol), eachMemberSymbol.Locations[0]);
                return;
            }

            string value;

            if (underlyingSymbol.SpecialType is SpecialType.System_String)
            {
                value = $"\"{enumName}\"";
            }
            else
            {
                value = counter.Increment().ValueAsText;
            }

            enumName = fieldName;

            l.Add(new MemberProperties(MemberSource.FromNewExpression, fieldName, enumName, value, value, "", false, false));
            return;
        }
        if (args.Arguments.Count == 2)
        {
            explicitlyNamed = true;
            ArgumentSyntax first = args.Arguments[0];

            var firstAsString = (first.Expression as LiteralExpressionSyntax)?.Token.Value as string;

            enumName = firstAsString ?? throw new InvalidOperationException(
                $"Expected string literal as name parameter to a parameter of the constructor for creating a type of '{ieSymbol.Name}'");
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
                explicitlyNamed,
                true));
        
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
}
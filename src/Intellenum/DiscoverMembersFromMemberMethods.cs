using System.Collections.Generic;
using System.Linq;
using Intellenum.Diagnostics;
using Intellenum.Extensions;
using Intellenum.MemberBuilding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ISymbolExtensions = Intellenum.Extensions.ISymbolExtensions;

namespace Intellenum;

internal class DiscoverMembersFromMemberMethods
{
    public static MemberPropertiesCollection Discover(INamedTypeSymbol ieSymbol,
        INamedTypeSymbol underlyingSymbol,
        Compilation compilation,
        Counter counter)
    {
        return MemberPropertiesCollection.Combine(
            FromCallsToMemberMethod(ieSymbol, underlyingSymbol, compilation, counter),
            FromTheSingleCallToMembersMethod(ieSymbol, underlyingSymbol, counter));
    }


    private static IEnumerable<InvocationExpressionSyntax> GetInvocationsInConstructor(INamedTypeSymbol voClass, string methodName)
    {
        var constructor = voClass.Constructors.FirstOrDefault(m => m.IsStatic && ISymbolExtensions.IsPrivate(m));

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
        Compilation compilation,
        Counter indexForImplicitValues)
    {
        MemberPropertiesCollection l = new();

        var memberInvocations = GetInvocationsInConstructor(voClass, "Member");

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
                    secondAsString = $"{indexForImplicitValues.Increment().ValueAsText}";
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
                    wasExplicitlySetAName: isExplicitlyNamed,
                    wasExplicitlySetAValue: true));
        }

        return l;
    }

    private static MemberPropertiesCollection FromTheSingleCallToMembersMethod(INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType,
        Counter indexForImplicitValues)
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

        return MemberBuilder.GenerateFromCsv(firstAsString, voClass, underlyingType, indexForImplicitValues);
    }
    
    private static bool IsCallingMember(InvocationExpressionSyntax arg, string methodName)
    {
        IEnumerable<IdentifierNameSyntax> nodes = arg.DescendantNodes().OfType<IdentifierNameSyntax>();

        var v = nodes.Where(n => n.Identifier.ToString() == methodName).SingleOrDefaultNoThrow();

        return v is not null;
    }
    
}
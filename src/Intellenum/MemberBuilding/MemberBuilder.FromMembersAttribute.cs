using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static partial class MemberBuilder
{
    public static IEnumerable<MemberProperties?> TryBuildFromMembersCsv(AttributeData matchingAttribute,
        SourceProductionContext context,
        INamedTypeSymbol voClass,
        INamedTypeSymbol? underlyingType)
    {
        // try build it from non-named arguments

        var constructorArgs = matchingAttribute.ConstructorArguments;

        if (!constructorArgs.IsEmpty)
        {
            // make sure we don't have any errors
            ImmutableArray<TypedConstant> args = constructorArgs;

            if (HasAnyErrors(args))
            {
                return [];
            }

            return TryBuild(args[0], voClass, context, underlyingType);
        }

        // try build it from named arguments
        var namedArgs = matchingAttribute.NamedArguments;

        if (namedArgs.IsEmpty)
        {
            return [];
        }

        if (namedArgs.Any(a => a.Value.Kind == TypedConstantKind.Error))
        {
            return [];
        }

        TypedConstant nameConstant = default;

        foreach (KeyValuePair<string, TypedConstant> arg in namedArgs)
        {
            TypedConstant typedConstant = arg.Value;

            switch (arg.Key)
            {
                case "name":
                    nameConstant = typedConstant;
                    break;
            }
        }

        return TryBuild(nameConstant, voClass, context, underlyingType);
    }    
    
    
    private static IEnumerable<MemberProperties?> TryBuild(
        TypedConstant namesConstant,
        INamedTypeSymbol voClass,
        SourceProductionContext context,
        INamedTypeSymbol? underlyingType)
    {
        bool hasErrors = false;
        if (namesConstant.Value is null)
        {
            context.ReportDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass));
            hasErrors = true;
        }

        if (hasErrors)
        {
            yield break;
        }

        var csv = (string?)namesConstant.Value;
        
        if(csv is null)
        {
            yield break;
        }

        var names = csv.Split(',').Select(each => each.Trim());

        int i = 0;
        
        foreach (var eachName in names)
        {
            MemberGeneration.BuildResult r = MemberGeneration.TryBuildMemberValueAsText(
                eachName,
                i.ToString(),
                underlyingType?.FullName());
            
            if (!r.Success)
            {
                context.ReportDiagnostic(DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage));
                yield return null;
            }
            
            yield return new MemberProperties(
                MemberSource.FromAttribute,
                eachName,
                eachName,
                r.Value,
                r,
                string.Empty);

            i++;
        }
    }
}
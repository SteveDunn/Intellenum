using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static partial class MemberBuilder
{
    public static ValuesOrDiagnostic<MemberProperties?> TryBuildFromMembersCsv(AttributeData matchingAttribute,
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
                return ValuesOrDiagnostic<MemberProperties?>.WithNoValues();
            }

            return TryBuild(args[0], voClass, underlyingType);
        }

        // try build it from named arguments
        var namedArgs = matchingAttribute.NamedArguments;

        if (namedArgs.IsEmpty)
        {
            return ValuesOrDiagnostic<MemberProperties?>.WithNoValues();
        }

        if (namedArgs.Any(a => a.Value.Kind == TypedConstantKind.Error))
        {
            return ValuesOrDiagnostic<MemberProperties?>.WithNoValues();
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

        return TryBuild(nameConstant, voClass, underlyingType);
    }    
    
    
    private static ValuesOrDiagnostic<MemberProperties?> TryBuild(
        TypedConstant namesConstant,
        INamedTypeSymbol voClass,
        INamedTypeSymbol? underlyingType)
    {
        List<MemberProperties> result = new();
        
        if (namesConstant.Value is null)
        {
            //context.ReportDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass));
            return ValuesOrDiagnostic<MemberProperties?>.WithDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass));
        }

        var csv = (string?)namesConstant.Value;
        
        if(csv is null)
        {
            return ValuesOrDiagnostic<MemberProperties?>.WithNoValues();
        }

        var names = csv.Split(',').Select(each => each.Trim());

        int i = 0;
        
        foreach (var eachName in names)
        {
            string value = ResolveValue();

            MemberGeneration.BuildResult r = MemberGeneration.TryBuildMemberValueAsText(
                eachName,
                value,
                underlyingType?.FullName());

            if (!r.Success)
            {
//                context.ReportDiagnostic(DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage));
                return ValuesOrDiagnostic<MemberProperties?>.WithDiagnostic(DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage));
            }

            result.Add(new MemberProperties(
                MemberSource.FromAttribute,
                eachName,
                eachName,
                r.Value,
                r,
                string.Empty));

            i++;
            continue;

            string ResolveValue()
            {
                if(underlyingType?.SpecialType is SpecialType.System_Int32)
                    return i.ToString();
                if(underlyingType?.SpecialType is SpecialType.System_String)
                    return eachName;

                Debug.Fail("Not a string or int");

                return i.ToString();
            }
        }

        return ValuesOrDiagnostic<MemberProperties?>.WithValue(result);
    }
}
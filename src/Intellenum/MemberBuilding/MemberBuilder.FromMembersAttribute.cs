using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static partial class MemberBuilder
{
    public static MemberPropertiesCollection TryBuildFromMembersFromCsvInAttribute(
        AttributeData matchingAttribute,
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
                return MemberPropertiesCollection.Empty();
            }

            return TryBuild(args[0], voClass, underlyingType);
        }

        // try build it from named arguments
        var namedArgs = matchingAttribute.NamedArguments;

        if (namedArgs.IsEmpty)
        {
            return MemberPropertiesCollection.Empty();
        }

        if (namedArgs.Any(a => a.Value.Kind == TypedConstantKind.Error))
        {
            return MemberPropertiesCollection.Empty();
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


    private static MemberPropertiesCollection TryBuild(
        TypedConstant namesConstant,
        INamedTypeSymbol voClass,
        INamedTypeSymbol? underlyingType)
    {
        if (namesConstant.Value is null)
        {
            return MemberPropertiesCollection.WithDiagnostic(
                DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass), voClass.Locations[0]);
        }

        var csv = (string?) namesConstant.Value;

        if (csv is null)
        {
            return MemberPropertiesCollection.Empty();
        }

        return GenerateFromCsv(csv, voClass, underlyingType);
    }

    public static MemberPropertiesCollection GenerateFromCsv(string csv, INamedTypeSymbol voClass, INamedTypeSymbol? underlyingType)
    {
        var names = csv.Split(',').Select(each => each.Trim());

        List<ValueOrDiagnostic<MemberProperties>> result = new();

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
                return MemberPropertiesCollection.WithDiagnostic(
                    DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage), voClass.Locations[0]);
            }

            result.Add(
                ValueOrDiagnostic<MemberProperties>.WithValue(
                    new MemberProperties(
                        MemberSource.FromAttribute,
                        eachName,
                        eachName,
                        r.Value,
                        r,
                        string.Empty,
                        true)));

            i++;
            continue;

            string ResolveValue()
            {
                if (underlyingType?.SpecialType is SpecialType.System_Int32)
                    return i.ToString();
                if (underlyingType?.SpecialType is SpecialType.System_String)
                    return eachName;

                Debug.Fail("Not a string or int");

                return i.ToString();
            }
        }

        return new MemberPropertiesCollection(result);
    }
}
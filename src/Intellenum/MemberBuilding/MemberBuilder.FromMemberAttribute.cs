using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static partial class MemberBuilder
{
    public static ValueOrDiagnostic<MemberProperties>? BuildFromMemberAttribute(
        AttributeData matchingAttribute,
        INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType,
        Func<string, object>? defaultValueGetter = null)
    {
        // try build it from non-named arguments

        var constructorArgs = matchingAttribute.ConstructorArguments;

        if (!constructorArgs.IsEmpty)
        {
            // make sure we don't have any errors
            ImmutableArray<TypedConstant> args = constructorArgs;

            if (HasAnyErrors(args))
            {
                return null;
            }

            var name = args[0];
            TypedConstant? value = args.Length > 1 ? args[1] : null;
            TypedConstant? comment = args.Length > 2 ? args[2] : null;

            return TryBuild(name, value, comment, voClass, underlyingType, defaultValueGetter);
        }

        // try build it from named arguments
        var namedArgs = matchingAttribute.NamedArguments;

        if (namedArgs.IsEmpty)
        {
            return null;
        }

        if (namedArgs.Any(a => a.Value.Kind == TypedConstantKind.Error))
        {
            return null;
        }

        TypedConstant nameConstant = default;
        TypedConstant valueConstant = default;
        TypedConstant commentConstant = default;

        foreach (KeyValuePair<string, TypedConstant> arg in namedArgs)
        {
            TypedConstant typedConstant = arg.Value;

            switch (arg.Key)
            {
                case "name":
                    nameConstant = typedConstant;
                    break;
                case "value":
                    valueConstant = typedConstant;
                    break;
                case "tripleSlashComments":
                    commentConstant = typedConstant;
                    break;
            }
        }

        return TryBuild(nameConstant, valueConstant, commentConstant, voClass, underlyingType, defaultValueGetter);
    }

    internal static MemberPropertiesCollection BuildFromMemberAttributes(IEnumerable<AttributeData> matchingAttributes,
        INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType,
        Counter counter)
    {
        // try build it from non-named arguments

        MemberPropertiesCollection c = new MemberPropertiesCollection();

        foreach (AttributeData e in matchingAttributes)
        {
            ValueOrDiagnostic<MemberProperties>? m = BuildFromMemberAttribute(
                e,
                voClass,
                underlyingType,
                name =>
                {
                    if (underlyingType?.SpecialType is SpecialType.System_String)
                    {
                        return name;
                    }

                    return counter.Increment().Value;
                });

            if (m is not null)
            {
                c.Add(m);
            }
        }

        return c;
    }

    
    private static ValueOrDiagnostic<MemberProperties> TryBuild(TypedConstant nameConstant,
        TypedConstant? valueConstant,
        TypedConstant? commentConstant,
        INamedTypeSymbol voClass,
        INamedTypeSymbol underlyingType,
        Func<string, object>? defaultValueGetter)
    {
        if (nameConstant.Value is null)
        {
            return ValueOrDiagnostic<MemberProperties>.WithDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentName(voClass), voClass.Locations[0]);
        }

        var name = (string) nameConstant.Value!;

        if (valueConstant is null && !underlyingType.SpecialType.IsStringOrInt())
        {
            return ValueOrDiagnostic<MemberProperties>.WithDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentValue(voClass), voClass.Locations[0]);
        }

        // For the `Member` attribute, if a value isn't supplied, then it's not an implicit
        // value per-se, as the code for the entry is emitted so it *doesn't need to set in the static constructor*.
        // If this were set, then the static constructor would run before the static field is initialized, and a null is thrown
        // as reported in https://github.com/SteveDunn/Intellenum/issues/142
        var isExplicit = true;

        object? value = valueConstant?.Value ?? defaultValueGetter?.Invoke(name);
        if (value is null)
        {
            return ValueOrDiagnostic<MemberProperties>.WithDiagnostic(DiagnosticsCatalogue.MemberMethodCallCannotHaveNullArgumentValue(voClass), voClass.Locations[0]);
          //  hasErrors = true;
        }

        MemberGeneration.BuildResult r = MemberGeneration.TryBuildMemberValueAsText(
            name,
            value,
            underlyingType?.FullName());

        if (!r.Success)
        {
            return ValueOrDiagnostic<MemberProperties>.WithDiagnostic(DiagnosticsCatalogue.MemberValueCannotBeConverted(voClass, r.ErrorMessage), voClass.Locations[0]);
        }

        return ValueOrDiagnostic<MemberProperties>.WithValue(new MemberProperties(
            source: MemberSource.FromAttribute,
            fieldName: name,
            enumFriendlyName: name,
            valueAsText: r.Value,
            value: value,
            tripleSlashComments: (string) (commentConstant?.Value ?? string.Empty),
            wasExplicitlySetAName: isExplicit,
            wasExplicitlySetAValue: true));
    }
}
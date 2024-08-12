using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Intellenum.Diagnostics;
using Intellenum.MemberBuilding;
using Microsoft.CodeAnalysis;

namespace Intellenum;

internal static class DiscoverMembersFromAttributes
{
    public static MemberPropertiesCollection Discover(INamedTypeSymbol ieSymbol, INamedTypeSymbol underlyingSymbol, Counter counter)
    {
        var allAttributes = ieSymbol.GetAttributes();

        return MemberPropertiesCollection.Combine(
            FromMemberAttributes(allAttributes, ieSymbol, underlyingSymbol, counter),
            FromMembersAttribute(allAttributes, ieSymbol, underlyingSymbol, counter));
    }

    private static MemberPropertiesCollection FromMemberAttributes(ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol ieSymbol,
        INamedTypeSymbol underlyingType,
        Counter counter)
    {
        var matchingAttributes = FilterToAttributesNamed(attributes, "Intellenum.MemberAttribute");

        return MemberBuilder.BuildFromMemberAttributes(matchingAttributes, ieSymbol, underlyingType, counter);
    }

    private static IEnumerable<AttributeData> FilterToAttributesNamed(ImmutableArray<AttributeData> attributes, string name) =>
        attributes.Where(a => a.AttributeClass?.FullName() == name);

    private static MemberPropertiesCollection FromMembersAttribute(ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol ieSymbol,
        INamedTypeSymbol underlyingSymbol,
        Counter counter)
    {
        var matchingAttributes = FilterToAttributesNamed(attributes, "Intellenum.MembersAttribute").ToList();

        if (matchingAttributes.Count != 1)
        {
            return MemberPropertiesCollection.Empty();
        }

        AttributeData matchingAttribute = matchingAttributes[0];

        if (!underlyingSymbol.SpecialType.IsStringOrInt())
        {
            return MemberPropertiesCollection.WithDiagnostic(
                DiagnosticsCatalogue.MembersAttributeShouldOnlyBeOnIntOrStringBasedEnums(ieSymbol),
                ieSymbol.Locations[0]);
        }

        return MemberBuilder.TryBuildFromMembersFromCsvInAttribute(matchingAttribute, ieSymbol, underlyingSymbol, counter);
    }
}
using Microsoft.CodeAnalysis;

namespace Intellenum;

internal class DiscoverMembers
{
    public static MemberPropertiesCollection Discover(INamedTypeSymbol ieSymbol,
        INamedTypeSymbol underlyingSymbol,
        Compilation compilation)
    {
        Counter counter = new();

        return MemberPropertiesCollection.Combine(
            DiscoverMembersFromAttributes.Discover(ieSymbol, underlyingSymbol, counter),
            DiscoverMembersFromMemberMethods.Discover(ieSymbol, underlyingSymbol, compilation, counter),
            DiscoverMembersFromFieldDeclarations.Discover(ieSymbol, underlyingSymbol, counter));
    }
}
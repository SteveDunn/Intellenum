using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Intellenum.MemberBuilding;

public static partial class MemberBuilder
{
    private static bool HasAnyErrors(ImmutableArray<TypedConstant> args)
    {
        if (args.Any(arg => arg.Kind == TypedConstantKind.Error))
        {
            return true;
        }

        return false;
    }
}
using System.Threading.Tasks;
using Intellenum;
using Shared;
using VerifyXunit;
// ReSharper disable CheckNamespace

namespace SnapshotTests.BugFixTests.BugFix149;

[UsesVerify] 
public class BugFix149
{
    [Fact]
    public Task Non_IComparable_underlyings_of_a_record_do_not_make_the_enum_IComparable()
    {
        var source = """
#nullable disable
using Intellenum;

namespace Whatever;

public record Size(int Width, int Height, string Unit);

[Intellenum<Size>]
public partial class E
{
    public static readonly E Something = new(new(1, 1, ""));
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task IComparableOfT_underlyings_of_a_record_do_make_the_enum_IComparableOfT_and_IComparable()
    {
        var source = """
#nullable disable
using System;
using Intellenum;


namespace Whatever;

public record Size(int Width, int Height, string Unit) : IComparable<Size>
{
    public int CompareTo(Size s) => throw null!;
}

[Intellenum<Size>]
public partial class E
{
    public static readonly E Something = new(new(1, 1, ""));
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task IComparable_underlyings_of_a_record_do_make_the_enum_IComparable()
    {
        var source = """
#nullable disable
using System;
using Intellenum;


namespace Whatever;

public record Size(int Width, int Height, string Unit) : IComparable
{
    public int CompareTo(Object o) => throw null!;
}

[Intellenum<Size>]
public partial class E
{
    public static readonly E Something = new(new(1, 1, ""));
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);
    }
}
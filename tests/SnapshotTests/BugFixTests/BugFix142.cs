using System.Threading.Tasks;
using Intellenum;
using Shared;
using VerifyXunit;
// ReSharper disable CheckNamespace

namespace SnapshotTests.BugFixTests.BugFix142;

[UsesVerify] 
public class BugFix142
{
    [Fact]
    public Task With_just_one_member_attribute()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
[Member("One")]
public partial class AttributeBasedEnum;
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task With_an_inferred_name_and_value_field_for_a_string()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
public partial class OneMemberAttributeAndOneImplicitField
{
    public static readonly OneMemberAttributeAndOneImplicitField Two = new();
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task With_an_inferred_name_and_value_field_for_an_int()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<int>]
public partial class OneMemberAttributeAndOneImplicitField
{
    public static readonly OneMemberAttributeAndOneImplicitField One = new();
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task With_a_field_declaration_that_is_not_newed_up()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
public partial class E
{
    public static readonly E One;
    public static readonly E Two = new("Two!");
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .IgnoreInitialCompilationErrors()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task With_field_declarations_that_combine_newed_up_and_non_newed_up()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
public partial class E
{
    public static readonly E One, Two = new("Two!");
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .IgnoreInitialCompilationErrors()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task Ignores_non_field_syntax()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
[Members("London, Paris, Peckham")]
public partial class City
{
    public static readonly City Frankfurt;
    
    public static City RandomMember() => City.Peckham;
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .IgnoreInitialCompilationErrors()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }

    [Fact]
    public Task With_a_field_declaration_for_two_declators()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
public partial class E
{
    public static readonly E One, Two;
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .IgnoreInitialCompilationErrors()
            .WithSource(source)
            .RunOn(TargetFramework.Net8_0);
    }
}
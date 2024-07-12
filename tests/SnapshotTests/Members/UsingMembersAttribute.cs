using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Members;

[UsesVerify] 
public class UsingMembersAttribute
{
    [Fact]
    public Task Can_have_just_a_name_for_ints()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
[Member("Standard")]
[Member("Gold")]
[Member("Diamond")]
public partial class CustomerType
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_have_just_a_name_for_ints_with_named_constructor()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
[Member(name: "Standard")]
[Member(name: "Gold")]
[Member(name: "Diamond")]
public partial class CustomerType
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_have_just_a_name_for_strings()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
[Member("Standard")]
[Member("Gold")]
[Member("Diamond")]
public partial class CustomerType
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_be_applied_to_ints()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
[Members("Standard, Gold, Diamond")]
public partial class CustomerType
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_be_combined_with_member_attributes()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
[Member("OffPeak", 3)]
[Members("Standard, Gold, Diamond")]
public partial class CustomerType
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_names_can_have_reserved_keywords()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum]
                     [Members(names: "@class, @event")]
                     public partial class CSharpSymbol
                     {
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }
}

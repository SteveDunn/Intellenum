using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Members;

[UsesVerify] 
public class UsingMembersMethod
{
    [Fact]
    public Task Can_be_specified_for_ints()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum]
public partial class CustomerType
{
    static CustomerType()
    {
        Members("Standard, Gold, Diamond");
    }
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_be_specified_for_strings()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
public partial class CustomerType
{
    static CustomerType() => Members("Standard, Gold, Diamond");
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
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
public partial class CustomerType
{
    static CustomerType()
    {
        Members("Standard, Gold, Diamond");
    }
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Can_be_combined_with_other_member_attributes_and_one_members_attribute()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
[Member("OffPeak")]
[Members("Elderly, Child, BlueLightWorker")]
public partial class CustomerType
{
    static CustomerType()
    {
        Members("Standard, Gold, Diamond");
    }
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_names_can_have_reserved_keywords()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum]
                     public partial class CSharpSymbol
                     {
                         static CSharpSymbol()
                         {
                             Members("@class, @event");
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }
}

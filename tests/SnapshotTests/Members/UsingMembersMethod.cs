using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Members;

[UsesVerify] 
public class UsingMemberMethods
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
        Member("Standard");
        Member("Gold");
        Member("Diamond");
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
    static CustomerType()
    {
        Member("Standard");
        Member("Gold");
        Member("Diamond");
    }
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
        Member("Standard");
        Member("Gold");
        Member("Diamond");
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
        Member("Standard");
        Member("Gold");
        Member("Diamond");
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
                             Member("@class");
                             Member("@event");
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_values_can_have_parameter_specified()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum]
                     public partial class CustomerType
                     {
                         static CustomerType()
                         {
                             Member(name: "Standard", value: -1);
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_values_can_have_parameter_specified2()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum(typeof(int))]
                     public partial class IeIntWithMembersInvalidAndUnspecified
                     {
                         static IeIntWithMembersInvalidAndUnspecified()
                         {
                             Member(name: "Invalid", value: -1);
                             Member(name: "Unspecified", value: -2);
                     
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_values_can_have_parameter_names_and_type_suffixes()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum<float>]
                     public partial class CustomerType
                     {
                         static CustomerType()
                         {
                             Member("Standard", value: 1.23f);
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_values_can_be_expressions()
    {
        var source = """
                     using System;
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum<DateTimeOffset>]
                     public partial class C
                     {
                         static C()
                         {
                             Member("Item1", DateTimeOffset.Parse("2020-01-01T00:00:00Z"));
                             Member("Item2", DateTimeOffset.Parse("2020-01-02T00:00:00Z"));
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_values_can_have_type_suffixes()
    {
        var source = """
                     using Intellenum;

                     namespace Whatever;

                     [Intellenum<float>]
                     public partial class CSharpSymbol
                     {
                         static CSharpSymbol()
                         {
                             Member(name: "Standard", 1.23f);
                         }
                     }
                     """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }
}

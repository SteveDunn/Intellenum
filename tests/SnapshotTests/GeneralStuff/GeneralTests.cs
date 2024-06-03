using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.GeneralStuff;

[UsesVerify]
public class GeneralTests
{
    [Fact]
    public Task One_parameter_string_instances()
    {
        var source = """
                     using Intellenum;
                     namespace Whatever;

                     [Intellenum]
                     [Members("Normal, Gold, Diamond")]
                     public partial class CustomerType
                     {
                     }
                     """;

        return RunTest(source);
    }

    [Fact]
    public Task Partial_partial_class_created_successfully()
    {
        var source = """
                     using Intellenum;
                     namespace Whatever;

                     [Intellenum]
                     [Member("Normal", 0)]
                     [Member("Gold", 1)]
                     [Member("Diamond", 2)]
                     public partial class CustomerType
                     {
                     }
                     """;

        return RunTest(source);
        
        // static Task RunTest(string source) =>
        //     new SnapshotRunner<IntellenumGenerator>()
        //         .WithSource(source)
        //         .IgnoreFinalCompilationErrors()
        //         .RunOn(TargetFramework.Net8_0);
    }

    private static Task RunTest(string source) =>
        new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreFinalCompilationErrors()
            .RunOnAllFrameworks();

    [Fact]
    public Task No_namespace() =>
        RunTest(
            """
            using Intellenum;

            [Intellenum]
            [Member("Normal", 0)]
            [Member("Gold", 1)]
            [Member("Diamond", 2)]
            public partial class CustomerType
            {
            }
            """);

    [Fact]
    public Task Produces_instances() =>
        RunTest(
            """
            using Intellenum;

            namespace Whatever;

            [Intellenum(typeof(int))]
            [Member(name: "Unspecified", value: -1, tripleSlashComment: "a short description that'll show up in intellisense")]
            [Member(name: "Normal", value: -2)]
            [Member(name: "Gold", value: -3, tripleSlashComment: "<some_xml>whatever</some_xml")]
            [Member(name: "Diamond", value: -4)]
            [Member(name: "Legacy", value: 42)]
            public partial class CustomerType
            {
            }

            """);

    [Fact]
    public Task Namespace_names_can_have_reserved_keywords() =>
        RunTest(
            """
            using Intellenum;

            namespace @double;

            [Intellenum]
            [Member(name: "@struct", value: 42)]
            [Member(name: "@double", value: 52)]
            [Member(name: "@event", value: 69)]
            [Member(name: "@void", value: 666)]
            public partial class @class
            {
            }

            """);
}
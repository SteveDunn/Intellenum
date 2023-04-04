using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace ScratchSnapshotTests
{
    [UsesVerify]
    public class Explicit_instance_tests
    {
        [Fact]
        public Task Explicit_instances_using_new()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    public partial class CustomerType
    {
        public static CustomerType Standard = new CustomerType("Standard", 1);
        public static CustomerType Gold = new CustomerType("Gold", 2);
    }
    """;

            return RunTest(source);
        }

        [Fact]
        public Task Explicit_instances_using_target_typed_new()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    public partial class CustomerType
    {
        public static CustomerType Standard = new("Standard", 1);
        public static CustomerType Gold = new("Gold", 2);
    }
    """;

            return RunTest(source);
        }

        [Fact]
        public Task Explicit_instances_using_Instance_method()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    public partial class CustomerType
    {
        static CustomerType()
        {
            Instance("Gold", 1);
            Instance("Diamond", 2);
        }
    }
    """;

            return RunTest(source);
        }

        [Fact]
        public Task Explicit_instances_using_Instance_attributes()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    [Instance("Standard", 1)]
    [Instance("Gold", 2)]
    [Instance("Platinum", 3)]
    public partial class CustomerType
    {
    }
    """;

            return RunTest(source);
        }

        [Fact]
        public Task Explicit_instances_using_a_mixture_of_mechanisms()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    [Instance("Standard", 1)]
    public partial class CustomerType
    {
        public static CustomerType Gold = new CustomerType("Gold", 2);

        static CustomerType()
        {
            Instance("Diamond", 3);
        }
    }
    """;

            return RunTest(source);
        }

        private static Task RunTest(string source) =>
            new SnapshotRunner<IntellenumGenerator>()
                .WithSource(source)
                .IgnoreInitialCompilationErrors()
                .IgnoreFinalCompilationErrors()
                .RunOnAllFrameworks();
    }
}
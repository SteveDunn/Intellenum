using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace ScratchSnapshotTests
{
    [UsesVerify]
    public class Explicit_instance_tests
    {
        [Fact]
        public Task Explicit_instances()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    [Instance("Platinum", 3)]
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

        private static Task RunTest(string source) =>
            new SnapshotRunner<IntellenumGenerator>()
                .WithSource(source)
                .IgnoreInitialCompilationErrors()
                .IgnoreFinalCompilationErrors()
                .RunOnAllFrameworks();
    }
}
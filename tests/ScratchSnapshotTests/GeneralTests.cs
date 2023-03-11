using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace ScratchSnapshotTests
{
    [UsesVerify]
    public class GeneralTests
    {
        [Fact]
        public Task Partial_struct_created_successfully()
        {
            var source = @"using Intellenum;
namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
[Instance(""Diamond"", 2)]
public partial struct CustomerType
{
}";

            return RunTest(source);
        }

        [Fact]
        public Task Partial_partial_class_created_successfully()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum]
    [Instance("Normal", 0)]
    [Instance("Gold", 1)]
    [Instance("Diamond", 2)]
    public partial class CustomerType
    {
    }
    """;

            return RunTest(source);
        }

        private static Task RunTest(string source) =>
            new SnapshotRunner<IntellenumGenerator>()
                .WithSource(source)
                .IgnoreFinalCompilationErrors()
                .RunOnAllFrameworks();
    }
}
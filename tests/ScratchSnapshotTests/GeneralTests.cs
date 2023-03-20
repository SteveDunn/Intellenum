using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace ScratchSnapshotTests
{
    [UsesVerify]
    public class GeneralTests
    {
        [Fact]
        public Task dev_test1()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum<string>]
    [Instance("Normal", "n")]
    [Instance("Gold", "g")]
    [Instance("Diamond", "d")]
    public partial class CustomerType
    {
    }
    """;

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task dev_test2()
        {
            var source = """
    using Intellenum;
    namespace Whatever;


    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(string))]
    public partial class public_partial_classConversions_NewtonsoftJsonstring { 

        static public_partial_classConversions_NewtonsoftJsonstring() {
            Instance("One", "1");
        }
    }
    """;

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task int_created_successfully()
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

        [Fact]
        public Task string_created_successfully()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum<string>]
    [Instance("Normal", "n")]
    [Instance("Gold", "g")]
    [Instance("Diamond", "d")]
    public partial class CustomerType
    {
    }
    """;

            return RunTest(source);
        }
        
        [Fact]
        public Task float_created_successfully()
        {
            var source = """
                using System;
                using Intellenum;
                
                [assembly: IntellenumDefaults(conversions: Conversions.DapperTypeHandler)]
                
                namespace Whatever;
                
                [Intellenum]
                [Instance("Normal", 0)]
                [Instance("Gold", 1)]
                public partial class CustomerType
                {
                }

                """;

            return RunTest(source);
        }

        private static Task RunTest(string source) =>
            new SnapshotRunner<IntellenumGenerator>()
                .WithSource(source)
                // .IgnoreFinalCompilationErrors()
                .RunOnAllFrameworks();
    }
}
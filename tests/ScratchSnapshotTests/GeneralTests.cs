using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace ScratchSnapshotTests
{
    [UsesVerify]
    public class GeneralTests
    {
        [Fact]
        public Task custom_type_literal_new3()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

[Intellenum<Foo>]
public partial class FooEnum
{
    public static readonly FooEnum Item1 = new("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2=  new("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type_literal_new2()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

[Intellenum<Foo>]
public partial class FooEnum
{
    // just for the test - it's generated in real life
    // public FooEnum(Foo name, int value) { }


    public static readonly FooEnum Item1 = new("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2 = new("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type_literal_new()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

[Intellenum<Foo>]
public partial class FooEnum
{
    public static readonly FooEnum Item1 = new("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2= new("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

[Intellenum<Foo>]
public partial class FooEnum
{
    public static readonly FooEnum Item1 = new FooEnum("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2= new FooEnum("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }
        
        [Fact]
        public Task guids()
        {
            var source = """
    using Intellenum;
    using System;
    namespace Whatever;

    [Intellenum<System.Guid>]
    public partial class NotableGuids
    {
        static NotableGuids()
        {
            Instance("IntellenumProject", new System.Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"));
            Instance("SnapshotTestsProject", new System.Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"));
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
        public Task decimals()
        {
            var source = """
    using Intellenum;
    namespace Whatever;

    [Intellenum<decimal>]
    public partial class MinimumWageInUK
    {
        static MinimumWageInUK()
        {
            Instance("Apprentice", 4.3m);
            Instance("UnderEighteen", 4.62m);
            Instance("EighteenToTwenty", 6.56m);
            Instance("TwentyOneAndOver", 8.36m);
            Instance("TwentyFiveAndOver", 8.91m);
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
                
                [Intellenum<float>]
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
                .IgnoreFinalCompilationErrors()
                .RunOnAllFrameworks();
    }
}
using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests
{
    [UsesVerify]
    public class GeneralTests
    {
        [Fact]
        public Task test_int_again()
        {
            var source = """
using System;
using Intellenum;

namespace record.@struct.@float
{
    public readonly record struct @decimal() : IComparable<@decimal>
    {
        public int CompareTo(@decimal other) => throw new NotImplementedException();
    }
    
}

namespace SomethingElse
{

    [Intellenum(typeof(record.@struct.@float.@decimal))]
    public partial class @event2
    {
        static @event2()
        {
            Member("One", new record.@struct.@float.@decimal());
            Member("Two", new record.@struct.@float.@decimal());
        }
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
        public Task test_short()
        {
            var source = """
using System;
using Intellenum;

namespace SomethingElse
{
    [Intellenum(underlyingType: typeof(short))]
    [Member("Item1", 123)]
    [Member("Item2", 321)]
    public partial class ShortHolderId_normal
    {
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
        public Task test_short_id_as_string()
        {
            var source = """
using System;
using Intellenum;

namespace SomethingElse
{
    [Intellenum(underlyingType: typeof(short), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Member("Item1", 123)]
    [Member("Item2", 321)]
    public partial class ShortHolderId_string
    {
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
        public Task date_time_offset()
        {
            var source = """
using System;
using Intellenum;

namespace SomethingElse
{
    [Intellenum(underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Member("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    //.IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task Member_methods_with_more_complex_expressions()
        {
            var source = """
using System;
using Intellenum;

namespace SomethingElse
{
    [Intellenum(underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero).AddTicks(123));
        }
    }
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    //.IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task test_datetimeoffset()
        {
            var source = """
using System;
using Intellenum;

    [Intellenum(underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Member("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
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
        public Task test_bool()
        {
            var source = """
using System;
using Intellenum;

    [Intellenum(underlyingType: typeof(bool))]
    [Member("No", false)]
    [Member("Yes", true)]
    public partial class BoolVo { }
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task test1()
        {
            var source = """
using System;
using Intellenum;

[Intellenum(underlyingType: typeof(decimal), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
public partial class DecimalHolderId_string
{
    static DecimalHolderId_string()
    {
        Member("Item1", 720742592373919744m);
        Member("Item2", 2.2m);
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

public class Tester
{
public FooEnum _myField = FooEnum.Item1;
}
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type_literal_new4()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

    [Intellenum(underlyingType: typeof(Bar))]
    public partial class FooVo
    {
        public static readonly FooVo Fred = new FooVo("Item1", new Bar(42, "Fred"));
        public static readonly FooVo Wilma = new FooVo("Item2", new Bar(52, "Wilma"));
    }

    public record class Bar(int Age, string Name) : IComparable<Bar>
    {
        public int CompareTo(Bar other) => Age.CompareTo(other.Age);
    }
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type_literal_new5()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

    [Intellenum<Foo>]
    public partial class FooEnum
    {
        public static readonly FooEnum Fred = new FooEnum("Item1", new Foo("Fred", 42));
        public static readonly FooEnum Wilma = new FooEnum("Item2", new Foo("Wilma", 52));
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
        public Task custom_type_literal_new_explicit_field_name()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

    [Intellenum]
    public partial class FooEnum
    {
        public static readonly FooEnum Member1 = new FooEnum(1);
        public static readonly FooEnum Member2 = new FooEnum(2);
        public static readonly FooEnum Member3 = new FooEnum("MEMBER 3!!", 3);
    }
""";

            return new SnapshotRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .IgnoreInitialCompilationErrors()
                    .IgnoreFinalCompilationErrors()
                    .RunOnAllFrameworks();

        }

        [Fact]
        public Task custom_type_literal_new_infer_field_name()
        {
            var source = """
using System;
using Intellenum;

namespace Whatever;

    [Intellenum<Foo>]
    public partial class FooEnum
    {
        public static readonly FooEnum Fred = new FooEnum(new Foo("Fred", 42));
        public static readonly FooEnum Wilma = new FooEnum(new Foo("Wilma", 52));
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
            Member("IntellenumProject", new System.Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"));
            Member("SnapshotTestsProject", new System.Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"));
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
            Member("Apprentice", 4.3m);
            Member("UnderEighteen", 4.62m);
            Member("EighteenToTwenty", 6.56m);
            Member("TwentyOneAndOver", 8.36m);
            Member("TwentyFiveAndOver", 8.91m);
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
    [Member("Normal", "n")]
    [Member("Gold", "g")]
    [Member("Diamond", "d")]
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
            Member("One", "1");
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
    [Member("Normal", 0)]
    [Member("Gold", 1)]
    [Member("Diamond", 2)]
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
    [Member("Normal", "n")]
    [Member("Gold", "g")]
    [Member("Diamond", "d")]
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
                [Member("Normal", 0)]
                [Member("Gold", 1)]
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
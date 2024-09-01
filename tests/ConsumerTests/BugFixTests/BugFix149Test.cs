
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ConvertTypeCheckToNullCheck
#pragma warning disable CS9113 // Parameter is unread.
namespace ConsumerTests.BugFixTests.BugFix149.One
{
    public record Size(int Width, int Height, string Unit);

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something = new(new(1, 1, ""));
    }
    
    public class When_underlying_has_no_comparable_interfaces
    {
        [Fact]
        public void The_enum_does_not_have_any_icomparable_of_T() =>
            (E.Something is IComparable<E>).Should().BeFalse();

        [Fact]
        public void The_enum_does_not_have_any_icomparable() =>
            (E.Something is IComparable).Should().BeFalse();
    }
}

namespace ConsumerTests.BugFixTests.BugFix149.Two
{
    public record Size(int Width, int Height, string Unit) : IComparable
    {
        public int CompareTo(object? obj) => throw null!;
    }

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something = new(new(1, 1, ""));
    }
    
    public class When_underlying_has_IComparable_interface
    {
        [Fact]
        public void The_enum_does_not_have_any_icomparable_of_T() =>
            (E.Something is IComparable<E>).Should().BeFalse();

        [Fact]
        public void The_enum_is_icomparable() =>
            (E.Something is IComparable).Should().BeTrue();
    }
}

namespace ConsumerTests.BugFixTests.BugFix149.Three
{
    public record Size(int Width, int Height, string Unit) : IComparable<Size>
    {
        public int CompareTo(Size? other) => throw null!;
    }

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something = new(new(1, 1, ""));
    }
    
    public class When_underlying_has_IComparableOfT_interface
    {
        [Fact]
        public void The_enum_is_icomparable_of_T() =>
            (E.Something is IComparable<E>).Should().BeTrue();

        [Fact]
        public void The_enum_is_not_icomparable() =>
            (E.Something is IComparable).Should().BeFalse();
    }
}

namespace ConsumerTests.BugFixTests.BugFix149.Four
{
    public record Size(int Width, int Height, string Unit) : IComparable, IComparable<Size>
    {
        public int CompareTo(Size? other) => throw null!;
        public int CompareTo(object? obj) => throw null!;
    }

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something = new(new(1, 1, ""));
    }
    
    public class When_underlying_has_IComparableOfT_and_IComparable_interfaces
    {
        [Fact]
        public void The_enum_is_icomparable_of_T() =>
            (E.Something is IComparable<E>).Should().BeTrue();

        [Fact]
        public void The_enum_is_icomparable() =>
            (E.Something is IComparable).Should().BeTrue();
    }
}

namespace ConsumerTests.BugFixTests.BugFix149.Five
{
    public record Size(int Width, int Height, string Unit) : IComparable, IComparable<Size>
    {
        public int CompareTo(Size? other) => Width.CompareTo(other?.Width);
        public int CompareTo(object? obj) => Width.CompareTo(obj);
    }

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something1 = new(new(1, 1, ""));
        public static readonly E Something2 = new(new(2, 1, ""));
    }
    
    public class When_underlying_has_IComparableOfT_and_IComparable_interfaces
    {
        [Fact]
        public void It_uses_the_underlying_IComparableOfT() => E.Something1.CompareTo(E.Something2).Should().Be(-1);

        [Fact]
        public void It_uses_the_underlying_IComparableOfT_via_the_operators()
        {
            (E.Something1 < E.Something2).Should().BeTrue();
            (E.Something1 <= E.Something2).Should().BeTrue();
            (E.Something1 > E.Something2).Should().BeFalse();
            (E.Something1 >= E.Something2).Should().BeFalse();
        }

        [Fact]
        public void It_uses_the_underlying_IComparable() =>
            E.Something1.CompareTo((object)E.Something2).Should().Be(-1);
    }
}

namespace ConsumerTests.BugFixTests.BugFix149.Six
{
    public class Size(int width, int height, string unit) : IComparable, IComparable<Size>
    {
        public int Width => width;
        public int Height => height;
        public string Unit => unit;
        
        public int CompareTo(Size? other) => Width.CompareTo(other?.Width);
        public int CompareTo(object? obj) => Width.CompareTo(obj);
    }

    [Intellenum<Size>]
    public partial class E
    {
        public static readonly E Something1 = new(new(1, 1, ""));
        public static readonly E Something2 = new(new(2, 1, ""));
    }
    
    public class When_underlying_has_IComparableOfT_and_IComparable_interfaces
    {
        [Fact]
        public void It_uses_the_underlying_IComparableOfT() => E.Something1.CompareTo(E.Something2).Should().Be(-1);

        [Fact]
        public void It_uses_the_underlying_IComparableOfT_via_the_operators()
        {
            (E.Something1 < E.Something2).Should().BeTrue();
            (E.Something1 <= E.Something2).Should().BeTrue();
            (E.Something1 > E.Something2).Should().BeFalse();
            (E.Something1 >= E.Something2).Should().BeFalse();
        }

        [Fact]
        public void It_uses_the_underlying_IComparable() =>
            E.Something1.CompareTo((object)E.Something2).Should().Be(-1);
    }
}

using FluentAssertions.Execution;
using DateTimeOffset = System.DateTimeOffset;

namespace ConsumerTests.MemberMethodsTests;

public class MemberAttributeTests
{
    [Fact]
    public void Type_with_two_members()
    {
        MyIntWithMembersInvalidAndUnspecified.Invalid.Value.Should().Be(-1);
        MyIntWithMembersInvalidAndUnspecified.Unspecified.Value.Should().Be(-2);
    }

    [Fact]
    public void Type_with_underlying_decimal_with_two_members()
    {
        MyDecimalWithMembersInvalidAndUnspecified.Invalid.Value.Should().Be(-1.23m);
        MyDecimalWithMembersInvalidAndUnspecified.Unspecified.Value.Should().Be(-2.34m);
    }

    public class DateTimeTests
    {
        [Fact]
        public void DateTime()
        {
            using var _ = new AssertionScope();
            DateTimeInstance.iso8601_1.Value.Should().Be(new DateTime(2022, 12, 13));
            DateTimeInstance.iso8601_2.Value.Should().Be(new DateTime(2022, 12, 13, 13, 14, 15));
            DateTimeInstance.ticks_as_long.Value.Should().Be(new DateTime(2022, 12, 13));
            // ticks as an Int.MaxValue is 2147483647, which is 2,147,483,647 / 10m, which is ~214 seconds, which 3 minutes, 34 seconds
            DateTimeInstance.ticks_as_int.Value.Should().BeCloseTo(new DateTime(1, 1, 1, 0, 3, 34, 0), TimeSpan.FromTicks(7483647));
            //var l = new DateTimeOffset()
        }
    }

    [Fact]
    public void Basics()
    {
        using var _ = new AssertionScope();

        MyFloatInstance.i1.Value.Should().BeApproximately(1.23f, 0.01f);
        MyFloatInstance.i2.Value.Should().BeApproximately(2.34f, 0.01f);
        MyFloatInstance.i3.Value.Should().BeApproximately(3.45f, 0.01f);
        MyFloatInstance.i4.Value.Should().BeApproximately(2f, 0.01f);

        MyDecimalInstance.i1.Value.Should().Be(1.23m);
        MyDecimalInstance.i2.Value.Should().Be(2.34m);
        MyDecimalInstance.i3.Value.Should().Be(3.45m);
        MyDecimalInstance.i4.Value.Should().Be(2m);

        MyDoubleInstance.i1.Value.Should().BeApproximately(1.23f, 0.01f);
        MyDoubleInstance.i2.Value.Should().BeApproximately(2.34f, 0.01f);
        MyDoubleInstance.i3.Value.Should().BeApproximately(3.45f, 0.01f);
        MyDoubleInstance.i4.Value.Should().BeApproximately(2f, 0.01f);

        MyCharInstance.i1.Value.Should().Be('');
        MyCharInstance.i2.Value.Should().Be('2');
        MyCharInstance.i3.Value.Should().Be('3');

        MyByteInstance.i1.Value.Should().Be(1);
        MyByteInstance.i2.Value.Should().Be((byte)'2');
        MyByteInstance.i3.Value.Should().Be((byte) 3);
    }

}

[Intellenum(typeof(int))]
public partial class MyIntWithMembersInvalidAndUnspecified
{
    static MyIntWithMembersInvalidAndUnspecified()
    {
        Member(name: "Invalid", value: -1);
        Member(name: "Unspecified", value: -2);

    }
}

[Intellenum(typeof(decimal))]
public partial class MyDecimalWithMembersInvalidAndUnspecified
{
    static MyDecimalWithMembersInvalidAndUnspecified()
    {
        Member(name: "Invalid", value: -1.23m);
        Member(name: "Unspecified", value: -2.34m);
    }
}

[Intellenum]
public partial class C
{
    static C()
    {
        Member(name: "Invalid", value: 1);
        Member(name: "Unspecified", value: -2);

    }
}

[Intellenum(typeof(DateTime))]
public partial class DateTimeInstance
{
    static DateTimeInstance()
    {
        Member(name: "iso8601_1", value: DateTime.Parse("2022-12-13"));
        Member(name: "iso8601_2", value: DateTime.Parse("2022-12-13T13:14:15Z"));
        Member(name: "ticks_as_long", value: new DateTime(638064864000000000L));
        Member(name: "ticks_as_int", value: new DateTime(2147483647));

    }
}

[Intellenum(typeof(DateTimeOffset))]
public partial class DateTimeOffsetInstance
{
    static DateTimeOffsetInstance()
    {
        Member(name: "iso8601_1", value: DateTimeOffset.Parse("2022-12-13"));
        Member(name: "iso8601_2", value: DateTimeOffset.Parse("2022-12-13T13:14:15Z"));
        Member(name: "ticks_as_long", value: DateTimeOffset.FromUnixTimeSeconds(638064864000000000L));
        Member(name: "ticks_as_int", value: DateTimeOffset.FromUnixTimeSeconds(2147483647));

    }
}

[Intellenum(typeof(float))]
public partial class MyFloatInstance
{
    static MyFloatInstance()
    {
        Member(name: "i1", value: 1.23f);
        Member(name: "i2", value: 2.34f);
        Member(name: "i3", value: 3.45f);
        Member(name: "i4", value: 2f);

    }
}

[Intellenum(typeof(decimal))]
public partial class MyDecimalInstance
{
    static MyDecimalInstance()
    {
        Member(name: "i1", value: 1.23m);
        Member(name: "i2", value: 2.34m);
        Member(name: "i3", value: 3.45m);
        Member(name: "i4", value: 2m);

    }
}

[Intellenum(typeof(double))]
public partial class MyDoubleInstance
{
    static MyDoubleInstance()
    {
        Member(name: "i1", value: 1.23d);
        Member(name: "i2", value: 2.34d);
        Member(name: "i3", value: 3.45d);
        Member(name: "i4", value: 2d);

    }
}

[Intellenum(typeof(char))]
public partial class MyCharInstance
{
    static MyCharInstance()
    {
        Member(name: "i1", value: (char)1);
        Member(name: "i2", value: '2');
        Member(name: "i3", value: '3');

    }
}

[Intellenum(typeof(byte))]
public partial class MyByteInstance
{
    static MyByteInstance()
    {
        Member(name: "i1", value: 1);
        Member(name: "i2", value: (byte)'2');
        Member(name: "i3", value: 3);

    }
}
namespace ConsumerTests.Members;

public class InstanceFieldTests
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
}
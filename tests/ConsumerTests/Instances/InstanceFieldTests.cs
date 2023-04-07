namespace ConsumerTests.Instances;

public class InstanceFieldTests
{
    [Fact]
    public void Type_with_two_instance_fields()
    {
        MyIntWithTwoInstanceOfInvalidAndUnspecified.Invalid.Value.Should().Be(-1);
        MyIntWithTwoInstanceOfInvalidAndUnspecified.Unspecified.Value.Should().Be(-2);
    }

    [Fact]
    public void Type_with_underlying_decimal_with_two_instance_fields()
    {
        MyDecimalWithTwoInstanceOfInvalidAndUnspecified.Invalid.Value.Should().Be(-1.23m);
        MyDecimalWithTwoInstanceOfInvalidAndUnspecified.Unspecified.Value.Should().Be(-2.34m);
    }
}
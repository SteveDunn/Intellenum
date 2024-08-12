namespace ConsumerTests.BugFixTests.BugFix142.One
{

    [Intellenum<string>]
    [Member("One")]
    public partial class E;

    public class BugFix142Test
    {
        [Fact]
        public void Should_include_member_attribute() =>
            E.One.Value.Should().Be("One");
    }
}

namespace ConsumerTests.BugFixTests.BugFix142.Two
{

    [Intellenum<string>]
    [Member("One")]
    public partial class E
    {
        public static readonly E Two = new();
    }


    public class BugFix142Test
    {
        [Fact]
        public void Should_include_member_attribute() =>
            E.One.Value.Should().Be("One");

        [Fact]
        public void Should_include_field_declaration() => E.Two.Value.Should().Be("Two");
    }
}
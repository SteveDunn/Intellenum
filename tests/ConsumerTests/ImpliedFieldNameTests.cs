namespace ConsumerTests;

public class ImpliedFieldNameTests
{
    [Fact]
    public void General()
    {
        ImpliedFieldName.Member1.Value.Should().Be(1);
        ImpliedFieldName.Member2.Value.Should().Be(2);
        ImpliedFieldName.Member3.Value.Should().Be(3);

        ImpliedFieldName.IsDefined(1).Should().BeTrue();
        ImpliedFieldName.IsDefined(2).Should().BeTrue();
        ImpliedFieldName.IsDefined(3).Should().BeTrue();

        ImpliedFieldName.TryFromName("Member1", out var i1).Should().BeTrue();
        i1.Value.Should().Be(1);

        ImpliedFieldName.TryFromName("MEMBER 3!!", out var i3).Should().BeTrue();
        i3.Value.Should().Be(3);

        ImpliedFieldName.IsDefined(3).Should().BeTrue();
        ImpliedFieldName.IsNameDefined("MEMBER 3!!").Should().BeTrue();
    }
}
    
[Intellenum]
public partial class ImpliedFieldName
{
    public static readonly ImpliedFieldName Member1 = new(1);
    public static readonly ImpliedFieldName Member2 = new(2);
    public static readonly ImpliedFieldName Member3 = new("MEMBER 3!!", 3);
}
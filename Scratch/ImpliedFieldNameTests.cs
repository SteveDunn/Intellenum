using FluentAssertions;
using Intellenum;

namespace Scratch
{
    public class ImpliedFieldNameTests
    {
        [Fact]
        public void General()
        {
            ImpliedFieldName.Instance1.Value.Should().Be(1);
            ImpliedFieldName.Instance2.Value.Should().Be(2);
            ImpliedFieldName.Instance3.Value.Should().Be(3);

            ImpliedFieldName.IsDefined(1).Should().BeTrue();
            ImpliedFieldName.IsDefined(2).Should().BeTrue();
            ImpliedFieldName.IsDefined(3).Should().BeTrue();

            ImpliedFieldName.TryFromName("Instance1", out var i1).Should().BeTrue();
            i1.Value.Should().Be(1);

            ImpliedFieldName.TryFromName("INSTANCE 3!!", out var i3).Should().BeTrue();
            i3.Value.Should().Be(3);

            ImpliedFieldName.IsDefined(3).Should().BeTrue();
            ImpliedFieldName.IsNamedDefined("INSTANCE 3!!").Should().BeTrue();
        }
    }
    
    [Intellenum]
    public partial class ImpliedFieldName
    {
        public static readonly ImpliedFieldName Instance1 = new(1);
        public static readonly ImpliedFieldName Instance2 = new(2);
        public static readonly ImpliedFieldName Instance3 = new("INSTANCE 3!!", 3);
    }
}
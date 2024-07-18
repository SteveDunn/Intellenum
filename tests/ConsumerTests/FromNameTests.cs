using Intellenum.Tests.Types;

namespace ConsumerTests.FromName
{
    [Intellenum(typeof(int))]
    public partial class MyClassInt
    {
        static MyClassInt() => Members("Item1, Item2");
    }

    public class FromNameTests
    {
        [Fact]
        public void SameClassesHaveSameHashCode() => 
            MyClassInt.FromName("Item1").Should().Be(MyClassInt.Item1);

        [Fact]
        public void Throws_if_no_such_member()
        {
            Action a = () => MyClassInt.FromName("Item3");
            a.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyClassInt has no matching members named 'Item3'");
        }
    }
}

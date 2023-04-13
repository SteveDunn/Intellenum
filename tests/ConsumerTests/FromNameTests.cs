using Intellenum.Tests.Types;

namespace ConsumerTests.FromName
{
    [Intellenum(typeof(int))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class MyClassInt
    {
    }

    public class FromNameTests
    {
        [Fact]
        public void SameClassesHaveSameHashCode()
        {
            MyClassInt.FromName("Item1").Should().Be(MyClassInt.Item1);
        }
    }
}

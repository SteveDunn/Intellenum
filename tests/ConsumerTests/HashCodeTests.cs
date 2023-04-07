using Intellenum.Tests.Types;

namespace ConsumerTests.HashCodes
{
    [Intellenum(typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class MyClassInt
    {
    }

    [Intellenum(typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class MyClassInt2
    {
    }

    public class HashCodeTests
    {
        [Fact]
        public void SameClassesHaveSameHashCode()
        {
            MyClassInt.Item1.GetHashCode().Should().Be(MyClassInt.Item1.GetHashCode());

            MyClassInt.Item2.GetHashCode().Should().Be(MyClassInt.Item2.GetHashCode());
        }

        /// <summary>
        /// The same as record structs, GetHashCode only considers the underlying type and not the type itself.
        /// </summary>
        [Fact]
        public void DifferentClassesWithSameUnderlyingTypeAndValueHaveDifferentHashCode()
        {
            MyClassInt.Item1.GetHashCode().Should().NotBe(MyClassInt2.Item1.GetHashCode());

            MyClassInt.Item2.GetHashCode().Should().NotBe(MyClassInt2.Item2.GetHashCode());
        }

        [Fact]
        public void Storing_1()
        {
            var a1 = Age.LegalVotingAge;
            var a2 = Age.LegalDrivingAge;

            var d = new Dictionary<Age, string>
            {
                { a1, "hello1" },
                { a2, "hello2" }
            };

            d.Count.Should().Be(2);

            d[a1].Should().Be("hello1");
            d[a2].Should().Be("hello2");
        }

        [Fact]
        public void Storing_2()
        {
            var a1 = Age.LegalVotingAge;
            var a2 = Age.LegalVotingAge;

            var d = new Dictionary<Age, string> { { a1, "hello1" } };

            d[a2] = "hello2";

            d.Count.Should().Be(1);

            d[a1].Should().Be("hello2");
        }
    }
}

namespace ConsumerTests.IComparableTests
{
#if NET6_0_OR_GREATER
    [Intellenum(typeof(DateOnly))]
    public partial class DOS1
    {
        static DOS1()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
            Instance("JanThird", new DateOnly(2021, 1, 3));
        }
    }
#endif

    [Intellenum(typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    [Instance("Item3", 3)]
    public partial class S1
    {
    }

    [Intellenum]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    [Instance("Item3", 3)]
    public partial class S2
    {
    }

    [Intellenum(typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    [Instance("Item3", 3)]
    public partial class C1
    {
    }

    [Intellenum]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    [Instance("Item3", 3)]
    public partial class C2
    {
    }

    public class IComparableTests
    {
        [Fact]
        public void Underlying_type_of_int_means_the_vo_is_IComparable()
        {
            var l = new List<C1>(new[] { C1.Item1, C1.Item3, C1.Item2 });
            l[0].Value.Should().Be(1);
            l[1].Value.Should().Be(3);
            l[2].Value.Should().Be(2);

            l.Sort();

            l[0].Value.Should().Be(1);
            l[1].Value.Should().Be(2);
            l[2].Value.Should().Be(3);
        }

        [Fact]
        public void Underlying_type_of_default_int_means_the_vo_is_IComparable()
        {
            var l = new List<C2>(new[] { C2.Item1, C2.Item3, C2.Item2 });
            l[0].Value.Should().Be(1);
            l[1].Value.Should().Be(3);
            l[2].Value.Should().Be(2);

            l.Sort();

            l[0].Value.Should().Be(1);
            l[1].Value.Should().Be(2);
            l[2].Value.Should().Be(3);
        }


        public class ExplicitCompareToCalls
        {
            [Fact]
            public void Underlying_type_of_int_means_the_vo_is_IComparable()
            {
                var c1 = C1.Item1;
                var cc1 = C1.Item1;
                c1.CompareTo(cc1).Should().Be(0);
            }

            [Fact]
            public void Underlying_type_of_int_means_the_vo_is_IComparable_with_object_version_of_same_type()
            {
                var c1 = C1.Item1;
                var cc1 = C1.Item1;
                c1.CompareTo((object) cc1).Should().Be(0);
            }

            [Fact]
            public void Underlying_type_of_int_means_the_vo_is_not_IComparable_with_object_version_of_different_type()
            {
                var c1 = C1.Item1;
                Action a = () => c1.CompareTo((object) 123).Should().Be(0);

                a.Should().ThrowExactly<ArgumentException>().WithMessage("Cannot compare to object as it is not of type C1*");
            }

            [Fact]
            public void
                Underlying_type_of_int_means_the_vo_is_IComparable_with_object_version_of_null_and_behaves_the_same_way_of_returning_1()
            {
                var c1 = C1.Item1;
                c1.CompareTo((object?) null).Should().Be(1);
            }

            [Fact]
            public void Underlying_type_of_default_int_means_the_vo_is_IComparable()
            {
                var c1 = C2.Item1;
                var cc1 = C2.Item1;
                c1.CompareTo(cc1).Should().Be(0);
            }
        }

    }
}


using @double;
using @bool.@byte.@short.@float.@object;
using Intellenum.Tests.Types;

namespace NotSystem
{
    public static class Activator
    {
        public static T? CreateInstance<T>() => default(T);
    }
}

namespace ConsumerTests
{
    public class CreationTests
    {
        [Fact]
        public void Creation_using_the_Members_attribute()
        {
            UsingMembersAttribute.Member1.Value.Should().Be(0);
            UsingMembersAttribute.Member2.Value.Should().Be(1);
            UsingMembersAttribute.Member3.Value.Should().Be(2);
        }

        [Fact]
        public void Creation_Happy_Path_MyIntGeneric()
        {
            MyIntGeneric vo1 = MyIntGeneric.Item1;
            MyIntGeneric vo2 = MyIntGeneric.Item1;

            vo1.Should().Be(vo2);
            (vo1 == vo2).Should().BeTrue();
        }

        // There is an analyzer that stops creation of VOs via Activator.CreateInstance.
        // This test is here to ensure that it *only* catches System.Activator.
        // If compilation fails here, then that logic is broken.
        [Fact]
        public void Allows_using_Activate_CreateInstance_from_another_namespace()
        {
            _ = NotSystem.Activator.CreateInstance<string>();
        }

        [Fact]
        public void Creation_Happy_Path_MyInt()
        {
            MyInt vo1 = MyInt.Item1;
            MyInt vo2 = MyInt.Item1;

            vo1.Should().Be(vo2);
            (vo1 == vo2).Should().BeTrue();
        }

        [Fact]
        public void Creation_Happy_Path_MyString()
        {
            MyString vo1 = MyString.Item1;
            MyString vo2 = MyString.Item1;

            vo1.Should().Be(vo2);
            (vo1 == vo2).Should().BeTrue();
        }

        [Fact]
        public void Creation_can_create_a_VO_with_a_verbatim_identifier()
        {
            @class c1 = @class.Item1;
            @class c2 = @class.Item1;

            c1.Should().Be(c2);
            (c1 == c2).Should().BeTrue();

            @event e1 = @event.Item1;
            @event e2 = @event.Item1;

            e1.Should().Be(e2);
            (e1 == e2).Should().BeTrue();
        }

        [Fact]
        public void Creation_can_create_a_VO_with_from_a_namespace_with_an_escaped_keyword()
        {
            @classFromEscapedNamespace c1 = @classFromEscapedNamespace.Item1;
            @classFromEscapedNamespace c2 = @classFromEscapedNamespace.Item1;

            c1.Should().Be(c2);
            (c1 == c2).Should().BeTrue();
        }

        [Fact]
        public void Underlying_types_can_have_escaped_keywords()
        {
            @classFromEscapedNamespace c1 = @classFromEscapedNamespace.Item1;
            @classFromEscapedNamespace c2 = @classFromEscapedNamespace.Item1;

            c1.Should().Be(c2);
            (c1 == c2).Should().BeTrue();
        }
    }
}
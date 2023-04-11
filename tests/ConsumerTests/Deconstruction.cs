using @double;
using @bool.@byte.@short.@float.@object;
using Intellenum.Tests.Types;

namespace ConsumerTests;

public class DeconstructionTests
{
    [Fact]
    public void Creation_Happy_Path_MyInt()
    {
        var (name, value) = (MyInt) MyInt.Item1;

        name.Should().Be("Item1");
        value.Should().Be(1);
    }
}
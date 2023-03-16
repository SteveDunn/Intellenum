using FluentAssertions;

namespace Scratch;

public class ExplicitInstances
{
    [Fact]
    public void MixedInstances()
    {
        CondimentMixedInstances c1 = CondimentMixedInstances.Salt;
        CondimentMixedInstances c2 = CondimentMixedInstances.Pepper;

        (c1 == c2).Should().BeFalse();

        (c1 < c2).Should().BeTrue();

        var f1 = CondimentMixedInstances.FromName("Salt");
        f1.Should().Be(c1);

        CondimentMixedInstances.Mayo.Should().BeLessThan(CondimentMixedInstances.Ketchup);
    }

    [Fact]
    public void General()
    {
        Condiment c1 = Condiment.Salt;
        Condiment c2 = Condiment.Pepper;

        (c1 == c2).Should().BeFalse();

        (c1 < c2).Should().BeTrue();

        var f1 = Condiment.FromName("Salt");
        f1.Should().Be(c1);

    }

    [Fact]
    public void Implicit_operator()
    {
        Condiment c1 = Condiment.Salt;

        go(c1);

        static void go(int value) => value.Should().Be(Condiment.Salt.Value);
    }

    [Fact]
    public void Deconstruct()
    {
        var (name, value) = Condiment.Salt;
        name.Should().Be("Salt");
        value.Should().Be(Condiment.Salt.Value);
    }
}
using System.Globalization;

namespace ConsumerTests.TryParseTests;

public class Tests
{
    [Fact]
    public void Integers()
    {
        IntVoNoValidation.TryParse("1", out var ie).Should().BeTrue();
        ie.Value.Should().Be(1);
    }

    [Fact]
    public void Decimals()
    {
        {
            DecimalEnum.TryParse("1.23", out var ie).Should().BeTrue();
            ie.Value.Should().Be(1.23m);
        }

        {
            DecimalEnum.TryParse("1,23", NumberStyles.Number, new CultureInfo("de"), out var ie).Should().BeTrue();
            ie.Value.Should().Be(1.23m);
        }
    }

    [Fact]
    public void Bytes()
    {
        ByteVo.TryParse("1", out var ie).Should().BeTrue();
        ie.Value.Should().Be(1);
    }

    [Fact]
    public void Chars()
    {
        CharVo.TryParse("a", out var ie).Should().BeTrue();
        ie.Value.Should().Be('a');
    }

    [Fact]
    public void Double()
    {
        DoubleEnum.TryParse("1.23", out var ie).Should().BeTrue();
        ie.Value.Should().Be(1.23);
    }

    [Fact]
    public void Custom()
    {
        {
            bool r = PlanetEnum.TryParse("Brown-273400", out var p);
            r.Should().BeTrue();
            p.Should().Be(PlanetEnum.Jupiter);
        }

        {
            bool r = PlanetEnum.TryParse("Blue-24901", out _);
            r.Should().BeFalse();
        }
    }

    [Fact]
    public void When_parsing_fails()
    {
        IntEnum.TryParse("fifty", out var ie).Should().BeFalse();
        ie.Should().BeNull();
    }
}
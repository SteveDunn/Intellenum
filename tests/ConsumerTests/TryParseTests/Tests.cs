using System;
using System.Globalization;
using FluentAssertions;
using Intellenum;

namespace ConsumerTests.TryParseTests;

public class Tests
{
    [Fact]
    public void Integers()
    {
        IntVoNoValidation.TryParse("1", out var ivo).Should().BeTrue();
        ivo.Value.Should().Be(1);
    }

    [Fact]
    public void Decimals()
    {
        {
            DecimalVo.TryParse("1.23", out var ivo).Should().BeTrue();
            ivo.Value.Should().Be(1.23m);
        }

        {
            DecimalVo.TryParse("1,23", NumberStyles.Number, new CultureInfo("de"), out var ivo).Should().BeTrue();
            ivo.Value.Should().Be(1.23m);
        }
    }

    [Fact]
    public void Bytes()
    {
        ByteVo.TryParse("1", out var ivo).Should().BeTrue();
        ivo.Value.Should().Be(1);
    }

    [Fact]
    public void Chars()
    {
        CharVo.TryParse("a", out var ivo).Should().BeTrue();
        ivo.Value.Should().Be('a');
    }

    [Fact]
    public void Double()
    {
        DoubleVo.TryParse("1.23", out var ivo).Should().BeTrue();
        ivo.Value.Should().Be(1.23);
    }

    [Fact]
    public void When_parsing_fails()
    {
        IntVo.TryParse("fifty", out var ivo).Should().BeFalse();
        ivo.Should().BeNull();
    }
}
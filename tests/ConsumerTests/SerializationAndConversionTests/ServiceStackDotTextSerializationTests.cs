using ConsumerTests.SerializationAndConversionTests.Types;
using ServiceStack.Text;

namespace ConsumerTests.SerializationAndConversionTests;

public class ServiceStackDotTextSerializationTests
{
    [Fact]
    public void RoundTrip_Bool_WithSsdtProvider()
    {
        string json = JsonSerializer.SerializeToString(SsdtBoolVo.Yes);

        SsdtBoolVo deserialised = JsonSerializer.DeserializeFromString<SsdtBoolVo>(json);

        SsdtBoolVo.Yes.Value.Should().Be(deserialised.Value);
    }

    [Fact]
    public void RoundTrip_Byte_WithSsdtProvider()
    {
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtByteVo>(JsonSerializer.SerializeToString(SsdtByteVo.Item1));

        Assert.Equal(SsdtByteVo.Item1, deserializedVo);
    }

    [Fact]
    public void RoundTrip_Char_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtCharVo.A);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtCharVo>(json);

        Assert.Equal(SsdtCharVo.A, deserializedVo);
    }

    [Fact]
    public void RoundTrip_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtStringVo.Item1);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtStringVo>(json);

        Assert.Equal(SsdtStringVo.Item1, deserializedVo);
    }

    [Fact]
    public void RoundTrip_DateTimeOffset_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtDateTimeOffsetVo.JanSecond);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtDateTimeOffsetVo>(json);

        Assert.Equal(SsdtDateTimeOffsetVo.JanSecond, deserializedVo);
    }

    [Fact]
    public void RoundTrip_DateTime_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtDateTimeVo.Item1);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtDateTimeVo>(json);

        Assert.Equal(SsdtDateTimeVo.Item1, deserializedVo);
    }

    [Fact]
    public void RoundTrip_Decimal_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtDecimalVo.Item1);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtDecimalVo>(json);

        Assert.Equal(SsdtDecimalVo.Item1, deserializedVo);
    }

    [Fact]
    public void RoundTrip_Double_WithSsdtProvider()
    {
        var json = JsonSerializer.SerializeToString(SsdtDoubleVo.Item1);

        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtDoubleVo>(json);

        Assert.Equal(SsdtDoubleVo.Item1, deserializedVo);
    }

    [Fact]
    public void RoundTrip_Float_WithSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtFloatVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtFloatVo>(serializedVo)!;

        deserializedVo.Value.Should().Be(1.1f);
    }

    [Fact]
    public void RoundTrip_Guid_WithSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtGuidVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtGuidVo>(serializedVo)!;

        deserializedVo.Value.Should().Be(SsdtGuidVo.Item1.Value);
    }

    [Fact]
    public void RoundTrip_Int_WithSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtLongVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtIntVo>(serializedVo)!;

        deserializedVo.Value.Should().Be(1);
    }

    [Fact]
    public void RoundTrip_ShortSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtShortVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtShortVo>(serializedVo)!;

        deserializedVo.Value.Should().Be(1);
    }

    [Fact]
    public void RoundTrip_String_WithSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtStringVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtStringVo>(serializedVo)!;

        deserializedVo.Value.Should().Be("1");
    }

    [Fact]
    public void RoundTrip_TimeOnly_WithSsdtProvider()
    {
        string serializedVo = JsonSerializer.SerializeToString(SsdtTimeOnlyVo.Item1);
        var deserializedVo = JsonSerializer.DeserializeFromString<SsdtTimeOnlyVo>(serializedVo)!;

        deserializedVo.Value.Should().Be(SsdtTimeOnlyVo.Item1.Value);
    }
}
// ReSharper disable NullableWarningSuppressionIsUsed

#nullable disable
using FluentAssertions;
using Intellenum;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MediumTests.SerializationAndConversionTests;

[Intellenum(underlyingType: typeof(double), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 720742592373919744d)]
[Instance("Item2", 2.2d)]
public partial class DoubleHolderId_string
{
}

[Intellenum(underlyingType: typeof(decimal), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
public partial class DecimalHolderId_string
{
    static DecimalHolderId_string()
    {
        Instance("Item1", 720742592373919744m);
        Instance("Item2", 2.2m);
    }
}

[Intellenum(underlyingType: typeof(float), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 720742592373919744)]
[Instance("Item2", 2.2f)]
public partial class FloatHolderId_string
{
}

[Intellenum(underlyingType: typeof(long), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 720742592373919744)]
[Instance("Item2", 2)]
public partial class LongHolderId_string
{
}

[Intellenum(underlyingType: typeof(short), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 123)]
[Instance("Item2", 321)]
public partial class ShortHolderId_string
{
}

[Intellenum(underlyingType: typeof(int), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 321)]
[Instance("Item2", 2)]
public partial class IntHolderId_string
{
}

[Intellenum(underlyingType: typeof(byte), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class ByteHolderId_string
{
}

[Intellenum(underlyingType: typeof(double))]
[Instance("Item1", 720742592373919744)]
[Instance("Item2", 2.2d)]
public partial class DoubleHolderId_normal
{
}

[Intellenum(underlyingType: typeof(decimal))]
public partial class DecimalHolderId_normal
{
    static DecimalHolderId_normal()
    {
        Instance("Item1", 720742592373919744m);
        Instance("Item2", 2.2m);
    }
}

[Intellenum(underlyingType: typeof(float))]
[Instance("Item1", 720742592373919744)]
[Instance("Item2", 2.2f)]
public partial class FloatHolderId_normal
{
}

[Intellenum(underlyingType: typeof(long))]
[Instance("Item1", 720742592373919744)]
[Instance("Item2", 2)]
public partial class LongHolderId_normal
{
}

[Intellenum(underlyingType: typeof(short))]
[Instance("Item1", 123)]
[Instance("Item2", 321)]
public partial class ShortHolderId_normal
{
}

[Intellenum(underlyingType: typeof(int))]
[Instance("Item1", 321)]
[Instance("Item2", 2)]
public partial class IntHolderId_normal
{
}

[Intellenum(underlyingType: typeof(byte))]
[Instance("Item1", 123)]
[Instance("Item2", 2)]
public partial class ByteHolderId_normal
{
}
    
public class Container
{
    public DoubleHolderId_string DoubleHolder_as_a_string { get; set; } = null!;
    public DecimalHolderId_string DecimalHolder_as_a_string { get; set; } = null!;
    public LongHolderId_string LongHolder_as_a_string { get; set; } = null!;
    public FloatHolderId_string FloatHolder_as_a_string { get; set; } = null!;
    public ByteHolderId_string ByteHolder_as_a_string { get; set; } = null!;
    public IntHolderId_string IntHolder_as_a_string { get; set; } = null!;
    public ShortHolderId_string ShortHolder_as_a_string { get; set; } = null!;

    public DoubleHolderId_normal DoubleHolder_normal { get; set; } = null!;
    public DecimalHolderId_normal DecimalHolder_normal { get; set; } = null!;
    public LongHolderId_normal LongHolder_normal { get; set; } = null!;
    public FloatHolderId_normal FloatHolder_normal { get; set; } = null!;
    public ByteHolderId_normal ByteHolder_normal { get; set; } = null!;
    public IntHolderId_normal IntHolder_normal { get; set; } = null!;
    public ShortHolderId_normal ShortHolder_normal { get; set; } = null!;
}

public class CustomizationTests
{
    [Fact]
    public void CanSerializeAndDeserializeAsString()
    {
        var holderId = DoubleHolderId_string.Item1;

        string serialized = JsonSerializer.Serialize(holderId);
        var deserialized = JsonSerializer.Deserialize<DoubleHolderId_string>(serialized);

        deserialized.Value.Should().Be(DoubleHolderId_string.Item1);
    }

    [Fact]
    public void CanSerializeAndDeserializeWhenVoIsInAComplexObject()
    {
        var container  = new Container
        {
            ByteHolder_as_a_string = ByteHolderId_string.Item1,
            DecimalHolder_as_a_string = DecimalHolderId_string.Item1,
            DoubleHolder_as_a_string = DoubleHolderId_string.Item1,
            FloatHolder_as_a_string = FloatHolderId_string.Item1,
            IntHolder_as_a_string = IntHolderId_string.Item1,
            LongHolder_as_a_string = LongHolderId_string.Item1,
            ShortHolder_as_a_string = ShortHolderId_string.Item1,

            ByteHolder_normal = ByteHolderId_normal.Item1,
            DecimalHolder_normal = DecimalHolderId_normal.Item1,
            DoubleHolder_normal = DoubleHolderId_normal.Item1,
            FloatHolder_normal = FloatHolderId_normal.Item1,
            IntHolder_normal = IntHolderId_normal.Item1,
            LongHolder_normal = LongHolderId_normal.Item1,
            ShortHolder_normal = ShortHolderId_normal.Item1,
        };
        
        string serialized = JsonSerializer.Serialize(container);

        var deserialized = JsonSerializer.Deserialize<Container>(serialized);

        deserialized.ByteHolder_as_a_string.Value.Should().Be(1);
        deserialized.ByteHolder_normal.Value.Should().Be(123);
    }
}

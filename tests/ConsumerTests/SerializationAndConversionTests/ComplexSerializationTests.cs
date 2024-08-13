#nullable disable
using System.Text.Json;
using ConsumerTests.TestEnums;

namespace MediumTests.SerializationAndConversionTests;

public class ComplexSerializationTests
{
    public class Complex
    {
        public ConsumerTests.TestEnums.SystemTextJsonBoolEnum SystemTextJsonBoolEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonBoolEnum.Yes;
        public ConsumerTests.TestEnums.SystemTextJsonByteEnum SystemTextJsonByteEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonByteEnum.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonCharEnum SystemTextJsonCharEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonCharEnum.B;
        public ConsumerTests.TestEnums.SystemTextJsonDateTimeOffsetEnum SystemTextJsonDateTimeOffsetEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonDateTimeOffsetEnum.JanFirst;

        public ConsumerTests.TestEnums.SystemTextJsonDateTimeVo
            TestTypes_SystemTextJsonDateTimeEnum { get; set; } =
            ConsumerTests.TestEnums.SystemTextJsonDateTimeVo.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonDecimalVo TestTypes_SystemTextJsonDecimalEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonDecimalVo.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonDoubleEnum TestTypes_SystemTextJsonDoubleEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonDoubleEnum.Item1;

        public ConsumerTests.TestEnums.SystemTextJsonFloatEnum
            TestTypesClassEnumsSystemTextJsonFloatEnum { get; set; } =
            ConsumerTests.TestEnums.SystemTextJsonFloatEnum.Item1;

        public ConsumerTests.TestEnums.SystemTextJsonFooEnum
            TestTypes_SystemTextJsonFooVo { get; set; } =
            ConsumerTests.TestEnums.SystemTextJsonFooEnum.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonGuidEnum SystemTextJsonGuidEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonGuidEnum.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonIntEnum SystemTextJsonIntEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonIntEnum.Item1;
        public ConsumerTests.TestEnums.SystemTextJsonLongEnum SystemTextJsonLongEnum { get; set; } = ConsumerTests.TestEnums.SystemTextJsonLongEnum.Item1;

        public ConsumerTests.TestEnums.SystemTextJsonStringEnum
            SystemTextJsonStringEnum { get; set; } =
            ConsumerTests.TestEnums.SystemTextJsonStringEnum.Item1;
    }

    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var complex = new Complex();

        string serialized = JsonSerializer.Serialize(complex);
        Complex deserialized = JsonSerializer.Deserialize<Complex>(serialized);

        deserialized.SystemTextJsonBoolEnum.Value.Should().Be(true);
        deserialized.SystemTextJsonBoolEnum.Value.Should().Be(true);
        deserialized.SystemTextJsonByteEnum.Value.Should().Be(1);
        deserialized.SystemTextJsonCharEnum.Value.Should().Be('b');
        deserialized.SystemTextJsonDateTimeOffsetEnum.Value.Should().Be(new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
        deserialized.TestTypes_SystemTextJsonDateTimeEnum.Value.Should().Be(TestDates._date1);
        deserialized.TestTypes_SystemTextJsonDecimalEnum.Value.Should().Be(1.1m);
        deserialized.TestTypes_SystemTextJsonDoubleEnum.Value.Should().Be(1.1d);
        deserialized.TestTypesClassEnumsSystemTextJsonFloatEnum.Value.Should().Be(1.1f);
        deserialized.TestTypes_SystemTextJsonFooVo.Value.Age.Should().Be(1);
        deserialized.TestTypes_SystemTextJsonFooVo.Value.Name.Should().Be("One");
        deserialized.SystemTextJsonGuidEnum.Value.Should().Be(new Guid("00000000-0000-0000-0000-000000000001"));
        deserialized.SystemTextJsonIntEnum.Value.Should().Be(1);
        deserialized.SystemTextJsonLongEnum.Value.Should().Be(1L);
        deserialized.SystemTextJsonStringEnum.Value.Should().Be("Item1!");
    }
}
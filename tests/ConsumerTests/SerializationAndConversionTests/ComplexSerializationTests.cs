#nullable disable
using System.Text.Json;

namespace MediumTests.SerializationAndConversionTests;

public class ComplexSerializationTests
{
    public class Complex
    {
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonBoolVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonBoolVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonBoolVo.Yes;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonByteVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonByteVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonByteVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonCharEnum VogenIntegrationTestsTestTypesClassEnumsSystemTextJsonCharEnum { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonCharEnum.B;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonDateTimeOffsetVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeOffsetVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonDateTimeOffsetVo.JanFirst;

        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonDateTimeVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeVo { get; set; } =
            Intellenum.IntegrationTests.TestEnums.SystemTextJsonDateTimeVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonDecimalVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDecimalVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonDecimalVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonDoubleVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDoubleVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonDoubleVo.Item1;

        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonFloatEnum
            VogenIntegrationTestsTestTypesClassEnumsSystemTextJsonFloatEnum { get; set; } =
            Intellenum.IntegrationTests.TestEnums.SystemTextJsonFloatEnum.Item1;

        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonFooVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo { get; set; } =
            Intellenum.IntegrationTests.TestEnums.SystemTextJsonFooVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonGuidVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonGuidVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonGuidVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonIntVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonIntVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonIntVo.Item1;
        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonLongVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonLongVo { get; set; } = Intellenum.IntegrationTests.TestEnums.SystemTextJsonLongVo.Item1;

        public Intellenum.IntegrationTests.TestEnums.SystemTextJsonStringVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonStringVo { get; set; } =
            Intellenum.IntegrationTests.TestEnums.SystemTextJsonStringVo.Item1;
    }

    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var complex = new Complex();

        string serialized = JsonSerializer.Serialize(complex);
        Complex deserialized = JsonSerializer.Deserialize<Complex>(serialized);

        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonBoolVo.Value.Should().Be(true);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonBoolVo.Value.Should().Be(true);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonByteVo.Value.Should().Be(1);
        deserialized.VogenIntegrationTestsTestTypesClassEnumsSystemTextJsonCharEnum.Value.Should().Be('b');
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeOffsetVo.Value.Should().Be(new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeVo.Value.Should().Be(new DateTime(2019, 12, 13, 14, 15, 16));
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDecimalVo.Value.Should().Be(1.1m);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDoubleVo.Value.Should().Be(1.1d);
        deserialized.VogenIntegrationTestsTestTypesClassEnumsSystemTextJsonFloatEnum.Value.Should().Be(1.1f);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo.Value.Age.Should().Be(1);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo.Value.Name.Should().Be("One");
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonGuidVo.Value.Should().Be(new Guid("00000000-0000-0000-0000-000000000001"));
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonIntVo.Value.Should().Be(1);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonLongVo.Value.Should().Be(1L);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonStringVo.Value.Should().Be("Item1!");
    }
}
#nullable disable
using System;
using System.Text.Json;
using FluentAssertions;

namespace MediumTests.SerializationAndConversionTests;

public class ComplexSerializationTests
{
    public class Complex
    {
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonBoolVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonBoolVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonBoolVo.Yes;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonByteVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonByteVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonByteVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonCharVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonCharVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonCharVo.B;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDateTimeOffsetVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeOffsetVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDateTimeOffsetVo.JanFirst;

        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDateTimeVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeVo { get; set; } =
            Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDateTimeVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDecimalVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDecimalVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDecimalVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDoubleVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDoubleVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonDoubleVo.Item1;

        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonFloatVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFloatVo { get; set; } =
            Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonFloatVo.Item1;

        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonFooVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo { get; set; } =
            Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonFooVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonGuidVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonGuidVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonGuidVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonIntVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonIntVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonIntVo.Item1;
        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonLongVo Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonLongVo { get; set; } = Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonLongVo.Item1;

        public Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonStringVo
            Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonStringVo { get; set; } =
            Intellenum.IntegrationTests.TestTypes.ClassVos.SystemTextJsonStringVo.Item1;
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
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonCharVo.Value.Should().Be('2');
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeOffsetVo.Value.Should().Be(new DateTimeOffset(2020, 12, 13, 23, 59, 59, 999, TimeSpan.Zero));
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDateTimeVo.Value.Should().Be(new DateTime(2020, 12, 13, 23, 59, 59, 999));
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDecimalVo.Value.Should().Be(3.33m);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonDoubleVo.Value.Should().Be(4.44d);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFloatVo.Value.Should().Be(5.55f);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo.Value.Age.Should().Be(42);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonFooVo.Value.Name.Should().Be("Fred");
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonGuidVo.Value.Should().Be(Guid.Empty);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonIntVo.Value.Should().Be(6);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonLongVo.Value.Should().Be(7L);
        deserialized.Vogen_IntegrationTests_TestTypes_ClassVos_SystemTextJsonStringVo.Value.Should().Be("8");
    }
}
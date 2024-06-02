#nullable disable

using System.Text.Json;
using Intellenum.Tests.Types;

namespace ConsumerTests.EnumAsDictionaryKeyTests;

[Intellenum(typeof(DateTimeOffset))]
[Member("Manager", 1)]
[Member("Operator", 2)]
public partial class EmployeeTypeDateTimeOffset
{
}

public class DateTimeOffsetTests
{
    [Fact]
    public void can_serialize_intellenum_as_key_of_dictionary()
    {
        Dictionary<EmployeeTypeDateTimeOffset, List<Employee>> d = new()
        {
            { EmployeeTypeDateTimeOffset.Manager, new List<Employee> { new Employee("John Smith", 30) } },
            { EmployeeTypeDateTimeOffset.Operator, new List<Employee> { new Employee("Dave Angel", 42) } }
        };

        var json = JsonSerializer.Serialize(d);

        var d2 = JsonSerializer.Deserialize<Dictionary<EmployeeTypeDateTimeOffset, List<Employee>>>(json);

        d2.Should().ContainKey(EmployeeTypeDateTimeOffset.Manager);
        d2.Should().ContainKey(EmployeeTypeDateTimeOffset.Operator);

        d2[EmployeeTypeDateTimeOffset.Manager].Should().Contain(new Employee("John Smith", 30));
        d2[EmployeeTypeDateTimeOffset.Operator].Should().Contain(new Employee("Dave Angel", 42));
    }
}


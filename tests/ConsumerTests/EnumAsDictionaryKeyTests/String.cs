#if NET6_0_OR_GREATER
#nullable disable

using System.Text.Json;
using Intellenum.Tests.Types;

namespace ConsumerTests.EnumAsDictionaryKeyTests;

[Intellenum(typeof(string))]
[Member("Manager", 1)]
[Member("Operator", 2)]
public partial class EmployeeTypeString
{
}

public class StringTests
{
    [Fact]
    public void int_can_serialize_intellenum_as_key_of_dictionary()
    {
        Dictionary<EmployeeTypeString, List<Employee>> d = new()
        {
            { EmployeeTypeString.Manager, new List<Employee> { new Employee("John Smith", 30) } },
            { EmployeeTypeString.Operator, new List<Employee> { new Employee("Dave Angel", 42) } }
        };

        var json = JsonSerializer.Serialize(d);

        var d2 = JsonSerializer.Deserialize<Dictionary<EmployeeTypeString, List<Employee>>>(json);

        d2.Should().ContainKey(EmployeeTypeString.Manager);
        d2.Should().ContainKey(EmployeeTypeString.Operator);

        d2[EmployeeTypeString.Manager].Should().Contain(new Employee("John Smith", 30));
        d2[EmployeeTypeString.Operator].Should().Contain(new Employee("Dave Angel", 42));
    }
}

#endif
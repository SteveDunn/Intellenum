﻿#nullable disable

using System.Text.Json;
using Intellenum.Tests.Types;

namespace ConsumerTests.EnumAsDictionaryKeyTests;

[Intellenum(typeof(long))]
[Member("Manager", 1)]
[Member("Operator", 2)]
public partial class EmployeeTypeLong
{
}

public class LongTests
{
    [Fact]
    public void int_can_serialize_intellenum_as_key_of_dictionary()
    {
        Dictionary<EmployeeTypeLong, List<Employee>> d = new()
        {
            { EmployeeTypeLong.Manager, new List<Employee> { new Employee("John Smith", 30) } },
            { EmployeeTypeLong.Operator, new List<Employee> { new Employee("Dave Angel", 42) } }
        };

        var json = JsonSerializer.Serialize(d);

        var d2 = JsonSerializer.Deserialize<Dictionary<EmployeeTypeLong, List<Employee>>>(json);

        d2.Should().ContainKey(EmployeeTypeLong.Manager);
        d2.Should().ContainKey(EmployeeTypeLong.Operator);

        d2[EmployeeTypeLong.Manager].Should().Contain(new Employee("John Smith", 30));
        d2[EmployeeTypeLong.Operator].Should().Contain(new Employee("Dave Angel", 42));
    }
}


﻿#nullable disable

using System.Text.Json;
using Intellenum.Tests.Types;

namespace ConsumerTests.EnumAsDictionaryKeyTests;

[Intellenum(typeof(char))]
[Member("Manager", 1)]
[Member("Operator", 2)]
public partial class EmployeeTypeChar
{
}

public class Char
{
    [Fact]
    public void int_can_serialize_intellenum_as_key_of_dictionary()
    {
        Dictionary<EmployeeTypeChar, List<Employee>> d = new()
        {
            { EmployeeTypeChar.Manager, new List<Employee> { new Employee("John Smith", 30) } },
            { EmployeeTypeChar.Operator, new List<Employee> { new Employee("Dave Angel", 42) } }
        };

        var json = JsonSerializer.Serialize(d);

        var d2 = JsonSerializer.Deserialize<Dictionary<EmployeeTypeChar, List<Employee>>>(json);

        d2.Should().ContainKey(EmployeeTypeChar.Manager);
        d2.Should().ContainKey(EmployeeTypeChar.Operator);

        d2[EmployeeTypeChar.Manager].Should().Contain(new Employee("John Smith", 30));
        d2[EmployeeTypeChar.Operator].Should().Contain(new Employee("Dave Angel", 42));
    }
}

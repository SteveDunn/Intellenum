﻿#nullable disable

using System.Text.Json;
using Intellenum.Tests.Types;

namespace ConsumerTests.EnumAsDictionaryKeyTests;

[Intellenum(typeof(DateOnly))]
public partial class DateOnlyEnum
{
    public static readonly DateOnlyEnum Manager = new(new DateOnly(2020, 12, 13));
    public static readonly DateOnlyEnum Operator = new(new DateOnly(2023, 12, 13));
}

public class DateOnlyTests
{
    [Fact]
    public void int_can_serialize_intellenum_as_key_of_dictionary()
    {
        Dictionary<DateOnlyEnum, List<Employee>> d = new()
        {
            { DateOnlyEnum.Manager, new List<Employee> { new Employee("John Smith", 30) } },
            { DateOnlyEnum.Operator, new List<Employee> { new Employee("Dave Angel", 42) } }
        };

        var json = JsonSerializer.Serialize(d);

        var d2 = JsonSerializer.Deserialize<Dictionary<DateOnlyEnum, List<Employee>>>(json);

        d2.Should().ContainKey(DateOnlyEnum.Manager);
        d2.Should().ContainKey(DateOnlyEnum.Operator);

        d2[DateOnlyEnum.Manager].Should().Contain(new Employee("John Smith", 30));
        d2[DateOnlyEnum.Operator].Should().Contain(new Employee("Dave Angel", 42));
    }
}

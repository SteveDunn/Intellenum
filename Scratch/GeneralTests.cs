#nullable disable

using System.ComponentModel;
using FluentAssertions;
using Intellenum;
using Intellenum.IntegrationTests.TestTypes.ClassVos;

namespace Scratch;

// [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(Bar))]
// public partial class NoJsonFooVo
// {
//     static NoJsonFooVo()
//     {
//         Instance("Item1", new NoJsonFooVo(new Bar(42, "Fred")));
//         Instance("Item2", new NoJsonFooVo(new Bar(2, "Two")));
//     }
// }

public record struct Bar(int Age, string Name) : IComparable<Bar>
{
    public int CompareTo(Bar other) => Age.CompareTo(other.Age);
}



public class GeneralTests
{
    enum ECustomerType
    {
        Standard, 
        Gold
    }
    

    [Fact]
    public void ToStringTest()
    {
        ECustomerType.Gold.ToString().Should().Be("Gold");    
        CustomerType t1 = CustomerType.Standard;
        t1.ToString().Should().Be("Standard");
    }

    [Fact]
    public void ValueTests()
    {
        CustomerType t1 = CustomerType.Standard;
        CustomerType t2 = CustomerType.Gold;
        t1.Value.Should().Be(1);
        t2.Value.Should().Be(2);
    }

    [Fact]
    public void General()
    {
        CustomerType t1 = CustomerType.Standard;
        CustomerType t2 = CustomerType.Gold;

        CustomerType x = CustomerType.FromValue(1);

        (t1 == t2).Should().BeFalse();
        
        (t1 == 1).Should().BeTrue();
        (t1 != 2).Should().BeTrue();

        (t1 != t2).Should().BeTrue();

        (t1 == x).Should().BeTrue();

        CustomerType.FromValue(1).Should().Be(CustomerType.Standard);
        CustomerType.FromValue(2).Should().Be(CustomerType.Gold);

        CustomerType.FromName("Standard").Should().Be(CustomerType.Standard);
        CustomerType.FromName("Gold").Should().Be(CustomerType.Gold);

        CustomerType.IsDefined(1).Should().BeTrue();
        CustomerType.IsDefined(2).Should().BeTrue();
        CustomerType.IsDefined(3).Should().BeFalse();

        CustomerType ct1;
        CustomerType.TryFromName("Standard", out ct1).Should().BeTrue();

        CustomerType ct2;
        CustomerType.TryFromName("Gold", out ct2).Should().BeTrue();

        CustomerType ct3;
        CustomerType.TryFromName("FOO", out ct3).Should().BeFalse();

        CustomerType ctv1;
        CustomerType.TryFromValue(1, out ctv1).Should().BeTrue();
        ctv1.Should().Be(CustomerType.Standard);

        CustomerType ctv2;
        CustomerType.TryFromValue(2, out ctv2).Should().BeTrue();
        ctv2.Should().Be(CustomerType.Gold);

        CustomerType.TryFromValue(666, out _).Should().BeFalse();

        (CustomerType.Standard < CustomerType.Gold).Should().BeTrue();
        (CustomerType.Gold < CustomerType.Standard).Should().BeFalse();

        ((int) t1 == t1).Should().BeTrue();
        ((int) t1 == 1).Should().BeTrue();
    }
}
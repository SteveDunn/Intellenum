using Intellenum;
using FluentAssertions;

namespace Scratch;

[Intellenum]
[Instance("Standard", 1)]
[Instance("Gold", 2)]
public partial class CustomerType
{
    
}


public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        CustomerType t1 = CustomerType.Standard;
        CustomerType t2 = CustomerType.Gold;

        CustomerType x = CustomerType.FromValue(1);

        (t1 == t2).Should().BeFalse();

        (t1 == x).Should().BeTrue();

        CustomerType.FromValue(1).Should().Be(CustomerType.Standard);
        CustomerType.FromValue(2).Should().Be(CustomerType.Gold);

        CustomerType.FromName("Standard").Should().Be(CustomerType.Standard);
        CustomerType.FromName("Gold").Should().Be(CustomerType.Gold);

        CustomerType.ContainsValue(1).Should().BeTrue();
        CustomerType.ContainsValue(2).Should().BeTrue();
        CustomerType.ContainsValue(3).Should().BeFalse();

        CustomerType ct1;
        CustomerType.TryFromName("Standard", out ct1).Should().BeTrue();

        CustomerType ct2;
        CustomerType.TryFromName("Gold", out ct2).Should().BeTrue();

        CustomerType ct3;
        CustomerType.TryFromName("FOO", out ct3).Should().BeFalse();




        //CustomerType.f

    }
}
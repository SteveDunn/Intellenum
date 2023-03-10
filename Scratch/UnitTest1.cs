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
        

        //CustomerType.f

    }
}
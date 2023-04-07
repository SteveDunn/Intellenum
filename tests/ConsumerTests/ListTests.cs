using Intellenum.Tests.Types;
using Xunit.Abstractions;

namespace ConsumerTests;

public class ListTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ListTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void General()
    {
        var l = CustomerType.List();
        
        l.Count().Should().Be(2);
        
        foreach (var (name, value) in CustomerType.List())
        {
            _testOutputHelper.WriteLine($"{name} - {value}");
        }
    }
}
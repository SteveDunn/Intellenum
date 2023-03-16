using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class TryFromNameValueBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public bool StandardEnums()
    {
        bool b = Enum.TryParse<ECustomerType>("Standard", out _);
        b |= Enum.TryParse<ECustomerType>("Gold", out _);
        b |= Enum.TryParse<ECustomerType>("Diamond", out _);
        b |= Enum.TryParse<ECustomerType>("Platinum", out _);

        return b;
    }

    [Benchmark]
    public bool Intellenums()
    {
        bool ret = IECustomerType.TryFromName("Standard", out _);
        ret |= IECustomerType.TryFromName("Gold", out _);
        ret |= IECustomerType.TryFromName("Diamond", out _);
        ret |= IECustomerType.TryFromName("Platinum", out _);
        
        return ret;
    }
    
    [Benchmark]
    public bool EnumGenerators()
    {
        bool ret = false;

        ret |= EGCustomerTypeExtensions.TryParse("Standard", out _);
        ret |= EGCustomerTypeExtensions.TryParse("Gold", out _);
        ret |= EGCustomerTypeExtensions.TryParse("Diamond", out _);
        ret |= EGCustomerTypeExtensions.TryParse("Platinum", out _);
        
        return ret;
    }

    [Benchmark]
    public bool SmartEnums()
    {
        bool ret = false;

        ret |= SECustomerType.TryFromName( "Gold", out _);
        ret |= SECustomerType.TryFromName("Silver", out _);
        ret |= SECustomerType.TryFromName("Diamond", out _);
        ret |= SECustomerType.TryFromName("Platinum", out _);
        
        return ret;
    }
}
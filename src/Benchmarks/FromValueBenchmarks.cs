using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class FromValueBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public bool StandardEnums()
    {
        bool b = Enum.TryParse<ECustomerType>("1", out _);
        b |= Enum.TryParse<ECustomerType>("2", out _);
        b |= Enum.TryParse<ECustomerType>("3", out _);
        b |= Enum.TryParse<ECustomerType>("4", out _);

        return b;
    }

    [Benchmark]
    public bool Intellenums_FromValue_Try()
    {
        bool ret = IECustomerType.TryFromValue(1, out _);
        ret |= IECustomerType.TryFromValue(2, out _);
        ret |= IECustomerType.TryFromValue(3, out _);
        ret |= IECustomerType.TryFromValue(4, out _);
        
        return ret;
    }

    [Benchmark]
    public bool Intellenums_FromValue()
    {
        var ret = IECustomerType.FromValue(1);
        ret |= IECustomerType.FromValue(2);
        ret |= IECustomerType.FromValue(3);
        ret |= IECustomerType.FromValue(4);
        
        return ret != 0;
    }

    [Benchmark]
    public bool SmartEnums_Try()
    {
        var ret = false;

        ret |= SECustomerType.TryFromValue( 1, out _);
        ret |= SECustomerType.TryFromValue( 2, out _);
        ret |= SECustomerType.TryFromValue( 3, out _);
        ret |= SECustomerType.TryFromValue( 4, out _);
        
        return ret;
    }

    [Benchmark]
    public bool SmartEnums()
    {
        var ret = SECustomerType.FromValue( 1);
        ret |= SECustomerType.FromValue( 2);
        ret |= SECustomerType.FromValue( 3);
        ret |= SECustomerType.FromValue( 4);
        
        return ret != 0;
    }
}
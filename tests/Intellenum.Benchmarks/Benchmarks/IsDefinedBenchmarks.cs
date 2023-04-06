using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmark for seeing if a value is defined.
/// </summary>
[MemoryDiagnoser]
public class IsDefinedBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }
    
    [Benchmark]
    public bool StandardEnums()
    {
        bool ret = false;

        ret |= Enum.IsDefined(typeof(ECustomerType), 1);
        ret |= Enum.IsDefined(typeof(ECustomerType), 2);
        ret |= Enum.IsDefined(typeof(ECustomerType), 3);
        ret |= Enum.IsDefined(typeof(ECustomerType), 4);

        return ret;
    }

    [Benchmark]
    public bool Intellenums()
    {
        bool ret = false;
        
        ret |= IECustomerType.IsDefined(1);
        ret |= IECustomerType.IsDefined(2);
        ret |= IECustomerType.IsDefined(3);
        ret |= IECustomerType.IsDefined(4);
        
        return ret;
    }
    
    [Benchmark]
    public bool EnumGenerators()
    {
        bool ret = false;

        // var s = IECustomerType.Gold;
        // var n = nameof(IECustomerType.Gold);
        
        ret |= EGCustomerTypeExtensions.IsDefined((EGCustomerType) 1);
        ret |= EGCustomerTypeExtensions.IsDefined((EGCustomerType) 2);
        ret |= EGCustomerTypeExtensions.IsDefined((EGCustomerType) 3);
        ret |= EGCustomerTypeExtensions.IsDefined((EGCustomerType) 4);
        
        return ret;
    }

    [Benchmark]
    public bool SmartEnums()
    {
        bool ret = false;

        ret |= SECustomerType.TryFromValue(1, out _);
        ret |= SECustomerType.TryFromValue(2, out _);
        ret |= SECustomerType.TryFromValue(3, out _);
        ret |= SECustomerType.TryFromValue(4, out _);
        
        return ret;
    }
}
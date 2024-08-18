using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class FromValueBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public (ECustomerType e1, ECustomerType e2, ECustomerType e3, ECustomerType e4) StandardEnums_Try()
    {
        Enum.TryParse<ECustomerType>("0", out var e1);
        Enum.TryParse<ECustomerType>("1", out var e2);
        Enum.TryParse<ECustomerType>("2", out var e3);
        Enum.TryParse<ECustomerType>("3", out var e4);

        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (IECustomerType e1, IECustomerType e2, IECustomerType e3, IECustomerType e4) Intellenums_Try()
    {
        IECustomerType.TryFromValue(0, out var e1);
        IECustomerType.TryFromValue(1, out var e2);
        IECustomerType.TryFromValue(2, out var e3);
        IECustomerType.TryFromValue(3, out var e4);
        
        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (SECustomerType e1, SECustomerType e2, SECustomerType e3, SECustomerType e4) SmartEnums_Try()
    {
        SECustomerType.TryFromValue(0, out var e1);
        SECustomerType.TryFromValue(1, out var e2);
        SECustomerType.TryFromValue(2, out var e3);
        SECustomerType.TryFromValue(3, out var e4);
        
        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (IECustomerType e1, IECustomerType e2, IECustomerType e3, IECustomerType e4) Intellenums_FromValue()
    {
        var e1 = IECustomerType.FromValue(0);
        var e2 = IECustomerType.FromValue(1);
        var e3 = IECustomerType.FromValue(2);
        var e4 = IECustomerType.FromValue(3);
        
        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (SECustomerType e1, SECustomerType e2, SECustomerType e3, SECustomerType e4) SmartEnums()
    {
        var e1 = SECustomerType.FromValue(0);
        var e2 = SECustomerType.FromValue(1);
        var e3 = SECustomerType.FromValue(2);
        var e4 = SECustomerType.FromValue(3);
        
        return (e1, e2, e3, e4);
    }
}
using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmark for seeing if a value is defined.
/// </summary>
[JsonExporterAttribute.Full]
[JsonExporterAttribute.FullCompressed]
[MemoryDiagnoser]
public class IsDefinedBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }
    
    [Benchmark]
    public (bool e1, bool e2, bool e3, bool e4) StandardEnums()
    {
        var e1 = Enum.IsDefined(typeof(ECustomerType), 0);
        var e2 = Enum.IsDefined(typeof(ECustomerType), 1);
        var e3 = Enum.IsDefined(typeof(ECustomerType), 2);
        var e4 = Enum.IsDefined(typeof(ECustomerType), 3);

        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (bool e1, bool e2, bool e3, bool e4) Intellenums()
    {
        var e1 =  IECustomerType.IsDefined(0);
        var e2 =  IECustomerType.IsDefined(1);
        var e3 =  IECustomerType.IsDefined(2);
        var e4 =  IECustomerType.IsDefined(3);
        
        return (e1, e2, e3, e4);
    }

    [Benchmark]
    public (bool e1, bool e2, bool e3, bool e4) EnumGenerators()
    {
        var e1 = EGCustomerTypeExtensions.IsDefined((EGCustomerType) 0);
        var e2 = EGCustomerTypeExtensions.IsDefined((EGCustomerType) 1);
        var e3 = EGCustomerTypeExtensions.IsDefined((EGCustomerType) 2);
        var e4 = EGCustomerTypeExtensions.IsDefined((EGCustomerType) 3);
        
        return (e1, e2, e3, e4);
    }
}
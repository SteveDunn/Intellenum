using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[JsonExporterAttribute.Full]
[JsonExporterAttribute.FullCompressed]
[MemoryDiagnoser]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class FromNameBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
        // warm up types in case they do anything on creation
        _ = IECustomerType.Standard;
        _ = ECustomerType.Standard;
        _ = SECustomerType.Standard;
    }

    // [Benchmark]
    // public (ECustomerType e1, ECustomerType e2, ECustomerType e3, ECustomerType e4) StandardEnums_Try()
    // {
    //     Enum.TryParse<ECustomerType>("Standard", out var e1);
    //     Enum.TryParse<ECustomerType>("Gold", out var e2);
    //     Enum.TryParse<ECustomerType>("Diamond", out var e3);
    //     Enum.TryParse<ECustomerType>("Platinum", out var e4);
    //
    //     return (e1, e2, e3, e4);
    // }
    //
    // [Benchmark]
    // public (SECustomerType e1, SECustomerType e2, SECustomerType e3, SECustomerType e4) SmartEnums_Try()
    // {
    //     SECustomerType.TryFromName("Standard", out var e1);
    //     SECustomerType.TryFromName("Gold", out var e2);
    //     SECustomerType.TryFromName("Diamond", out var e3);
    //     SECustomerType.TryFromName("Platinum", out var e4);
    //
    //     return (e1, e2, e3, e4);
    // }
    //
    [Benchmark]
    public (EGCustomerType a, EGCustomerType b, EGCustomerType c, EGCustomerType d) EnumGenerators_Try()
    {
        EGCustomerTypeExtensions.TryParse("Standard", out var e1);
        EGCustomerTypeExtensions.TryParse("Gold", out var e2);
        EGCustomerTypeExtensions.TryParse("Diamond", out var e3);
        EGCustomerTypeExtensions.TryParse("Platinum", out var e4);
    
        return (e1, e2, e3, e4);
    }
    
    [Benchmark]
    public (IECustomerType a, IECustomerType b, IECustomerType c, IECustomerType d) Intellenums_Try()
    {
        IECustomerType.TryFromName("Standard", out var e1);
        IECustomerType.TryFromName("Gold", out var e2);
        IECustomerType.TryFromName("Diamond", out var e3);
        IECustomerType.TryFromName("Platinum", out var e4);
    
        return (e1, e2, e3, e4);
    }
    //
    // [Benchmark]
    // public (IECustomerType e1, IECustomerType e2, IECustomerType e3, IECustomerType e4) Intellenums()
    // {
    //     var e1 = IECustomerType.FromName("Standard");
    //     var e2 = IECustomerType.FromName("Gold");
    //     var e3 = IECustomerType.FromName("Diamond");
    //     var e4 = IECustomerType.FromName("Platinum");
    //
    //     return (e1, e2, e3, e4);
    // }
    //
    // [Benchmark]
    // public (SECustomerType e1, SECustomerType e2, SECustomerType e3, SECustomerType e4) SmartEnums()
    // {
    //     var e1 = SECustomerType.FromName("Standard");
    //     var e2 = SECustomerType.FromName("Gold");
    //     var e3 = SECustomerType.FromName("Diamond");
    //     var e4 = SECustomerType.FromName("Platinum");
    //
    //     return (e1, e2, e3, e4);
    // }
}
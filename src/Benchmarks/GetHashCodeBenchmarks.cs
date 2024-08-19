using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[JsonExporterAttribute.Full]
[JsonExporterAttribute.FullCompressed]
[MemoryDiagnoser]
public class GetHashCodeBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public int StandardEnums()
    {
        return ECustomerType.Standard.GetHashCode();
    }

    [Benchmark]
    public int Intellenums()
    {
        return IECustomerType.Standard.GetHashCode();
    }
    
    [Benchmark]
    public int SmartEnums()
    {
        return SECustomerType.Standard.GetHashCode();
    }
}
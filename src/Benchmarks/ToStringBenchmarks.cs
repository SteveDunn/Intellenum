using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ToStringBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public string StandardEnums()
    {
        return ECustomerType.Standard.ToString();
    }

    [Benchmark]
    public string Intellenums()
    {
        return IECustomerType.Standard.ToString();
    }
    
    [Benchmark]
    public string EnumGenerators()
    {
        return EGCustomerTypeExtensions.ToStringFast(EGCustomerType.Standard);
    }

    [Benchmark]
    public string SmartEnums()
    {
        return SECustomerType.Standard.ToString();
    }
}
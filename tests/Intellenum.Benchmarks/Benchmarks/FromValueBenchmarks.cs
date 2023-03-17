using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class FromValueBenchmarks
{
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public int StandardEnums()
    {
        int b = (int) ECustomerType.Standard;
        b += (int) ECustomerType.Gold;
        b += (int) ECustomerType.Diamond;
        b += (int) ECustomerType.Platinum;

        return b;
    }

    [Benchmark]
    public int Intellenums()
    {
        int b = IECustomerType.Standard.Value;
        b += IECustomerType.Gold.Value;
        b += IECustomerType.Diamond.Value;
        b += IECustomerType.Platinum.Value;

        return b;
    }

    [Benchmark]
    public int SmartEnums()
    {
        int b = SECustomerType.Standard.Value;
        b += SECustomerType.Gold.Value;
        b += SECustomerType.Diamond.Value;
        b += SECustomerType.Platinum.Value;

        return b;
    }

    // EnumGenerators Not Applicable as it uses the standard enum
}


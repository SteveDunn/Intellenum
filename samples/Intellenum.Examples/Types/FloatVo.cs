namespace Intellenum.Examples.Types
{
    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    public readonly partial struct Celsius { }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial struct FloatVo { }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial struct NoConverterFloatVo { }

    [Intellenum<float>(conversions: Conversions.TypeConverter)]
    public partial struct NoJsonFloatVo { }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson)]
    public partial struct NewtonsoftJsonFloatVo { }

    [Intellenum<float>(conversions: Conversions.SystemTextJson)]
    public partial struct SystemTextJsonFloatVo { }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    public partial struct BothJsonFloatVo { }

    [Intellenum<float>(conversions: Conversions.EfCoreValueConverter)]
    public partial struct EfCoreFloatVo { }

    [Intellenum<float>(conversions: Conversions.DapperTypeHandler)]
    public partial struct DapperFloatVo { }

    [Intellenum<float>(conversions: Conversions.LinqToDbValueConverter)]
    public partial struct LinqToDbFloatVo { }
}

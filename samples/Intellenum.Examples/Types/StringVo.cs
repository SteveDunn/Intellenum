namespace Intellenum.Examples.Types
{
    [Intellenum<string>(conversions: Conversions.None)]
    public partial struct StringVo { }

    [Intellenum<string>(conversions: Conversions.None)]
    public partial struct NoConverterStringVo { }

    [Intellenum<string>(conversions: Conversions.TypeConverter)]
    public partial struct NoJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson)]
    public partial struct NewtonsoftJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.SystemTextJson)]
    public partial struct SystemTextJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    public partial struct BothJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.EfCoreValueConverter)]
    public partial struct EfCoreStringVo { }

    [Intellenum<string>(conversions: Conversions.DapperTypeHandler)]
    public partial struct DapperStringVo { }

    [Intellenum<string>(conversions: Conversions.LinqToDbValueConverter)]
    public partial struct LinqToDbStringVo { }
}

namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(decimal))]
    public partial class DecimalVo
    {
        static DecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(decimal))]
    public partial class NoConverterDecimalVo
    {
        static NoConverterDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(decimal))]
    public partial class NoJsonDecimalVo
    {
        static NoJsonDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(decimal))]
    public partial class NewtonsoftJsonDecimalVo
    {
        static NewtonsoftJsonDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(decimal))]
    public partial class SystemTextJsonDecimalVo
    {
        static SystemTextJsonDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(decimal),
        customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    public partial class SystemTextJsonDecimalVo_Treating_number_as_string
    {
        static SystemTextJsonDecimalVo_Treating_number_as_string()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(decimal))]
    public partial class BothJsonDecimalVo
    {
        static BothJsonDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(decimal))]
    public partial class EfCoreDecimalVo
    {
        static EfCoreDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(decimal))]
    public partial class DapperDecimalVo
    {
        static DapperDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(decimal))]
    public partial class LinqToDbDecimalVo
    {
        static LinqToDbDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }
    }
}

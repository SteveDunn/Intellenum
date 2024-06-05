namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(decimal))]
    public partial class DecimalEnum
    {
        static DecimalEnum()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(decimal))]
    public partial class NoConverterDecimalVo
    {
        static NoConverterDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(decimal))]
    public partial class NoJsonDecimalVo
    {
        static NoJsonDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(decimal))]
    public partial class NewtonsoftJsonDecimalVo
    {
        static NewtonsoftJsonDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(decimal))]
    public partial class SystemTextJsonDecimalVo
    {
        static SystemTextJsonDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(decimal),
        customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    public partial class SystemTextJsonDecimalVo_Treating_number_as_string
    {
        static SystemTextJsonDecimalVo_Treating_number_as_string()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(decimal))]
    public partial class BothJsonDecimalVo
    {
        static BothJsonDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(decimal))]
    public partial class EfCoreDecimalVo
    {
        static EfCoreDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(decimal))]
    public partial class DapperDecimalVo
    {
        static DapperDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(decimal))]
    public partial class LinqToDbDecimalVo
    {
        static LinqToDbDecimalVo()
        {
            Member("Item1", 1.1m);
            Member("Item2", 2.2m);
        }
    }
}

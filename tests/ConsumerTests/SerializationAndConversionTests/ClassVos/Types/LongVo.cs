namespace Intellenum.IntegrationTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class LongVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoConverterLongVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoJsonLongVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NewtonsoftJsonLongVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonLongVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(long), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonLongVo_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class BothJsonLongVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class EfCoreLongVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class DapperLongVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class LinqToDbLongVo { }
}

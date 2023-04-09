namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class LongEnum { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoConverterLongEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoJsonLongEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NewtonsoftJsonLongEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonLongEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(long), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonLongEnum_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class BothJsonLongEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class EfCoreLongEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class DapperLongEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(long))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class LinqToDbLongEnum { }
}

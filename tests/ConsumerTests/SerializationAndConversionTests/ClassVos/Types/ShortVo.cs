namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class ShortEnum { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoConverterShortEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoJsonShortEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NewtonsoftJsonShortEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonShortEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(short), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonShortEnum_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class BothJsonShortEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class EfCoreShortEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class DapperShortEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(short))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class LinqToDbShortEnum { }
}

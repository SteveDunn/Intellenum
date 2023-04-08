namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class IntEnum { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoConverterIntEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoJsonIntEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NewtonsoftJsonIntEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonIntEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(int), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonIntEnum_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class BothJsonIntEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class EfCoreIntEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class DapperIntEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class LinqToDbIntEnum { }
}

namespace Intellenum.IntegrationTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class IntVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoConverterIntVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoJsonIntVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NewtonsoftJsonIntVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonIntVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(int), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonIntVo_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class BothJsonIntVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class EfCoreIntVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class DapperIntVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class LinqToDbIntVo { }
}

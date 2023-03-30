namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class ShortVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoConverterShortVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NoJsonShortVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class NewtonsoftJsonShortVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonShortVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(short), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class SystemTextJsonShortVo_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class BothJsonShortVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class EfCoreShortVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class DapperShortVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class LinqToDbShortVo { }
}

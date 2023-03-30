namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class FloatVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class NoConverterFloatVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class NoJsonFloatVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class NewtonsoftJsonFloatVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class SystemTextJsonFloatVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(float), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class SystemTextJsonFloatVo_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class BothJsonFloatVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class EfCoreFloatVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class DapperFloatVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class LinqToDbFloatVo { }
}

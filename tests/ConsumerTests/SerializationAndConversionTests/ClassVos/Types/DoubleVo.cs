namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class DoubleVo
    {
        
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NoConverterDoubleVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NoJsonDoubleVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NewtonsoftJsonDoubleVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class SystemTextJsonDoubleVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(double), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class SystemTextJsonDoubleVo_number_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class BothJsonDoubleVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class EfCoreDoubleVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class DapperDoubleVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class LinqToDbDoubleVo { }
}

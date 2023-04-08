namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class DoubleEnum
    {
        
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NoConverterDoubleEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NoJsonDoubleEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class NewtonsoftJsonDoubleEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class SystemTextJsonDoubleEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(double), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class SystemTextJsonDoubleEnum_number_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class BothJsonDoubleEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class EfCoreDoubleEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class DapperDoubleEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class LinqToDbDoubleEnum { }
}

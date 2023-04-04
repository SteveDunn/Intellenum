namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class BoolVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class NoConverterBoolVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class NoJsonBoolVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class NewtonsoftJsonBoolVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class SystemTextJsonBoolVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class BothJsonBoolVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class EfCoreBoolVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class DapperBoolVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class LinqToDbBoolVo { }
}

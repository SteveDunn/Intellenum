namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class StringVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NoConverterStringVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(string))]
    [Instance("Item1", "Item1!")]
    [Instance("Item2", "Item2!")]
    public partial class NoJsonStringVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(string))]
    [Instance("Item1", "Item1!")]
    [Instance("Item2", "Item2!")]
    public partial class NewtonsoftJsonStringVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(string))]
    [Instance("Item1", "Item1!")]
    [Instance("Item2", "Item2!")]
    public partial class SystemTextJsonStringVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class BothJsonStringVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class EfCoreStringVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(string))]
    [Instance("Item1", "Item1!")]
    [Instance("Item2", "Item2!")]
    public partial class DapperStringVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class LinqToDbStringVo { }
}

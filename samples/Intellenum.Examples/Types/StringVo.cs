namespace Intellenum.Examples.Types
{
    [Intellenum<string>(conversions: Conversions.None)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class StringVo { }

    [Intellenum<string>(conversions: Conversions.None)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NoConverterStringVo { }

    [Intellenum<string>(conversions: Conversions.TypeConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NoJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NewtonsoftJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.SystemTextJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class SystemTextJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class BothJsonStringVo { }

    [Intellenum<string>(conversions: Conversions.EfCoreValueConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class EfCoreStringVo { }

    [Intellenum<string>(conversions: Conversions.DapperTypeHandler)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class DapperStringVo { }

    [Intellenum<string>(conversions: Conversions.LinqToDbValueConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class LinqToDbStringVo { }
}

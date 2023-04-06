namespace Intellenum.Examples.Types
{
    [Intellenum<string>(conversions: Conversions.None)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class StringEnums { }

    [Intellenum<string>(conversions: Conversions.None)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NoConverterStringEnum { }

    [Intellenum<string>(conversions: Conversions.TypeConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NoJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class NewtonsoftJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.SystemTextJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class SystemTextJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class BothJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.EfCoreValueConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class EfCoreStringEnum { }

    [Intellenum<string>(conversions: Conversions.DapperTypeHandler)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class DapperStringEnum { }

    [Intellenum<string>(conversions: Conversions.LinqToDbValueConverter)]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class LinqToDbStringEnum { }
}

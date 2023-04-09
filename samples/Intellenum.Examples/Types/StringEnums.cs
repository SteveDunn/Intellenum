namespace Intellenum.Examples.Types
{
    [Intellenum<string>(conversions: Conversions.None)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class StringEnums { }

    [Intellenum<string>(conversions: Conversions.None)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class NoConverterStringEnum { }

    [Intellenum<string>(conversions: Conversions.TypeConverter)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class NoJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class NewtonsoftJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.SystemTextJson)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class SystemTextJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class BothJsonStringEnum { }

    [Intellenum<string>(conversions: Conversions.EfCoreValueConverter)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class EfCoreStringEnum { }

    [Intellenum<string>(conversions: Conversions.DapperTypeHandler)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class DapperStringEnum { }

    [Intellenum<string>(conversions: Conversions.LinqToDbValueConverter)]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class LinqToDbStringEnum { }
}

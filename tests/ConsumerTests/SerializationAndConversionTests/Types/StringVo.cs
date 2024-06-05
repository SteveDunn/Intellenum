namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(string))]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class StringEnum { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(string))]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class NoConverterStringEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(string))]
    [Member("Item1", "Item1!")]
    [Member("Item2", "Item2!")]
    public partial class NoJsonStringEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(string))]
    [Member("Item1", "Item1!")]
    [Member("Item2", "Item2!")]
    public partial class NewtonsoftJsonStringEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(string))]
    [Member("Item1", "Item1!")]
    [Member("Item2", "Item2!")]
    public partial class SystemTextJsonStringEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(string))]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class BothJsonStringEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(string))]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class EfCoreStringEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(string))]
    [Member("Item1", "Item1!")]
    [Member("Item2", "Item2!")]
    public partial class DapperStringEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(string))]
    [Member("Item1", "Item1")]
    [Member("Item2", "Item2")]
    public partial class LinqToDbStringEnum { }
}

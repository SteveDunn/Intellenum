// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable PartialTypeWithSinglePart

namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class CharEnum { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class NoConverterCharEnum { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class NoJsonCharEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class NewtonsoftJsonCharEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class SystemTextJsonCharEnum { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class BothJsonCharEnum { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class EfCoreCharEnum { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class DapperCharEnum { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(char))]
    [Member("A", 'a')]
    [Member("B", 'b')]
    public partial class LinqToDbCharEnum { }
}

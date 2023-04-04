namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class CharVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class NoConverterCharVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class NoJsonCharVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class NewtonsoftJsonCharVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class SystemTextJsonCharVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class BothJsonCharVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class EfCoreCharVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class DapperCharVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class LinqToDbCharVo { }
}

namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class ByteVo { }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoConverterByteVo { }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NoJsonByteVo { }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class NewtonsoftJsonByteVo { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonByteEnum { }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(byte), customizations: Customizations.TreatNumberAsStringInSystemTextJson)]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class SystemTextJsonByteVo_Treating_numbers_as_string { }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class BothJsonByteVo { }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class EfCoreByteVo { }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class DapperByteVo { }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(byte))]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class LinqToDbByteVo { }
}

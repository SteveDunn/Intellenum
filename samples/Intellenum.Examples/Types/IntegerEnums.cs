namespace Intellenum.Examples.Types;

// the underlying type can be omitted and is defaulted to int
[Intellenum]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class MyIntellenum { }

// the underlying type can be specified
[Intellenum(typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class MyIntellenum2 { }

// conversions can be specified, but if not, it defaults to TypeConverter and SystemTextJson
[Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class IntEnum { }

[Intellenum<int>(conversions: Conversions.None)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class IntGenericVo { }

[Intellenum<int>(conversions: Conversions.None)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NoConverterIntEnum { }

[Intellenum(conversions: Conversions.TypeConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NoJsonIntEnum { }

[Intellenum(conversions: Conversions.NewtonsoftJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NewtonsoftJsonIntEnum { }

[Intellenum(conversions: Conversions.SystemTextJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class SystemTextJsonIntEnum { }

[Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class BothJsonIntEnum { }

[Intellenum(conversions: Conversions.EfCoreValueConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class EfCoreIntEnum { }

[Intellenum(conversions: Conversions.DapperTypeHandler)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class DapperIntEnum { }

[Intellenum(conversions: Conversions.LinqToDbValueConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class LinqToDbIntEnum { }
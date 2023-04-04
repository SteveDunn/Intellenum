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
public partial class IntVo { }

[Intellenum<int>(conversions: Conversions.None)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class IntGenericVo { }

[Intellenum<int>(conversions: Conversions.None)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NoConverterIntVo { }

[Intellenum(conversions: Conversions.TypeConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NoJsonIntVo { }

[Intellenum(conversions: Conversions.NewtonsoftJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class NewtonsoftJsonIntVo { }

[Intellenum(conversions: Conversions.SystemTextJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class SystemTextJsonIntVo { }

[Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class BothJsonIntVo { }

[Intellenum(conversions: Conversions.EfCoreValueConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class EfCoreIntVo { }

[Intellenum(conversions: Conversions.DapperTypeHandler)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class DapperIntVo { }

[Intellenum(conversions: Conversions.LinqToDbValueConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class LinqToDbIntVo { }
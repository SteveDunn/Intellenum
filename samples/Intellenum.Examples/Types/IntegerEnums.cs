namespace Intellenum.Examples.Types;

// the underlying type can be omitted and is defaulted to int
[Intellenum]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class MyIntellenum { }

// the underlying type can be specified
[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class MyIntellenum2 { }

// conversions can be specified, but if not, it defaults to TypeConverter and SystemTextJson
[Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntEnum { }

[Intellenum<int>(conversions: Conversions.None)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntGenericVo { }

[Intellenum<int>(conversions: Conversions.None)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class NoConverterIntEnum { }

[Intellenum(conversions: Conversions.TypeConverter)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class NoJsonIntEnum { }

[Intellenum(conversions: Conversions.NewtonsoftJson)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class NewtonsoftJsonIntEnum { }

[Intellenum(conversions: Conversions.SystemTextJson)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class SystemTextJsonIntEnum { }

[Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class BothJsonIntEnum { }

[Intellenum(conversions: Conversions.EfCoreValueConverter)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class EfCoreIntEnum { }

[Intellenum(conversions: Conversions.DapperTypeHandler)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class DapperIntEnum { }

[Intellenum(conversions: Conversions.LinqToDbValueConverter)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class LinqToDbIntEnum { }

[Intellenum(conversions: Conversions.ServiceStackDotText)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class ServiceStackDotTextIntEnum { }

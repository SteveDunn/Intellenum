namespace Intellenum.Examples.Types;

// the underlying type can be omitted and is defaulted to int
[Intellenum]
public partial struct MyValueObject { }

// the underlying type can be specified
[Intellenum(typeof(int))]
public partial struct MyValueObject2 { }

// conversions can be specified, but if not, it defaults to TypeConverter and SystemTextJson
[Intellenum(conversions: Conversions.None, underlyingType: typeof(int))]
public partial struct IntVo { }

[Intellenum<int>(conversions: Conversions.None)]
public partial struct IntGenericVo { }

[Intellenum<int>(conversions: Conversions.None)]
public partial struct NoConverterIntVo { }

[Intellenum(conversions: Conversions.TypeConverter)]
public partial struct NoJsonIntVo { }

[Intellenum(conversions: Conversions.NewtonsoftJson)]
public partial struct NewtonsoftJsonIntVo { }

[Intellenum(conversions: Conversions.SystemTextJson)]
public partial struct SystemTextJsonIntVo { }

[Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
public partial struct BothJsonIntVo { }

[Intellenum(conversions: Conversions.EfCoreValueConverter)]
public partial struct EfCoreIntVo { }

[Intellenum(conversions: Conversions.DapperTypeHandler)]
public partial struct DapperIntVo { }

[Intellenum(conversions: Conversions.LinqToDbValueConverter)]
public partial struct LinqToDbIntVo { }
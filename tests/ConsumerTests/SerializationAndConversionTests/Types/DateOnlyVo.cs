using System;

namespace ConsumerTests.TestEnums;

[Intellenum(conversions: Conversions.None, underlyingType: typeof(DateOnly))]
public partial class DateOnlyVo
{
    static DateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.None, underlyingType: typeof(DateOnly))]
public partial class NoConverterDateOnlyVo
{
    static NoConverterDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
        
}

[Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateOnly))]
public partial class NoJsonDateOnlyVo
{
    static NoJsonDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateOnly))]
public partial class NewtonsoftJsonDateOnlyVo
{
    static NewtonsoftJsonDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateOnly))]
public partial class SystemTextJsonDateOnlyVo
{
    static SystemTextJsonDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateOnly))]
public partial class BothJsonDateOnlyVo
{
    static BothJsonDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateOnly))]
public partial class EfCoreDateOnlyVo
{
    static EfCoreDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateOnly))]
public partial class DapperDateOnlyVo
{
    static DapperDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateOnly))]
public partial class LinqToDbDateOnlyVo
{
    static LinqToDbDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}
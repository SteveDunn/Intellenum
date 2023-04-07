#if NET6_0_OR_GREATER

using System;

namespace Intellenum.IntegrationTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateOnly))]
    public partial class DateOnlyVo
    {
        static DateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateOnly))]
    public partial class NoConverterDateOnlyVo
    {
        static NoConverterDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
        
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateOnly))]
    public partial class NoJsonDateOnlyVo
    {
        static NoJsonDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateOnly))]
    public partial class NewtonsoftJsonDateOnlyVo
    {
        static NewtonsoftJsonDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateOnly))]
    public partial class SystemTextJsonDateOnlyVo
    {
        static SystemTextJsonDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateOnly))]
    public partial class BothJsonDateOnlyVo
    {
        static BothJsonDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateOnly))]
    public partial class EfCoreDateOnlyVo
    {
        static EfCoreDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateOnly))]
    public partial class DapperDateOnlyVo
    {
        static DapperDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateOnly))]
    public partial class LinqToDbDateOnlyVo
    {
        static LinqToDbDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }
    }
}

#endif

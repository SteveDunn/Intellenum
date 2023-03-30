using System;

namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class NoConverterDateTimeOffsetVo
    {
        static NoConverterDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class NoJsonDateTimeOffsetVo
    {
        static NoJsonDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Instance("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateTimeOffset))]
    public partial class NewtonsoftJsonDateTimeOffsetVo
    {
        static NewtonsoftJsonDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class SystemTextJsonDateTimeOffsetVo
    {
        static SystemTextJsonDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class BothJsonDateTimeOffsetVo
    {
        static BothJsonDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class EfCoreDateTimeOffsetVo
    {
        static EfCoreDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateTimeOffset))]
    public partial class DapperDateTimeOffsetVo
    {
        static DapperDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Instance("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class LinqToDbDateTimeOffsetVo
    {
        static LinqToDbDateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }
}
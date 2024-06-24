namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class NoConverterDateTimeOffsetVo
    {
        static NoConverterDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class NoJsonDateTimeOffsetVo
    {
        static NoJsonDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Member("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateTimeOffset))]
    public partial class NewtonsoftJsonDateTimeOffsetVo
    {
        static NewtonsoftJsonDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class SystemTextJsonDateTimeOffsetEnum
    {
        static SystemTextJsonDateTimeOffsetEnum()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class BothJsonDateTimeOffsetVo
    {
        static BothJsonDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class EfCoreDateTimeOffsetVo
    {
        static EfCoreDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateTimeOffset))]
    public partial class DapperDateTimeOffsetVo
    {
        static DapperDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Member("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class LinqToDbDateTimeOffsetVo
    {
        static LinqToDbDateTimeOffsetVo()
        {
            Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
            Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
        }
    }
}
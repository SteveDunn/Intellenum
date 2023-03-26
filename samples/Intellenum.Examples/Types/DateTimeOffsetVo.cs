using System;

namespace Intellenum.Examples.Types
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        public static readonly DateTimeOffsetVo None = new DateTimeOffsetVo("None", DateTimeOffset.MinValue);
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class NoConverterDateTimeOffsetVo
    {
        static NoConverterDateTimeOffsetVo()
        {
            Instance("Item1", DateTimeOffset.Parse("2020-01-01T00:00:00Z"));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class NoJsonDateTimeOffsetVo
    {
        static NoJsonDateTimeOffsetVo()
        {
            Instance("Item1", DateTimeOffset.Parse("2020-01-01T00:00:00Z"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateTimeOffset))]
    public partial class NewtonsoftJsonDateTimeOffsetVo
    {
        static NewtonsoftJsonDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class SystemTextJsonDateTimeOffsetVo
    {
        static SystemTextJsonDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class BothJsonDateTimeOffsetVo
    {
        static BothJsonDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class EfCoreDateTimeOffsetVo
    {
        static EfCoreDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateTimeOffset))]
    public partial class DapperDateTimeOffsetVo
    {
        static DapperDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class LinqToDbDateTimeOffsetVo
    {
        static LinqToDbDateTimeOffsetVo()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }
}
using System;

namespace Intellenum.Examples.Types
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetEnum
    {
        public static readonly DateTimeOffsetEnum None = new DateTimeOffsetEnum("None", DateTimeOffset.MinValue);
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTimeOffset))]
    public partial class NoConverterDateTimeOffsetEnum
    {
        static NoConverterDateTimeOffsetEnum()
        {
            Instance("Item1", DateTimeOffset.Parse("2020-01-01T00:00:00Z"));
            Instance("Item2", DateTimeOffset.Parse("2020-01-02T00:00:00Z"));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class NoJsonDateTimeOffsetEnum
    {
        static NoJsonDateTimeOffsetEnum()
        {
            Instance("Item1", DateTimeOffset.Parse("2020-01-01T00:00:00Z"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateTimeOffset))]
    public partial class NewtonsoftJsonDateTimeOffsetEnum
    {
        static NewtonsoftJsonDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class SystemTextJsonDateTimeOffsetEnum
    {
        static SystemTextJsonDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateTimeOffset))]
    public partial class BothJsonDateTimeOffsetEnum
    {
        static BothJsonDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class EfCoreDateTimeOffsetEnum
    {
        static EfCoreDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateTimeOffset))]
    public partial class DapperDateTimeOffsetEnum
    {
        static DapperDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateTimeOffset))]
    public partial class LinqToDbDateTimeOffsetEnum
    {
        static LinqToDbDateTimeOffsetEnum()
        {
            Instance("Item1", new DateTimeOffset(2019, 12, 13, 14, 15, 16, TimeSpan.Zero));
            Instance("Item2", new DateTimeOffset(2020, 12, 13, 14, 15, 16, TimeSpan.Zero));
        }
    }
}
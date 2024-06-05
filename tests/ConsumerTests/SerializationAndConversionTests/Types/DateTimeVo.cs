namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTime))]
    public partial class DateTimeEnum
    {
        static DateTimeEnum()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(DateTime))]
    public partial class NoConverterDateTimeVo
    {
        static NoConverterDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(DateTime))]
    public partial class NoJsonDateTimeVo
    {
        static NoJsonDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16, DateTimeKind.Utc) + TimeSpan.FromTicks(12345678));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
            // 2022-01-15T19:08:49.5413764+00:00
            Member("Item3", new DateTime(2022, 01, 15, 19, 08, 49, DateTimeKind.Local).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(DateTime))]
    public partial class NewtonsoftJsonDateTimeVo
    {
        static NewtonsoftJsonDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(DateTime))]
    public partial class SystemTextJsonDateTimeVo
    {
        static SystemTextJsonDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(DateTime))]
    public partial class BothJsonDateTimeVo
    {
        static BothJsonDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(DateTime))]
    public partial class EfCoreDateTimeVo
    {
        static EfCoreDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(DateTime))]
    public partial class DapperDateTimeVo
    {
        static DapperDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
            Member("SomethingElse", new DateTime(2022,01,15,19,08,49,DateTimeKind.Utc).AddTicks(5413764));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(DateTime))]
    public partial class LinqToDbDateTimeVo
    {
        static LinqToDbDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }
}
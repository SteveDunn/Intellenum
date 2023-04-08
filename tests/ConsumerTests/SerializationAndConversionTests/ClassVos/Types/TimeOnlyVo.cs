#if NET6_0_OR_GREATER

using System;

namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class TimeOnlyEnum
    {
        static TimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class NoConverterTimeOnlyEnum
    {
        static NoConverterTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(TimeOnly))]
    public partial class NoJsonTimeOnlyEnum
    {
        static NoJsonTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(TimeOnly))]
    public partial class NewtonsoftJsonTimeOnlyEnum
    {
        static NewtonsoftJsonTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class SystemTextJsonTimeOnlyEnum
    {
        static SystemTextJsonTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class BothJsonTimeOnlyEnum
    {
        static BothJsonTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class EfCoreTimeOnlyEnum
    {
        static EfCoreTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(TimeOnly))]
    public partial class DapperTimeOnlyEnum
    {
        static DapperTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class LinqToDbTimeOnlyEnum
    {
        static LinqToDbTimeOnlyEnum()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }
}
#endif
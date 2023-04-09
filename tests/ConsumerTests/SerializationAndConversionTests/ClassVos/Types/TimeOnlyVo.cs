#if NET6_0_OR_GREATER

using System;

namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class TimeOnlyEnum
    {
        static TimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class NoConverterTimeOnlyEnum
    {
        static NoConverterTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(TimeOnly))]
    public partial class NoJsonTimeOnlyEnum
    {
        static NoJsonTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(TimeOnly))]
    public partial class NewtonsoftJsonTimeOnlyEnum
    {
        static NewtonsoftJsonTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class SystemTextJsonTimeOnlyEnum
    {
        static SystemTextJsonTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class BothJsonTimeOnlyEnum
    {
        static BothJsonTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class EfCoreTimeOnlyEnum
    {
        static EfCoreTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(TimeOnly))]
    public partial class DapperTimeOnlyEnum
    {
        static DapperTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class LinqToDbTimeOnlyEnum
    {
        static LinqToDbTimeOnlyEnum()
        {
            Member("Item1", new TimeOnly(1, 2, 3, 04));
            Member("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }
}
#endif
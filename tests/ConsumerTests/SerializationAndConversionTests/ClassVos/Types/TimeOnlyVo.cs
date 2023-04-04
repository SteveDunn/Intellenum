#if NET6_0_OR_GREATER

using System;

namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class TimeOnlyVo
    {
        static TimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(TimeOnly))]
    public partial class NoConverterTimeOnlyVo
    {
        static NoConverterTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(TimeOnly))]
    public partial class NoJsonTimeOnlyVo
    {
        static NoJsonTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(TimeOnly))]
    public partial class NewtonsoftJsonTimeOnlyVo
    {
        static NewtonsoftJsonTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class SystemTextJsonTimeOnlyVo
    {
        static SystemTextJsonTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(TimeOnly))]
    public partial class BothJsonTimeOnlyVo
    {
        static BothJsonTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }


    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class EfCoreTimeOnlyVo
    {
        static EfCoreTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(TimeOnly))]
    public partial class DapperTimeOnlyVo
    {
        static DapperTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(TimeOnly))]
    public partial class LinqToDbTimeOnlyVo
    {
        static LinqToDbTimeOnlyVo()
        {
            Instance("Item1", new TimeOnly(1, 2, 3, 04));
            Instance("Item2", new TimeOnly(5, 6, 7, 08));
        }
    }
}



#endif
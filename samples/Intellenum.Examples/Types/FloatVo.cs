namespace Intellenum.Examples.Types
{
    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    public partial class Celsius
    {
        static Celsius()
        {
            Instance("AbsoluteZero", -273.15f);
            Instance("FreezingPointOfWater", 0f);
            Instance("BoilingPointOfWater", 100f);
        }
    }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial class FloatVo
    {
        static FloatVo()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial class NoConverterFloatVo
    {
        static NoConverterFloatVo()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.TypeConverter)]
    public partial class NoJsonFloatVo
    {
        static NoJsonFloatVo()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson)]
    public partial class NewtonsoftJsonFloatVo
    {
        static NewtonsoftJsonFloatVo()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.SystemTextJson)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class SystemTextJsonFloatVo
    {
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class BothJsonFloatVo
    {
    }

    [Intellenum<float>(conversions: Conversions.EfCoreValueConverter)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class EfCoreFloatVo
    {
    }

    [Intellenum<float>(conversions: Conversions.DapperTypeHandler)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class DapperFloatVo
    {
    }

    [Intellenum<float>(conversions: Conversions.LinqToDbValueConverter)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class LinqToDbFloatVo
    {
    }
}
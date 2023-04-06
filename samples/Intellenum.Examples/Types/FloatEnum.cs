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
    public partial class FloatEnum
    {
        static FloatEnum()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial class NoConverterFloatEnum
    {
        static NoConverterFloatEnum()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.TypeConverter)]
    public partial class NoJsonFloatEnum
    {
        static NoJsonFloatEnum()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson)]
    public partial class NewtonsoftJsonFloatEnum
    {
        static NewtonsoftJsonFloatEnum()
        {
            Instance("Item1", 1.23f);
            Instance("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.SystemTextJson)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class SystemTextJsonFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class BothJsonFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.EfCoreValueConverter)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class EfCoreFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.DapperTypeHandler)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class DapperFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.LinqToDbValueConverter)]
    [Instance("Item1", 1f)]
    [Instance("Item2", 2f)]
    public partial class LinqToDbFloatEnum
    {
    }
}
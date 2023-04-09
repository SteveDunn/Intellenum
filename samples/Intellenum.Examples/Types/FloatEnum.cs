namespace Intellenum.Examples.Types
{
    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    public partial class Celsius
    {
        static Celsius()
        {
            Member("AbsoluteZero", -273.15f);
            Member("FreezingPointOfWater", 0f);
            Member("BoilingPointOfWater", 100f);
        }
    }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial class FloatEnum
    {
        static FloatEnum()
        {
            Member("Item1", 1.23f);
            Member("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.None)]
    public partial class NoConverterFloatEnum
    {
        static NoConverterFloatEnum()
        {
            Member("Item1", 1.23f);
            Member("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.TypeConverter)]
    public partial class NoJsonFloatEnum
    {
        static NoJsonFloatEnum()
        {
            Member("Item1", 1.23f);
            Member("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson)]
    public partial class NewtonsoftJsonFloatEnum
    {
        static NewtonsoftJsonFloatEnum()
        {
            Member("Item1", 1.23f);
            Member("Item2", 3.21f);
        }
    }

    [Intellenum<float>(conversions: Conversions.SystemTextJson)]
    [Member("Item1", 1f)]
    [Member("Item2", 2f)]
    public partial class SystemTextJsonFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson)]
    [Member("Item1", 1f)]
    [Member("Item2", 2f)]
    public partial class BothJsonFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.EfCoreValueConverter)]
    [Member("Item1", 1f)]
    [Member("Item2", 2f)]
    public partial class EfCoreFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.DapperTypeHandler)]
    [Member("Item1", 1f)]
    [Member("Item2", 2f)]
    public partial class DapperFloatEnum
    {
    }

    [Intellenum<float>(conversions: Conversions.LinqToDbValueConverter)]
    [Member("Item1", 1f)]
    [Member("Item2", 2f)]
    public partial class LinqToDbFloatEnum
    {
    }
}
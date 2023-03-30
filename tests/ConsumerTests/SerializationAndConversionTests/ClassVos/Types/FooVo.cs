using System;

namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    public record struct Bar(int Age, string Name) : IComparable<Bar>
    {
        public int CompareTo(Bar other) => Age.CompareTo(other.Age);
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Bar))]
    public partial class FooVo
    {
        static FooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Bar))]
    public partial class NoConverterFooVo
    {
        static NoConverterFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(Bar))]
    public partial class NoJsonFooVo
    {
        static NoJsonFooVo()
        {
            Instance("Item1", new NoJsonFooVo(new Bar(42, "Fred")));
            Instance("Item2", new NoJsonFooVo(new Bar(2, "Two")));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(Bar))]
    public partial class NewtonsoftJsonFooVo
    {
        static NewtonsoftJsonFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class SystemTextJsonFooVo
    {
        static SystemTextJsonFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class BothJsonFooVo
    {
        static BothJsonFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class BothJsonFooVoClass
    {
        static BothJsonFooVoClass()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(Bar))]
    public partial class EfCoreFooVo
    {
        static EfCoreFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(Bar))]
    public partial class DapperFooVo
    {
        static DapperFooVo()
        {
            Instance("Item1", new DapperFooVo(new Bar(42, "Fred")));
            Instance("Item2", new DapperFooVo(new Bar(2, "Two")));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(Bar))]
    public partial class LinqToDbFooVo
    {
        static LinqToDbFooVo()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }
}

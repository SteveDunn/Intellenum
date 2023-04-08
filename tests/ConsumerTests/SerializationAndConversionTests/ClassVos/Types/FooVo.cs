namespace ConsumerTests.TestEnums
{
    public record struct Bar(int Age, string Name) : IComparable<Bar>
    {
        public int CompareTo(Bar other) => Age.CompareTo(other.Age);
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Bar))]
    public partial class NoConverterFooEnum
    {
        static NoConverterFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(Bar))]
    public partial class NoJsonFooEnum
    {
        static NoJsonFooEnum()
        {
            Instance("Item1", new NoJsonFooEnum(new Bar(42, "Fred")));
            Instance("Item2", new NoJsonFooEnum(new Bar(2, "Two")));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(Bar))]
    public partial class NewtonsoftJsonFooEnum
    {
        static NewtonsoftJsonFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class SystemTextJsonFooEnum
    {
        static SystemTextJsonFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class BothJsonFooEnum
    {
        static BothJsonFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Bar))]
    public partial class BothJsonFooEnumClass
    {
        static BothJsonFooEnumClass()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(Bar))]
    public partial class EfCoreFooEnum
    {
        static EfCoreFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }

    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(Bar))]
    public partial class DapperFooEnum
    {
        static DapperFooEnum()
        {
            Instance("Item1", new DapperFooEnum(new Bar(42, "Fred")));
            Instance("Item2", new DapperFooEnum(new Bar(2, "Two")));
        }
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(Bar))]
    public partial class LinqToDbFooEnum
    {
        static LinqToDbFooEnum()
        {
            Instance("Item1", new Bar(1, "One"));
            Instance("Item2", new Bar(2, "Two"));
        }
    }
}

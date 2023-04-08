namespace ConsumerTests.TestEnums
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Guid))]
    public partial class GuidEnum
    {
        static GuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Guid))]
    public partial class NoConverterGuidEnum
    {
        static NoConverterGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(Guid))]
    public partial class NoJsonGuidEnum
    {
        static NoJsonGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(Guid))]
    public partial class NewtonsoftJsonGuidEnum
    {
        static NewtonsoftJsonGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(Guid))]
    public partial class SystemTextJsonGuidEnum
    {
        static SystemTextJsonGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Guid))]
    public partial class BothJsonGuidEnum
    {
        static BothJsonGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(Guid))]
    public partial class EfCoreGuidEnum
    {
        static EfCoreGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }    
    }


    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(Guid))]
    public partial class DapperGuidEnum
    {
        static DapperGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
        
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(Guid))]
    public partial class LinqToDbGuidEnum
    {
        static LinqToDbGuidEnum()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }
}

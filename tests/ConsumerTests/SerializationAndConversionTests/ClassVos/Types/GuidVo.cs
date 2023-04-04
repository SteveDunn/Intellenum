using System;

namespace Intellenum.IntegrationTests.TestTypes.ClassVos
{
    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Guid))]
    public partial class GuidVo
    {
        static GuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.None, underlyingType: typeof(Guid))]
    public partial class NoConverterGuidVo
    {
        static NoConverterGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.TypeConverter, underlyingType: typeof(Guid))]
    public partial class NoJsonGuidVo
    {
        static NoJsonGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson, underlyingType: typeof(Guid))]
    public partial class NewtonsoftJsonGuidVo
    {
        static NewtonsoftJsonGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.SystemTextJson, underlyingType: typeof(Guid))]
    public partial class SystemTextJsonGuidVo
    {
        static SystemTextJsonGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.NewtonsoftJson | Conversions.SystemTextJson, underlyingType: typeof(Guid))]
    public partial class BothJsonGuidVo
    {
        static BothJsonGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }

    [Intellenum(conversions: Conversions.EfCoreValueConverter, underlyingType: typeof(Guid))]
    public partial class EfCoreGuidVo
    {
        static EfCoreGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }    
    }


    [Intellenum(conversions: Conversions.DapperTypeHandler, underlyingType: typeof(Guid))]
    public partial class DapperGuidVo
    {
        static DapperGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
        
    }

    [Intellenum(conversions: Conversions.LinqToDbValueConverter, underlyingType: typeof(Guid))]
    public partial class LinqToDbGuidVo
    {
        static LinqToDbGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }
    }
}

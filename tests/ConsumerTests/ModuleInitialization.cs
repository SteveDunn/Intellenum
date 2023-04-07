using System.Runtime.CompilerServices;
using Dapper;
using LinqToDB.Mapping;

namespace ConsumerTests;

public static class ModuleInitialization
{
    [ModuleInitializer]
    public static void Init()
    {
        SqlMapper.AddTypeHandler(new ConsumerTests.DeserializationTests.MyIntEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.DeserializationTests.MyStringEnum.DapperTypeHandler());

#if NET6_0_OR_GREATER
        MappingSchema.Default.SetConverter<DateTime, TimeOnly>(dt => TimeOnly.FromDateTime(dt));
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperDateOnlyVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperTimeOnlyVo.DapperTypeHandler());
#endif

#if NET7_0_OR_GREATER
        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationTests.MyVoInt_should_not_bypass_validation.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationTests.MyVoString.DapperTypeHandler());
#endif

        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperFooVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperCharEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperBoolVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperByteVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperDateTimeOffsetVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperDateTimeVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperIntVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperStringVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperLongVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperShortVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperFloatEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperDoubleVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperDecimalVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestEnums.DapperGuidVo.DapperTypeHandler());
    }
}
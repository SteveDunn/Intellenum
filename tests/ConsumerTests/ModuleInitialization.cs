using System;
using System.Runtime.CompilerServices;
using Dapper;
using LinqToDB.Mapping;

namespace ConsumerTests;

public static class ModuleInitialization
{
    [ModuleInitializer]
    public static void Init()
    {
        SqlMapper.AddTypeHandler(new ConsumerTests.DeserializationValidationTests.MyVoInt_should_not_bypass_validation.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.DeserializationValidationTests.MyVoString.DapperTypeHandler());

#if NET6_0_OR_GREATER
        MappingSchema.Default.SetConverter<DateTime, TimeOnly>(dt => TimeOnly.FromDateTime(dt));
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperDateOnlyVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperTimeOnlyVo.DapperTypeHandler());
#endif

#if NET7_0_OR_GREATER
        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationValidationTests.MyVoInt_should_not_bypass_validation.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationValidationTests.MyVoString_should_not_bypass_validation.DapperTypeHandler());
#endif

        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperFooVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperCharVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperBoolVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperByteVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperDateTimeOffsetVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperDateTimeVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperIntVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperStringVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperLongVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperShortVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperFloatVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperDoubleVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperDecimalVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new Intellenum.IntegrationTests.TestTypes.ClassVos.DapperGuidVo.DapperTypeHandler());
    }
}
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

        MappingSchema.Default.SetConverter<DateTime, TimeOnly>(dt => TimeOnly.FromDateTime(dt));
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperDateOnlyVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperTimeOnlyEnum.DapperTypeHandler());

        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationTests.MyIntEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.GenericDeserializationTests.MyStringEnum.DapperTypeHandler());

        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperFooEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperCharEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperBoolVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperByteVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperDateTimeOffsetVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperDateTimeVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperIntEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperStringEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperLongEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperShortEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperFloatEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperDoubleEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperDecimalVo.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new ConsumerTests.TestEnums.DapperGuidEnum.DapperTypeHandler());
    }
}
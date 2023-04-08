using System;
using System.Runtime.CompilerServices;
using Dapper;
using Intellenum.Examples.Types;
using LinqToDB.Mapping;

namespace Intellenum.Examples;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        MappingSchema.Default.SetConverter<DateTime, TimeOnly>(dt => TimeOnly.FromDateTime(dt));
        SqlMapper.AddTypeHandler(new DapperDateTimeOffsetEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new DapperIntEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new DapperStringEnum.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new DapperFloatEnum.DapperTypeHandler());
    }
}
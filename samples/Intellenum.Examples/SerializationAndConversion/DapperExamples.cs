using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Intellenum.Examples.Types;
using Microsoft.Data.Sqlite;

namespace Intellenum.Examples.SerializationAndConversion
{
    public class DapperExamples : IScenario
    {
        public async Task Run()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            IEnumerable<DapperDateTimeOffsetEnum> results = await connection.QueryAsync<DapperDateTimeOffsetEnum>("SELECT '2022-01-15 19:08:49.5413764'");

            DapperDateTimeOffsetEnum result = results.Single();

            Console.WriteLine(result);
        }

    }
}
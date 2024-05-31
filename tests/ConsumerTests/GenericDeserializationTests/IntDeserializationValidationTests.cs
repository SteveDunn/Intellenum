// ReSharper disable NullableWarningSuppressionIsUsed

#if NET7_0_OR_GREATER

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Intellenum;
using Dapper;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using LinqToDB;

namespace ConsumerTests.GenericDeserializationTests;

public class IntDeserializationValidationTests
{
    [Fact]
    public async Task Deserialization_dapper()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        
        var actual = (await connection.QueryAsync<MyIntEnum>("SELECT 1")).AsList()[0].Value;

        actual.Should().Be(1);
    }

    [Fact]
    public async Task Deserialization_efcore()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<DeserializationValidationDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new DeserializationValidationDbContext(options))
        {
            var actual = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.IntEntities!.FromSqlRaw("SELECT 1 As Id"));
            actual.Id!.Value.Should().Be(1);
        }
    }

    [Fact]
    public async Task Deserialization_linqtodb()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        using (var context = new DeserializationValidationDataConnection(connection))
        {
            var actual = await LinqToDB.AsyncExtensions.SingleAsync(context.FromSql<DeserializationValidationTestLinqToDbTestIntEntity>("SELECT 1 As Id"));
            actual.Id!.Value.Should().Be(1);
        }
    }

    [Fact]
    public void TypeConversion()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyIntEnum));
        var validValue = 1;

        var actual = ((MyIntEnum?)converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_systemtextjson()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyIntEnum.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyIntEnum>(validValue)!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_newtonsoft()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyIntEnum.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyIntEnum>(validValue)!.Value;
        
        actual.Should().Be(1);
    }
}
#endif
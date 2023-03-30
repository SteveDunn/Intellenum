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

namespace ConsumerTests.GenericDeserializationValidationTests;

public class IntDeserializationValidationTests
{
    [Fact]
    public async void Deserialization_dapper_should_not_bypass_validation_pass()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        
        var actual = (await connection.QueryAsync<MyVoInt_should_not_bypass_validation>("SELECT 1")).AsList()[0].Value;

        actual.Should().Be(1);
    }

    [Fact]
    public async void Deserialization_efcore()
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
    public async void Deserialization_linqtodb()
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
        var converter = TypeDescriptor.GetConverter(typeof(MyVoInt_should_not_bypass_validation));
        var validValue = 1;

        var actual = ((MyVoInt_should_not_bypass_validation?)converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_systemtextjson()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyVoInt_should_not_bypass_validation.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyVoInt_should_not_bypass_validation>(validValue)!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_newtonsoft()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyVoInt_should_not_bypass_validation.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyVoInt_should_not_bypass_validation>(validValue)!.Value;
        
        actual.Should().Be(1);
    }
}
#endif
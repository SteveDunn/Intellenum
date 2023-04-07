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
// ReSharper disable NullableWarningSuppressionIsUsed

namespace ConsumerTests.DeserializationTests;

public class StringDeserializationValidationTests
{
    [Fact]
    public async void Deserialization_dapper()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var actual = (await connection.QueryAsync<MyStringEnum>("SELECT 'Item1!'")).AsList()[0].Value;

        actual.Should().Be("Item1!");
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
            DeserializationValidationTestStringEntity actual = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.StringEntities!.FromSqlRaw("SELECT 'Item1!' As Id"));
            actual.Id!.Value.Should().Be("Item1!");
        }
    }

    [Fact]
    public async void Deserialization_linqtodb()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        using (var context = new DeserializationValidationDataConnection(connection))
        {
            var actual = await LinqToDB.AsyncExtensions.SingleAsync(context.FromSql<DeserializationValidationTestLinqToDbTestStringEntity>("SELECT 'Item1!' As Id"));
            actual.Id!.Value.Should().Be("Item1!");
        }
    }

    [Fact]
    public void TypeConversion()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyStringEnum));
        var validValue = "Item1!";

        var actual = ((MyStringEnum?) converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be("Item1!");
    }

    [Fact]
    public void Deserialization_systemtextjson()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyStringEnum.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyStringEnum>(validValue)!.Value;

        actual.Should().Be("Item1!");
    }


    [Fact]
    public void Deserialization_newtonsoft()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyStringEnum.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyStringEnum>(validValue)!.Value;

        actual.Should().Be("Item1!");
    }
}
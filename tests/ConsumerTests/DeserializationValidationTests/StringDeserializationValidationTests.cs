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

namespace ConsumerTests.DeserializationValidationTests;

public class StringDeserializationValidationTests
{
    [Fact]
    public async void Deserialization_dapper()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var actual = (await connection.QueryAsync<MyVoString>("SELECT 'Item1!'")).AsList()[0].Value;

        actual.Should().Be("Item11");
    }

    [Fact]
    public async void Deserialization_efcore_should_not_bypass_validation_pass()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<DeserializationValidationDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new DeserializationValidationDbContext(options))
        {
            var actual = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.StringEntities!.FromSqlRaw("SELECT 'abcdefghijk' As Id"));
            actual.Id!.Value.Should().Be("Item1");
        }
    }

    [Fact]
    public async void Deserialization_efcore_should_not_bypass_validation_fail()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<DeserializationValidationDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new DeserializationValidationDbContext(options))
        {
            Func<Task<string>> vo = async () => (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.StringEntities!.FromSqlRaw("SELECT 'abc' As Id"))).Id!.Value;
            await vo.Should().ThrowExactlyAsync<IntellenumValidationException>().WithMessage("length must be greater than ten characters");
        }
    }
    [Fact]
    public async void Deserialization_linqtodb_should_not_bypass_validation_pass()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        using (var context = new DeserializationValidationDataConnection(connection))
        {
            var actual = await LinqToDB.AsyncExtensions.SingleAsync(context.FromSql<DeserializationValidationTestLinqToDbTestStringEntity>("SELECT 'abcdefghijk' As Id"));
            actual.Id!.Value.Should().Be("Item1");
        }
    }

    [Fact]
    public void TypeConversion_should_not_bypass_validation_pass()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyVoString));
        var validValue = "Item1";

        var actual = ((MyVoString?) converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be("Item1");
    }

    [Fact]
    public void Deserialization_systemtextjson_should_not_bypass_validation_pass()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyVoString.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyVoString>(validValue)!.Value;

        actual.Should().Be("Item1");
    }

    [Fact]
    public void Deserialization_systemtextjson_should_not_bypass_validation_fail()
    {
        var invalidValue = SystemTextJsonSerializer.Serialize(MyVoString.Item1).Replace("Item1", "abc");

        Action vo = () => SystemTextJsonSerializer.Deserialize<MyVoString>(invalidValue);

        vo.Should().ThrowExactly<IntellenumValidationException>().WithMessage("length must be greater than ten characters");
    }

    [Fact]
    public void Deserialization_newtonsoft_should_not_bypass_validation_pass()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyVoString.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyVoString>(validValue)!.Value;

        actual.Should().Be("Item1");
    }

    [Fact]
    public void Deserialization_newtonsoft_should_not_bypass_validation_fail()
    {
        var invalidValue = NewtonsoftJsonSerializer.SerializeObject(MyVoString.Item1).Replace("Item1", "abc");

        Func<string> vo = () => NewtonsoftJsonSerializer.DeserializeObject<MyVoString>(invalidValue)!.Value;

        vo.Should().ThrowExactly<IntellenumValidationException>().WithMessage("length must be greater than ten characters");
    }

}
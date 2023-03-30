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
    public async void Deserialization_dapper_should_throw_on_no_match()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        
        Func<Task<int>> vo = async () => (await connection.QueryAsync<MyVoInt_should_not_bypass_validation>("SELECT 0")).AsList()[0].Value;

        await vo.Should().ThrowExactlyAsync<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
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
            var actual = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.IntEntities!.FromSqlRaw("SELECT 1 As Id"));
            actual.Id!.Value.Should().Be(1);
        }
    }

    [Fact]
    public async void Deserialization_efcore_should_throw_on_no_match()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<DeserializationValidationDbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new DeserializationValidationDbContext(options))
        {
            Func<Task<int>> vo = async () => (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(context.IntEntities!.FromSqlRaw("SELECT 0 As Id"))).Id!.Value;
            await vo.Should().ThrowExactlyAsync<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
        }
    }
    [Fact]
    public async void Deserialization_linqtodb_should_not_bypass_validation_pass()
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
    public async void Deserialization_linqtodb_should_throw_on_no_match()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        //var original = new TestEntity { Id = LinqToDbStringVo.From("foo!") };
        using (var context = new DeserializationValidationDataConnection(connection))
        {
            Func<Task<int>> vo = async () => (await LinqToDB.AsyncExtensions.SingleAsync(context.FromSql<DeserializationValidationTestLinqToDbTestIntEntity>("SELECT 0 As Id"))).Id!.Value;
            await vo.Should().ThrowExactlyAsync<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
        }
    }

    [Fact]
    public void TypeConversion_should_not_bypass_validation_pass()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyVoInt_should_not_bypass_validation));
        var validValue = 1;

        var actual = ((MyVoInt_should_not_bypass_validation?)converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void TypeConversion_should_throw_on_no_match()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyVoInt_should_not_bypass_validation));
        var invalidValue = 0;

        Action vo = () => converter.ConvertFrom(invalidValue);

        vo.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
    }

    [Fact]
    public void Deserialization_systemtextjson_should_not_bypass_validation_pass()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyVoInt_should_not_bypass_validation.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyVoInt_should_not_bypass_validation>(validValue)!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_systemtextjson_should_throw_on_no_match()
    {
        var invalidValue = SystemTextJsonSerializer.Serialize(MyVoInt_should_not_bypass_validation.Item1).Replace("1", "0");

        Action vo = () => SystemTextJsonSerializer.Deserialize<MyVoInt_should_not_bypass_validation>(invalidValue);

        vo.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
    }

    [Fact]
    public void Deserialization_newtonsoft_should_not_bypass_validation_pass()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyVoInt_should_not_bypass_validation.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyVoInt_should_not_bypass_validation>(validValue)!.Value;
        
        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_newtonsoft_should_throw_on_no_match()
    {
        var invalidValue = NewtonsoftJsonSerializer.SerializeObject(MyVoInt_should_not_bypass_validation.Item1).Replace("1", "0");

        Func<int> vo = () => NewtonsoftJsonSerializer.DeserializeObject<MyVoInt_should_not_bypass_validation>(invalidValue)!.Value;

        vo.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyVoInt_should_not_bypass_validation has no matching instances with a value of '0'");
    }

}
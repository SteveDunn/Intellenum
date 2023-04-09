using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using LinqToDB;
// ReSharper disable NullableWarningSuppressionIsUsed

namespace ConsumerTests.DeserializationTests;

public class IntDeserializationTests
{
    [Fact]
    public async void Deserialization_dapper_should_not_bypass_validation_pass()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        
        var actual = (await connection.QueryAsync<MyIntEnum>("SELECT 1")).AsList()[0].Value;

        actual.Should().Be(1);
    }

    [Fact]
    public async void Deserialization_dapper_should_throw_on_no_match()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        
        Func<Task<int>> f = async () => (await connection.QueryAsync<MyIntEnum>("SELECT 0")).AsList()[0].Value;

        await f.Should().ThrowExactlyAsync<IntellenumMatchFailedException>()
            .WithMessage("MyIntEnum has no matching members with a value of '0'");
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
            Func<Task<int>> f = async () =>
                (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleAsync(
                    context.IntEntities!.FromSqlRaw("SELECT 0 As Id"))).Id!.Value;

            await f.Should().ThrowExactlyAsync<IntellenumMatchFailedException>()
                .WithMessage("MyIntEnum has no matching members with a value of '0'");
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

        //var original = new TestEntity { Id = LinqToDbStringEnum.From("foo!") };
        using (var context = new DeserializationValidationDataConnection(connection))
        {
            Func<Task<int>> f = async () =>
                (await LinqToDB.AsyncExtensions.SingleAsync(
                    context.FromSql<DeserializationValidationTestLinqToDbTestIntEntity>("SELECT 0 As Id"))).Id!.Value;

            await f.Should().ThrowExactlyAsync<IntellenumMatchFailedException>()
                .WithMessage("MyIntEnum has no matching members with a value of '0'");
        }
    }

    [Fact]
    public void TypeConversion_should_not_bypass_validation_pass()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyIntEnum));
        var validValue = 1;

        var actual = ((MyIntEnum?)converter.ConvertFrom(validValue))!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void TypeConversion_should_throw_on_no_match()
    {
        var converter = TypeDescriptor.GetConverter(typeof(MyIntEnum));
        var invalidValue = 0;

        Action a = () => converter.ConvertFrom(invalidValue);

        a.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyIntEnum has no matching members with a value of '0'");
    }

    [Fact]
    public void Deserialization_systemtextjson_should_not_bypass_validation_pass()
    {
        var validValue = SystemTextJsonSerializer.Serialize(MyIntEnum.Item1);

        var actual = SystemTextJsonSerializer.Deserialize<MyIntEnum>(validValue)!.Value;

        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_systemtextjson_should_throw_on_no_match()
    {
        var invalidValue = SystemTextJsonSerializer.Serialize(MyIntEnum.Item1).Replace("1", "0");

        Action a = () => SystemTextJsonSerializer.Deserialize<MyIntEnum>(invalidValue);

        a.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyIntEnum has no matching members with a value of '0'");
    }

    [Fact]
    public void Deserialization_newtonsoft_should_not_bypass_validation_pass()
    {
        var validValue = NewtonsoftJsonSerializer.SerializeObject(MyIntEnum.Item1);

        var actual = NewtonsoftJsonSerializer.DeserializeObject<MyIntEnum>(validValue)!.Value;
        
        actual.Should().Be(1);
    }

    [Fact]
    public void Deserialization_newtonsoft_should_throw_on_no_match()
    {
        var invalidValue = NewtonsoftJsonSerializer.SerializeObject(MyIntEnum.Item1).Replace("1", "0");

        Func<int> f = () => NewtonsoftJsonSerializer.DeserializeObject<MyIntEnum>(invalidValue)!.Value;

        f.Should().ThrowExactly<IntellenumMatchFailedException>().WithMessage("MyIntEnum has no matching members with a value of '0'");
    }

}
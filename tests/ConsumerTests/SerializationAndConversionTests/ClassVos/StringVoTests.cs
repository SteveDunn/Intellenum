#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(string))]
    [Instance("Item1", "Item1")]
    [Instance("Item2", "Item2")]
    public partial class AnotherStringVo { }

    public class StringVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            StringVo.Item1.Equals(StringVo.Item1).Should().BeTrue();
            (StringVo.Item1 == StringVo.Item1).Should().BeTrue();

            (StringVo.Item1 != StringVo.Item2).Should().BeTrue();
            (StringVo.Item1 == StringVo.Item2).Should().BeFalse();

            StringVo.Item1.Equals(StringVo.Item1).Should().BeTrue();
            (StringVo.Item1 == StringVo.Item1).Should().BeTrue();

            var original = StringVo.Item1;
            var other = StringVo.Item1;

            ((original as IEquatable<StringVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<StringVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonStringVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonStringVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedString = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = "Item1!";
            var vo = NewtonsoftJsonStringVo.Item1;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonStringVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = "Item1!";
            var vo = SystemTextJsonStringVo.Item1;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonStringVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var vo = BothJsonStringVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedString2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedString1);
            Assert.Equal(serializedVo2, serializedString2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var vo = NoJsonStringVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + vo.Value + "\",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonStringVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterStringVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + vo.Value + "\"}";

            Assert.Equal(expected, newtonsoft);
            Assert.Equal(expected, systemText);
        }

        [Fact]
        public void WhenEfCoreValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            var original = new EfCoreTestEntity { Id = EfCoreStringVo.Item1 };
            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Entities.Add(original);
                context.SaveChanges();
            }
            using (var context = new TestDbContext(options))
            {
                var all = context.Entities.ToList();
                var retrieved = Assert.Single(all);
                Assert.Equal(original.Id, retrieved.Id);
            }
        }

        [Fact]
        public async Task WhenDapperValueConverterUsesValueConverter()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            IEnumerable<DapperStringVo> results = await connection.QueryAsync<DapperStringVo>("SELECT 'Item1!'");

            var value = Assert.Single(results);
            Assert.Equal(DapperStringVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbStringVo.Item1 };
            using (var context = new DataConnection(
                SQLiteTools.GetDataProvider("SQLite.MS"),
                connection,
                disposeConnection: false))
            {
                context.CreateTable<LinqToDbTestEntity>();
                context.Insert(original);
            }
            using (var context = new DataConnection(
                SQLiteTools.GetDataProvider("SQLite.MS"),
                connection,
                disposeConnection: false))
            {
                var all = context.GetTable<LinqToDbTestEntity>().ToList();
                var retrieved = Assert.Single(all);
                Assert.Equal(original.Id, retrieved.Id);
            }
        }

        [Theory]
        [InlineData("Item1!")]
        [InlineData("Item2!")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonStringVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonStringVo>(id);
            Assert.Equal(NoJsonStringVo.FromValue(value!.ToString()), id);

            var reconverted = converter.ConvertTo(id, value.GetType());
            Assert.Equal(value, reconverted);
        }

        public class TestDbContext : DbContext
        {
            public DbSet<EfCoreTestEntity> Entities { get; set; }

            public TestDbContext(DbContextOptions options) : base(options)
            {
            }

             protected override void OnModelCreating(ModelBuilder modelBuilder)
             {
                 modelBuilder
                     .Entity<EfCoreTestEntity>(builder =>
                     {
                         builder
                             .Property(x => x.Id)
                             .HasConversion(new EfCoreStringVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreStringVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.VarChar)]
            [ValueConverter(ConverterType = typeof(LinqToDbStringVo.LinqToDbValueConverter))]
            public LinqToDbStringVo Id { get; set; }
        }
    }
}
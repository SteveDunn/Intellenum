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
    [Intellenum(underlyingType: typeof(long))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class AnotherLongVo { }

    public class LongVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            LongVo.Item1.Equals(LongVo.Item1).Should().BeTrue();
            (LongVo.Item1 == LongVo.Item1).Should().BeTrue();

            (LongVo.Item1 != LongVo.Item2).Should().BeTrue();
            (LongVo.Item1 == LongVo.Item2).Should().BeFalse();

            LongVo.Item1.Equals(LongVo.Item1).Should().BeTrue();
            (LongVo.Item1 == LongVo.Item1).Should().BeTrue();

            var original = LongVo.Item1;
            var other = LongVo.Item1;

            ((original as IEquatable<LongVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<LongVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToLong_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonLongVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedLong = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedLong);
        }

        [Fact]
        public void CanSerializeToLong_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonLongVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedLong = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedLong).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromLong_WithNewtonsoftJsonProvider()
        {
            var value = 1L;
            var vo = NewtonsoftJsonLongVo.Item1;
            var serializedLong = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonLongVo>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonLongVo.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonLongVo>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var vo = SystemTextJsonLongVo_Treating_numbers_as_string.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonLongVo_Treating_numbers_as_string>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToLong_WithBothJsonConverters()
        {
            var vo = BothJsonLongVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedLong1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedLong2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedLong1);
            Assert.Equal(serializedVo2, serializedLong2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var vo = NoJsonLongVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + ",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonLongVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterLongVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + "}";

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

            var original = new EfCoreTestEntity { Id = EfCoreLongVo.Item1 };
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

            IEnumerable<DapperLongVo> results = await connection.QueryAsync<DapperLongVo>("SELECT 1");

            var value = Assert.Single(results);
            Assert.Equal(DapperLongVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbLongVo.Item1 };
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
        [InlineData(1L)]
        [InlineData("1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonLongVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonLongVo>(id);
            Assert.Equal(NoJsonLongVo.Item1, id);

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
                             .HasConversion(new EfCoreLongVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreLongVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Int64)]
            [ValueConverter(ConverterType = typeof(LinqToDbLongVo.LinqToDbValueConverter))]
            public LinqToDbLongVo Id { get; set; }
        }
    }
}
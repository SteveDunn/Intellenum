#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Intellenum;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace MediumTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(double))]
    [Instance("Item1", 1.1d)]
    [Instance("Item2", 2.2d)]
    public partial class AnotherDoubleVo { }

    public class DoubleVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            DoubleVo.Item1.Equals(DoubleVo.Item1).Should().BeTrue();
            (DoubleVo.Item1 == DoubleVo.Item1).Should().BeTrue();

            (DoubleVo.Item1 != DoubleVo.Item2).Should().BeTrue();
            (DoubleVo.Item1 == DoubleVo.Item2).Should().BeFalse();

            DoubleVo.Item1.Equals(DoubleVo.Item1).Should().BeTrue();
            (DoubleVo.Item1 == DoubleVo.Item1).Should().BeTrue();

            var original = DoubleVo.Item1;
            var other = DoubleVo.Item1;

            ((original as IEquatable<DoubleVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DoubleVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToLong_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonDoubleVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedLong = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedLong);
        }

        [Fact]
        public void CanSerializeToLong_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonDoubleVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedLong = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedLong).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromLong_WithNewtonsoftJsonProvider()
        {
            var value = 123D;
            var vo = NewtonsoftJsonDoubleVo.Item1;
            var serializedLong = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDoubleVo>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider()
        {
            var value = 123D;
            var vo = SystemTextJsonDoubleVo.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleVo>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var vo = SystemTextJsonDoubleVo_number_as_string.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleVo_number_as_string>(serializedLong);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToLong_WithBothJsonConverters()
        {
            var vo = BothJsonDoubleVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedLong1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedLong2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedLong1);
            Assert.Equal(serializedVo2, serializedLong2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueProperty()
        {
            var vo = NoJsonDoubleVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + "}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonDoubleVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoTypeConverter_SerializesWithValueProperty()
        {
            var vo = NoConverterDoubleVo.Item1;

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

            var original = new EfCoreTestEntity { Id = EfCoreDoubleVo.Item1 };
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

            var parameters = new { Value = 123.45d };
            IEnumerable<DapperDoubleVo> results = await connection.QueryAsync<DapperDoubleVo>("SELECT @Value", parameters);

            var value = Assert.Single(results);
            Assert.Equal(DapperDoubleVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDoubleVo.Item1 };
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
        [InlineData(123.45D)]
        [InlineData("123.45")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var culture = new CultureInfo("en-US");

            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDoubleVo));
            var id = converter.ConvertFrom(null!, culture, value);
            Assert.IsType<NoJsonDoubleVo>(id);
            Assert.Equal(NoJsonDoubleVo.Item1, id);

            var reconverted = converter.ConvertTo(null, culture, id, value.GetType());
            Assert.Equal(value, reconverted);
        }
        
        [Fact]
        public void RoundTrip_WithNsj()
        {
            var vo = NewtonsoftJsonDoubleVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDoubleVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(123.45);
        }

        [Fact]
        public void RoundTrip_WithStj()
        {
            var vo = SystemTextJsonDoubleVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(123.45);
        }

        [Fact]
        public void RoundTrip_WithStj_Treating_numbers_as_string()
        {
            var vo = SystemTextJsonDoubleVo_number_as_string.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleVo_number_as_string>(serializedVo)!;

            deserializedVo.Value.Should().Be(123.45);
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
                             .HasConversion(new EfCoreDoubleVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDoubleVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Double)]
            [ValueConverter(ConverterType = typeof(LinqToDbDoubleVo.LinqToDbValueConverter))]
            public LinqToDbDoubleVo Id { get; set; }
        }
    }
}
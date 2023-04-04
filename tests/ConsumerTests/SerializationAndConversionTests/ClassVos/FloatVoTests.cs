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

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace MediumTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(float))]
    [Instance("Item1", 1.1f)]
    [Instance("Item2", 2.2f)]
    public partial class AnotherFloatVo { }

    public class FloatVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            FloatVo.Item1.Equals(FloatVo.Item1).Should().BeTrue();
            (FloatVo.Item1 == FloatVo.Item1).Should().BeTrue();

            (FloatVo.Item1 != FloatVo.Item2).Should().BeTrue();
            (FloatVo.Item1 == FloatVo.Item2).Should().BeFalse();

            FloatVo.Item1.Equals(FloatVo.Item1).Should().BeTrue();
            (FloatVo.Item1 == FloatVo.Item1).Should().BeTrue();

            var original = FloatVo.Item1;
            var other = FloatVo.Item1;

            ((original as IEquatable<FloatVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<FloatVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToFloat_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonFloatVo.Item2;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedFloat = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedFloat);
        }

        [Fact]
        public void RoundTrip_WithNsj()
        {
            var vo = NewtonsoftJsonFloatVo.Item2;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonFloatVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(2.2f);
        }

        [Fact]
        public void RoundTrip_WithStj()
        {
            var vo = SystemTextJsonFloatVo.Item2;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(2.2f);
        }

        [Fact]
        public void CanSerializeToFloat_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonFloatVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedFloat = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedFloat).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromFloat_WithNewtonsoftJsonProvider()
        {
            var value = 1.1f;
            var vo = NewtonsoftJsonFloatVo.Item1;
            var serializedFloat = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonFloatVo>(serializedFloat);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromFloat_WithSystemTextJsonProvider()
        {
            var value = 1.1f;
            var vo = SystemTextJsonFloatVo.Item1;
            var serializedFloat = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatVo>(serializedFloat);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromFloat_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var vo = SystemTextJsonFloatVo_Treating_numbers_as_string.Item1;
            var serializedFloat = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatVo_Treating_numbers_as_string>(serializedFloat);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToFloat_WithBothJsonConverters()
        {
            var vo = BothJsonFloatVo.Item2;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedFloat1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedFloat2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedFloat1);
            Assert.Equal(serializedVo2, serializedFloat2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var vo = NoJsonFloatVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

#if !NET5_0_OR_GREATER
            var expected = """{"Value":1.10000002,"Name":"Item1"}""";
#else
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#endif

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonFloatVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoTypeConverter_SerializesWithValueAndNameProperty()
        {
            var vo = NoConverterFloatVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

#if !NET5_0_OR_GREATER
            var expected = """{"Value":1.10000002,"Name":"Item1"}""";
#else
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#endif

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

            var original = new EfCoreTestEntity { Id = EfCoreFloatVo.Item1 };
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

            var parameters = new { Value = 2.2f };
            IEnumerable<DapperFloatVo> results = await connection.QueryAsync<DapperFloatVo>("SELECT @Value", parameters);

            var value = Assert.Single(results);
            Assert.Equal(DapperFloatVo.Item2, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbFloatVo.Item1 };
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
        [InlineData((float)1.1)]
        [InlineData("1.1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonFloatVo));

            var culture = new CultureInfo("en-US");
            
            var id = converter.ConvertFrom(null!, culture, value);
            Assert.IsType<NoJsonFloatVo>(id);
            Assert.Equal(NoJsonFloatVo.Item1, id);

            object reconverted = converter.ConvertTo(null, culture, id, value.GetType());
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
                             .HasConversion(new EfCoreFloatVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreFloatVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Single)]
            [ValueConverter(ConverterType = typeof(LinqToDbFloatVo.LinqToDbValueConverter))]
            public LinqToDbFloatVo Id { get; set; }
        }
    }
}
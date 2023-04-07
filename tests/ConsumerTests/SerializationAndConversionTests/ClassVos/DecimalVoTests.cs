// ReSharper disable NullableWarningSuppressionIsUsed

#nullable disable
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Dapper;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Intellenum.IntegrationTests.TestEnums;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace MediumTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(decimal))]
    public partial class AnotherDecimalVo
    {
        static AnotherDecimalVo()
        {
            Instance("Item1", 1.1m);
            Instance("Item2", 2.2m);
        }

    }

    public class DecimalVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            DecimalVo.Item1.Equals(DecimalVo.Item1).Should().BeTrue();
            (DecimalVo.Item1 == DecimalVo.Item1).Should().BeTrue();

            (DecimalVo.Item1 != DecimalVo.Item2).Should().BeTrue();
            (DecimalVo.Item1 == DecimalVo.Item2).Should().BeFalse();

            DecimalVo.Item1.Equals(DecimalVo.Item1).Should().BeTrue();
            (DecimalVo.Item1 == DecimalVo.Item1).Should().BeTrue();

            var original = DecimalVo.Item1;
            var other = DecimalVo.Item1;

            ((original as IEquatable<DecimalVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DecimalVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToLong_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonDecimalVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedLong = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedLong);
        }

        [Fact]
        public void CanSerializeToLong_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonDecimalVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedLong = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedLong).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromLong_WithNewtonsoftJsonProvider()
        {
            var value = 1.1m;
            var ie = NewtonsoftJsonDecimalVo.Item1;
            var serializedLong = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDecimalVo>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider()
        {
            var value = 1.1m;
            var ie = SystemTextJsonDecimalVo.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDecimalVo>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonDecimalVo_Treating_number_as_string.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDecimalVo_Treating_number_as_string>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToLong_WithBothJsonConverters()
        {
            var ie = BothJsonDecimalVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedLong1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedLong2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedLong1);
            Assert.Equal(serializedVo2, serializedLong2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithNameAndValueProperty()
        {
            var ie = NoJsonDecimalVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":1.1,"Name":"Item1"}""";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonDecimalVo.Item1;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = string.Format(CultureInfo.InvariantCulture, "\"{0}\"",  ie.Value);

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterDecimalVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":1.1,"Name":"Item1"}""";

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

            var original = new EfCoreTestEntity { Id = EfCoreDecimalVo.Item1 };
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

            var parameters = new { Value = 1.1m };
            IEnumerable<DapperDecimalVo> results = await connection.QueryAsync<DapperDecimalVo>("SELECT @Value", parameters);

            DapperDecimalVo value = Assert.Single(results);
            Assert.Equal(DapperDecimalVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDecimalVo.Item1 };
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
        
        [Fact]
        public void TypeConverter_CanConvertToAndFromDecimal()
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDecimalVo));
            var id = converter.ConvertFrom(1.1m);
            Assert.IsType<NoJsonDecimalVo>(id);
            Assert.Equal(NoJsonDecimalVo.Item1, id);

            var reconverted = converter.ConvertTo(id, typeof(decimal));
            Assert.Equal(1.1m, reconverted);
        }

        [Fact]
        public void TypeConverter_CanConvertToAndFrom()
        {
            var culture = new CultureInfo("en-US");

            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDecimalVo));
            var id = converter.ConvertFrom(null!, culture, "1.1");
            Assert.IsType<NoJsonDecimalVo>(id);
            Assert.Equal(NoJsonDecimalVo.Item1, id);

            var reconverted = converter.ConvertTo(null, culture, id, typeof(string));
            Assert.Equal("1.1", reconverted);
        }
        
        [Fact]
        public void RoundTrip_WithNsj()
        {
            var ie = NewtonsoftJsonDecimalVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDecimalVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1m);
        }

        [Fact]
        public void RoundTrip_WithStj()
        {
            var ie = SystemTextJsonDecimalVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDecimalVo>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1m);
        }

        [Fact]
        public void RoundTrip_WithStj_Treating_numbers_as_string()
        {
            var ie = SystemTextJsonDecimalVo_Treating_number_as_string.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDecimalVo_Treating_number_as_string>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1m);
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
                             .HasConversion(new EfCoreDecimalVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDecimalVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Decimal)]
            [ValueConverter(ConverterType = typeof(LinqToDbDecimalVo.LinqToDbValueConverter))]
            public LinqToDbDecimalVo Id { get; set; }
        }
    }
}
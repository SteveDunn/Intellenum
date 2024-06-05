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
using ConsumerTests.TestEnums;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace MediumTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(double))]
    [Member("Item1", 1.1d)]
    [Member("Item2", 2.2d)]
    public partial class AnotherDoubleVo { }

    public class DoubleVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            DoubleEnum.Item1.Equals(DoubleEnum.Item1).Should().BeTrue();
            (DoubleEnum.Item1 == DoubleEnum.Item1).Should().BeTrue();

            (DoubleEnum.Item1 != DoubleEnum.Item2).Should().BeTrue();
            (DoubleEnum.Item1 == DoubleEnum.Item2).Should().BeFalse();

            DoubleEnum.Item1.Equals(DoubleEnum.Item1).Should().BeTrue();
            (DoubleEnum.Item1 == DoubleEnum.Item1).Should().BeTrue();

            var original = DoubleEnum.Item1;
            var other = DoubleEnum.Item1;

            ((original as IEquatable<DoubleEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DoubleEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToLong_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonDoubleEnum.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedLong = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedLong);
        }

        [Fact]
        public void CanSerializeToLong_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonDoubleEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedLong = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedLong).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromLong_WithNewtonsoftJsonProvider()
        {
            var value = 1.1D;
            var ie = NewtonsoftJsonDoubleEnum.Item1;
            var serializedLong = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDoubleEnum>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider()
        {
            var value = 1.1D;
            var ie = SystemTextJsonDoubleEnum.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleEnum>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromLong_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonDoubleEnum_number_as_string.Item1;
            var serializedLong = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleEnum_number_as_string>(serializedLong);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToLong_WithBothJsonConverters()
        {
            var ie = BothJsonDoubleEnum.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedLong1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedLong2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedLong1);
            Assert.Equal(serializedVo2, serializedLong2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonDoubleEnum.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

#if NET5_0_OR_GREATER || NETCOREAPP3_1
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#else
            var expected = """{"Value":1.1000000000000001,"Name":"Item1"}""";
#endif

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonDoubleEnum.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterDoubleEnum.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

#if NET5_0_OR_GREATER || NETCOREAPP3_1
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#else
            var expected = """{"Value":1.1000000000000001,"Name":"Item1"}""";
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

            var original = new EfCoreTestEntity { Id = EfCoreDoubleEnum.Item1 };
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

            var parameters = new { Value = 1.1D };
            IEnumerable<DapperDoubleEnum> results = await connection.QueryAsync<DapperDoubleEnum>("SELECT @Value", parameters);

            var value = Assert.Single(results);
            Assert.Equal(DapperDoubleEnum.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDoubleEnum.Item1 };
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
        [InlineData(1.1D)]
        [InlineData("1.1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var culture = new CultureInfo("en-US");

            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDoubleEnum));
            object id = converter.ConvertFrom(null!, culture, value);
            Assert.IsType<NoJsonDoubleEnum>(id);
            Assert.Equal(NoJsonDoubleEnum.Item1, id);

            var reconverted = converter.ConvertTo(null, culture, id, value.GetType());
            Assert.Equal(value, reconverted);
        }
        
        [Fact]
        public void RoundTrip_WithNsj()
        {
            var ie = NewtonsoftJsonDoubleEnum.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDoubleEnum>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1D);
        }

        [Fact]
        public void RoundTrip_WithStj()
        {
            var ie = SystemTextJsonDoubleEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleEnum>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1D);
        }

        [Fact]
        public void RoundTrip_WithStj_Treating_numbers_as_string()
        {
            var ie = SystemTextJsonDoubleEnum_number_as_string.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDoubleEnum_number_as_string>(serializedVo)!;

            deserializedVo.Value.Should().Be(1.1D);
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
                             .HasConversion(new EfCoreDoubleEnum.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDoubleEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Double)]
            [ValueConverter(ConverterType = typeof(LinqToDbDoubleEnum.LinqToDbValueConverter))]
            public LinqToDbDoubleEnum Id { get; set; }
        }
    }
}
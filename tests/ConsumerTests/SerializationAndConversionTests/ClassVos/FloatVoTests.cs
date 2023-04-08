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
            FloatEnum.Item1.Equals(FloatEnum.Item1).Should().BeTrue();
            (FloatEnum.Item1 == FloatEnum.Item1).Should().BeTrue();

            (FloatEnum.Item1 != FloatEnum.Item2).Should().BeTrue();
            (FloatEnum.Item1 == FloatEnum.Item2).Should().BeFalse();

            FloatEnum.Item1.Equals(FloatEnum.Item1).Should().BeTrue();
            (FloatEnum.Item1 == FloatEnum.Item1).Should().BeTrue();

            var original = FloatEnum.Item1;
            var other = FloatEnum.Item1;

            ((original as IEquatable<FloatEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<FloatEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToFloat_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonFloatEnum.Item2;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedFloat = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedFloat);
        }

        [Fact]
        public void RoundTrip_WithNsj()
        {
            var ie = NewtonsoftJsonFloatEnum.Item2;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonFloatEnum>(serializedVo)!;

            deserializedVo.Value.Should().Be(2.2f);
        }

        [Fact]
        public void RoundTrip_WithStj()
        {
            var ie = SystemTextJsonFloatEnum.Item2;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatEnum>(serializedVo)!;

            deserializedVo.Value.Should().Be(2.2f);
        }

        [Fact]
        public void CanSerializeToFloat_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonFloatEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedFloat = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedFloat).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromFloat_WithNewtonsoftJsonProvider()
        {
            var value = 1.1f;
            var ie = NewtonsoftJsonFloatEnum.Item1;
            var serializedFloat = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonFloatEnum>(serializedFloat);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromFloat_WithSystemTextJsonProvider()
        {
            var value = 1.1f;
            var ie = SystemTextJsonFloatEnum.Item1;
            var serializedFloat = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatEnum>(serializedFloat);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromFloat_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonFloatEnum_Treating_numbers_as_string.Item1;
            var serializedFloat = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFloatEnum_Treating_numbers_as_string>(serializedFloat);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToFloat_WithBothJsonConverters()
        {
            var ie = BothJsonFloatEnum.Item2;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedFloat1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedFloat2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedFloat1);
            Assert.Equal(serializedVo2, serializedFloat2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonFloatEnum.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

#if NET5_0_OR_GREATER || NETCOREAPP3_1
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#elif NET461
            var expected = """{"Value":1.10000002,"Name":"Item1"}""";
#else
            var expected = """{"Value":1.1000000000000001,"Name":"Item1"}""";
#endif

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonFloatEnum.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoTypeConverter_SerializesWithValueAndNameProperty()
        {
            var ie = NoConverterFloatEnum.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

#if NET5_0_OR_GREATER || NETCOREAPP3_1
            var expected = """{"Value":1.1,"Name":"Item1"}""";
#elif NET461
            var expected = """{"Value":1.10000002,"Name":"Item1"}""";
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

            var original = new EfCoreTestEntity { Id = EfCoreFloatEnum.Item1 };
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
            IEnumerable<DapperFloatEnum> results = await connection.QueryAsync<DapperFloatEnum>("SELECT @Value", parameters);

            var value = Assert.Single(results);
            Assert.Equal(DapperFloatEnum.Item2, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbFloatEnum.Item1 };
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
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonFloatEnum));

            var culture = new CultureInfo("en-US");
            
            var id = converter.ConvertFrom(null!, culture, value);
            Assert.IsType<NoJsonFloatEnum>(id);
            Assert.Equal(NoJsonFloatEnum.Item1, id);

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
                             .HasConversion(new EfCoreFloatEnum.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreFloatEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Single)]
            [ValueConverter(ConverterType = typeof(LinqToDbFloatEnum.LinqToDbValueConverter))]
            public LinqToDbFloatEnum Id { get; set; }
        }
    }
}
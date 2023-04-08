#nullable disable
using System.ComponentModel;
using System.Threading.Tasks;
using ConsumerTests.TestEnums;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718


namespace ConsumerTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(short))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class AnotherShortVo { }

    public class ShortVoTests
    {
        [Fact]
        public void equality_between_same_enums()
        {
            ShortEnum.Item1.Equals(ShortEnum.Item1).Should().BeTrue();
            (ShortEnum.Item1 == ShortEnum.Item1).Should().BeTrue();

            (ShortEnum.Item1 != ShortEnum.Item2).Should().BeTrue();
            (ShortEnum.Item1 == ShortEnum.Item2).Should().BeFalse();

            ShortEnum.Item1.Equals(ShortEnum.Item1).Should().BeTrue();
            (ShortEnum.Item1 == ShortEnum.Item1).Should().BeTrue();

            var original = ShortEnum.Item1;
            var other = ShortEnum.Item1;

            ((original as IEquatable<ShortEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<ShortEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void equality_between_different_enums()
        {
            // the implicit cast to short means this is true
            ShortEnum.Item1.Equals(AnotherShortVo.Item1).Should().BeFalse();
        }

        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonShortEnum.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedShort = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedShort);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonShortEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedShort = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            short value = 1;
            var ie = NewtonsoftJsonShortEnum.Item1;
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonShortEnum>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            short value = 1;
            var ie = SystemTextJsonShortEnum.Item1;
            var serializedShort = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonShortEnum>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonShortEnum_Treating_numbers_as_string.Item1;
            var serializedShort = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonShortEnum_Treating_numbers_as_string>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var ie = BothJsonShortEnum.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedShort1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedShort2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedShort1);
            Assert.Equal(serializedVo2, serializedShort2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonShortEnum.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":" + ie.Value + ",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonShortEnum.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterShortEnum.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":1,"Name":"Item1"}""";

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

            var original = new EfCoreTestEntity { Id = EfCoreShortEnum.Item1 };
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

            IEnumerable<DapperShortEnum> results = await connection.QueryAsync<DapperShortEnum>("SELECT 1");

            var value = Assert.Single(results);
            Assert.Equal(DapperShortEnum.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbShortEnum.Item1 };
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
        [InlineData((short)1)]
        [InlineData("1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonShortEnum));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonShortEnum>(id);
            Assert.Equal(NoJsonShortEnum.Item1, id);

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
                             .HasConversion(new EfCoreShortEnum.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreShortEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Int16)]
            [ValueConverter(ConverterType = typeof(LinqToDbShortEnum.LinqToDbValueConverter))]
            public LinqToDbShortEnum Id { get; set; }
        }
    }
}
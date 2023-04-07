#nullable disable
using System.ComponentModel;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Intellenum.IntegrationTests.TestEnums;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718


namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
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
            ShortVo.Item1.Equals(ShortVo.Item1).Should().BeTrue();
            (ShortVo.Item1 == ShortVo.Item1).Should().BeTrue();

            (ShortVo.Item1 != ShortVo.Item2).Should().BeTrue();
            (ShortVo.Item1 == ShortVo.Item2).Should().BeFalse();

            ShortVo.Item1.Equals(ShortVo.Item1).Should().BeTrue();
            (ShortVo.Item1 == ShortVo.Item1).Should().BeTrue();

            var original = ShortVo.Item1;
            var other = ShortVo.Item1;

            ((original as IEquatable<ShortVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<ShortVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void equality_between_different_enums()
        {
            // the implicit cast to short means this is true
            ShortVo.Item1.Equals(AnotherShortVo.Item1).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonShortVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedShort = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedShort);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonShortVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedShort = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            short value = 1;
            var ie = NewtonsoftJsonShortVo.Item1;
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonShortVo>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            short value = 1;
            var ie = SystemTextJsonShortVo.Item1;
            var serializedShort = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonShortVo>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonShortVo_Treating_numbers_as_string.Item1;
            var serializedShort = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonShortVo_Treating_numbers_as_string>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var ie = BothJsonShortVo.Item1;

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
            var ie = NoJsonShortVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":" + ie.Value + ",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonShortVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterShortVo.Item1;

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

            var original = new EfCoreTestEntity { Id = EfCoreShortVo.Item1 };
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

            IEnumerable<DapperShortVo> results = await connection.QueryAsync<DapperShortVo>("SELECT 1");

            var value = Assert.Single(results);
            Assert.Equal(DapperShortVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbShortVo.Item1 };
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
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonShortVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonShortVo>(id);
            Assert.Equal(NoJsonShortVo.Item1, id);

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
                             .HasConversion(new EfCoreShortVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreShortVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Int16)]
            [ValueConverter(ConverterType = typeof(LinqToDbShortVo.LinqToDbValueConverter))]
            public LinqToDbShortVo Id { get; set; }
        }
    }
}
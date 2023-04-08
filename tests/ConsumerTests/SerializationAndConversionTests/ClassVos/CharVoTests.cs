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
// ReSharper disable RedundantOverflowCheckingContext
// ReSharper disable ConvertToLocalFunction
// ReSharper disable EqualExpressionComparison
// ReSharper disable SuspiciousTypeConversion.Global

namespace ConsumerTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(char))]
    [Instance("A", 'a')]
    [Instance("B", 'b')]
    public partial class AnotherCharVo { }

    public class CharVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            CharEnum.A.Equals(AnotherCharVo.A).Should().BeFalse();
            CharEnum.A.Equals(CharEnum.A).Should().BeTrue();
            (CharEnum.A == CharEnum.A).Should().BeTrue();

            (CharEnum.A != CharEnum.B).Should().BeTrue();
            (CharEnum.A == CharEnum.B).Should().BeFalse();

            CharEnum.A.Equals(CharEnum.A).Should().BeTrue();
            (CharEnum.A == CharEnum.A).Should().BeTrue();

            var original = CharEnum.A;
            var other = CharEnum.A;

            ((original as IEquatable<CharEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<CharEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonCharEnum.A;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedShort = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedShort);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonCharEnum.A;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedShort = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(NewtonsoftJsonCharEnum.A);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonCharEnum>(serializedShort);

            Assert.Equal(NewtonsoftJsonCharEnum.A, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonCharEnum.A;
            var serializedShort = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonCharEnum>(serializedShort);

            Assert.Equal(SystemTextJsonCharEnum.A, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var ie = BothJsonCharEnum.A;

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
            var ie = NoJsonCharEnum.A;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":\"a\",\"Name\":\"A\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonCharEnum.A;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterCharEnum.A;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":\"a\",\"Name\":\"A\"}";

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

            var original = new EfCoreTestEntity { Id = EfCoreCharEnum.A };
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

            IEnumerable<DapperCharEnum> results = await connection.QueryAsync<DapperCharEnum>("SELECT 'a'");

            var value = Assert.Single(results);
            Assert.Equal(DapperCharEnum.A, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbCharEnum.A };
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
        [InlineData((char) 97)]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonCharEnum));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonCharEnum>(id);
            Assert.Equal(NoJsonCharEnum.A, id);

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
                            .HasConversion(new EfCoreCharEnum.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public EfCoreCharEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Char)]
            [ValueConverter(ConverterType = typeof(LinqToDbCharEnum.LinqToDbValueConverter))]
            public LinqToDbCharEnum Id { get; set; }
        }
    }
}
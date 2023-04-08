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
    [Intellenum(underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class AnotherIntVo { }

    public class IntVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            IntEnum.Item1.Equals(IntEnum.Item1).Should().BeTrue();
            (IntEnum.Item1 == IntEnum.Item1).Should().BeTrue();

            (IntEnum.Item1 != IntEnum.Item2).Should().BeTrue();
            (IntEnum.Item1 == IntEnum.Item2).Should().BeFalse();

            IntEnum.Item1.Equals(IntEnum.Item1).Should().BeTrue();
            (IntEnum.Item1 == IntEnum.Item1).Should().BeTrue();

            var original = IntEnum.Item1;
            var other = IntEnum.Item1;

            ((original as IEquatable<IntEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<IntEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToInt_WithNewtonsoftJsonProvider()
        {
            var ie = NewtonsoftJsonIntEnum.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedInt = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedInt);
        }

        [Fact]
        public void CanSerializeToInt_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonIntEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedInt = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedInt).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromInt_WithNewtonsoftJsonProvider()
        {
            var value = 1;
            var ie = NewtonsoftJsonIntEnum.Item1;
            var serializedInt = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonIntEnum>(serializedInt);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromInt_WithSystemTextJsonProvider()
        {
            var value = 1;
            var ie = SystemTextJsonIntEnum.Item1;
            var serializedInt = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonIntEnum>(serializedInt);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromInt_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var ie = SystemTextJsonIntEnum_Treating_numbers_as_string.Item1;
            var serializedInt = SystemTextJsonSerializer.Serialize(ie);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonIntEnum_Treating_numbers_as_string>(serializedInt);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToInt_WithBothJsonConverters()
        {
            var ie = BothJsonIntEnum.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedInt1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedInt2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedInt1);
            Assert.Equal(serializedVo2, serializedInt2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonIntEnum.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":" + ie.Value + ",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonIntEnum.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterIntEnum.Item1;

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

            var original = new EfCoreTestEntity { Id = EfCoreIntEnum.Item1 };
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

            IEnumerable<DapperIntEnum> results = await connection.QueryAsync<DapperIntEnum>("SELECT 1");

            var value = Assert.Single(results);
            Assert.Equal(DapperIntEnum.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbIntEnum.Item1 };
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
        [InlineData(1)]
        [InlineData("1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonIntEnum));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonIntEnum>(id);
            Assert.Equal(NoJsonIntEnum.Item1, id);

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
                             .HasConversion(new EfCoreIntEnum.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreIntEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Int32)]
            [ValueConverter(ConverterType = typeof(LinqToDbIntEnum.LinqToDbValueConverter))]
            public LinqToDbIntEnum Id { get; set; }
        }
    }
}
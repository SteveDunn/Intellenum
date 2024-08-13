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
// ReSharper disable RedundantCast
// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable SuspiciousTypeConversion.Global

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace ConsumerTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(DateTime))]
    public partial class AnotherDateTimeVo
    {
        static AnotherDateTimeVo()
        {
            Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
            Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
        }
    }

    public class DateTimeVoTests
    {
        private readonly DateTime _date1 = new DateTime(2019, 12, 13, 14, 15, 16, DateTimeKind.Utc) + TimeSpan.FromTicks(12345678);

        [Fact]
        public void equality_between_same_value_objects()
        {
            DateTimeEnum.Item1.Equals(DateTimeEnum.Item1).Should().BeTrue();
            (DateTimeEnum.Item1 == DateTimeEnum.Item1).Should().BeTrue();

            (DateTimeEnum.Item1 != DateTimeEnum.Item2).Should().BeTrue();
            (DateTimeEnum.Item1 == DateTimeEnum.Item2).Should().BeFalse();

            DateTimeEnum.Item1.Equals(DateTimeEnum.Item1).Should().BeTrue();
            (DateTimeEnum.Item1 == DateTimeEnum.Item1).Should().BeTrue();

            var original = DateTimeEnum.Item1;
            var other = DateTimeEnum.Item1;

            ((original as IEquatable<DateTimeEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DateTimeEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var g1 = NewtonsoftJsonDateTimeVo.Item1;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serialized, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonDateTimeVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedString = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonDateTimeVo.Item1.Value;
            var ie = NewtonsoftJsonDateTimeVo.Item1;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDateTimeVo>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = NewtonsoftJsonDateTimeVo.Item1.Value;
            var ie = SystemTextJsonDateTimeVo.Item1;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDateTimeVo>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var ie = BothJsonDateTimeVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedString2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedString1);
            Assert.Equal(serializedVo2, serializedString2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonDateTimeVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":\"" + _date1.ToString("O") + "\",\"Name\":\"Item1\"}";

            serialized.Should().Be(expected);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonDateTimeVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{_date1:o}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterDateTimeVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":"1970-06-10T14:01:03.2345678Z","Name":"Item1"}""";

            newtonsoft.Should().Be(expected);
            systemText.Should().Be(expected);
        }

        [Fact]
        public void WhenEfCoreValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            var original = new EfCoreTestEntity { Id = EfCoreDateTimeVo.Item1 };
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

            IEnumerable<DapperDateTimeVo> results = await connection.QueryAsync<DapperDateTimeVo>("SELECT '2022-01-15 19:08:49.5413764'");

            DapperDateTimeVo actual = Assert.Single(results);

            var expected = DapperDateTimeVo.SomethingElse;
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDateTimeVo.Item1 };
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
        [InlineData("2022-01-15T19:08:49.5413764+00:00")]
        public void TypeConverter_CanConvertToAndFrom(string inputString)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDateTimeVo));
            
            var instance = converter.ConvertFrom(inputString);
            Assert.IsType<NoJsonDateTimeVo>(instance);
            Assert.Equal(NoJsonDateTimeVo.Item3, instance);

            var reconverted = converter.ConvertTo(instance, inputString.GetType());
            Assert.Equal(inputString, reconverted);
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
                             .HasConversion(new EfCoreDateTimeVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDateTimeVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.DateTime)]
            [ValueConverter(ConverterType = typeof(LinqToDbDateTimeVo.LinqToDbValueConverter))]
            public LinqToDbDateTimeVo Id { get; set; }
        }
    }
}
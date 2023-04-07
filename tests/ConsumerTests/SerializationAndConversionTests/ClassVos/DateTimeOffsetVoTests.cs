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
// ReSharper disable RedundantCast
// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable SuspiciousTypeConversion.Global

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(DateTimeOffset))]
    public partial class DateTimeOffsetVo
    {
        static DateTimeOffsetVo()
        {
            Instance("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero).AddTicks(123));
            Instance("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
            Instance("SomethingElse", new DateTimeOffset(2022,01,15,19,08,49, TimeSpan.Zero).AddTicks(5413764));
        }
    }

    public class DateTimeOffsetVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            DateTimeOffsetVo.JanFirst.Equals(DateTimeOffsetVo.JanFirst).Should().BeTrue();
            (DateTimeOffsetVo.JanFirst == DateTimeOffsetVo.JanFirst).Should().BeTrue();

            (DateTimeOffsetVo.JanFirst != DateTimeOffsetVo.JanSecond).Should().BeTrue();
            (DateTimeOffsetVo.JanFirst == DateTimeOffsetVo.JanSecond).Should().BeFalse();

            DateTimeOffsetVo.JanFirst.Equals(DateTimeOffsetVo.JanFirst).Should().BeTrue();
            (DateTimeOffsetVo.JanFirst == DateTimeOffsetVo.JanFirst).Should().BeTrue();

            var original = DateTimeOffsetVo.JanFirst;
            var other = DateTimeOffsetVo.JanFirst;

            ((original as IEquatable<DateTimeOffsetVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DateTimeOffsetVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var g1 = NewtonsoftJsonDateTimeOffsetVo.JanFirst;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serialized, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonDateTimeOffsetVo.JanFirst;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedString = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonDateTimeOffsetVo.JanFirst.Value;
            var ie = NewtonsoftJsonDateTimeOffsetVo.JanFirst;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDateTimeOffsetVo>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = NewtonsoftJsonDateTimeOffsetVo.JanFirst.Value;
            var ie = SystemTextJsonDateTimeOffsetVo.JanFirst;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDateTimeOffsetVo>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var ie = BothJsonDateTimeOffsetVo.JanFirst;

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
            var ie = NoJsonDateTimeOffsetVo.JanFirst;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = """
                {"Value":"2019-01-01T14:15:16+00:00","Name":"JanFirst"}
                """;

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonDateTimeOffsetVo.JanFirst;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = "\"2019-01-01T14:15:16.0000000+00:00\"";
            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterDateTimeOffsetVo.JanFirst;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":"2019-01-01T14:15:16+00:00","Name":"JanFirst"}""";

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

            var original = new EfCoreTestEntity { Id = EfCoreDateTimeOffsetVo.JanFirst };
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

            IEnumerable<DapperDateTimeOffsetVo> results = await connection.QueryAsync<DapperDateTimeOffsetVo>("SELECT '2022-01-15 19:08:49.5413764'");

            DapperDateTimeOffsetVo actual = Assert.Single(results);

            var expected = DapperDateTimeOffsetVo.SomethingElse;
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDateTimeOffsetVo.JanFirst };
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
        public void TypeConverter_CanConvertToAndFrom(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDateTimeOffsetVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonDateTimeOffsetVo>(id);
            Assert.Equal(NoJsonDateTimeOffsetVo.SomethingElse, id);

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
                             .HasConversion(new EfCoreDateTimeOffsetVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDateTimeOffsetVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.DateTimeOffset)]
            [ValueConverter(ConverterType = typeof(LinqToDbDateTimeOffsetVo.LinqToDbValueConverter))]
            public LinqToDbDateTimeOffsetVo Id { get; set; }
        }
    }
}
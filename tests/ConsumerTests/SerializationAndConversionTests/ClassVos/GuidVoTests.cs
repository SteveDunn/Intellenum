﻿#nullable disable
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
    [Intellenum(underlyingType: typeof(Guid))]
    public partial class AnotherGuidVo
    {
        static AnotherGuidVo()
        {
            Member("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Member("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }

    }

    public class GuidVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            GuidEnum.Item1.Equals(GuidEnum.Item1).Should().BeTrue();
            (GuidEnum.Item1 == GuidEnum.Item1).Should().BeTrue();

            (GuidEnum.Item1 != GuidEnum.Item2).Should().BeTrue();
            (GuidEnum.Item1 == GuidEnum.Item2).Should().BeFalse();

            GuidEnum.Item1.Equals(GuidEnum.Item1).Should().BeTrue();
            (GuidEnum.Item1 == GuidEnum.Item1).Should().BeTrue();

            var original = GuidEnum.Item1;
            var other = GuidEnum.Item1;

            ((original as IEquatable<GuidEnum>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<GuidEnum>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var g1 = NewtonsoftJsonGuidEnum.Item1;

            string serializedGuid = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serializedGuid, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonGuidEnum.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedString = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonGuidEnum.Item1.Value;
            var ie = NewtonsoftJsonGuidEnum.Item1;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonGuidEnum>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = NewtonsoftJsonGuidEnum.Item1.Value;
            var ie = SystemTextJsonGuidEnum.Item1;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonGuidEnum>(serializedString);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var ie = BothJsonGuidEnum.Item1;

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
            var ie = NoJsonGuidEnum.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":\"" + ie.Value + "\",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonGuidEnum.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterGuidEnum.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = """{"Value":"00000000-0000-0000-0000-000000000001","Name":"Item1"}""";

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

            var original = new EfCoreTestEntity { Id = EfCoreGuidEnum.Item1 };
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

            IEnumerable<DapperGuidEnum> results = await connection.QueryAsync<DapperGuidEnum>("SELECT '00000000-0000-0000-0000-000000000001'");

            var value = Assert.Single(results);
            Assert.Equal(value, DapperGuidEnum.Item1);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity
            {
                Id = LinqToDbGuidEnum.Item1
            };
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
        public void TypeConverter_CanConvertToAndFrom()
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonGuidEnum));
            var id = converter.ConvertFrom("00000000-0000-0000-0000-000000000001");
            Assert.IsType<NoJsonGuidEnum>(id);
            Assert.Equal(NoJsonGuidEnum.Item1, id);

            var reconverted = converter.ConvertTo(id, typeof(string));
            Assert.Equal("00000000-0000-0000-0000-000000000001", reconverted);
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
                             .HasConversion(new EfCoreGuidEnum.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreGuidEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Guid)]
            [ValueConverter(ConverterType = typeof(LinqToDbGuidEnum.LinqToDbValueConverter))]
            public LinqToDbGuidEnum Id { get; set; }
        }
    }
}
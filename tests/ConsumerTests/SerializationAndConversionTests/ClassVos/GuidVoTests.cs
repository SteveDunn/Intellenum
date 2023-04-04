#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(Guid))]
    public partial class AnotherGuidVo
    {
        static AnotherGuidVo()
        {
            Instance("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
            Instance("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
        }

    }

    public class GuidVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            GuidVo.Item1.Equals(GuidVo.Item1).Should().BeTrue();
            (GuidVo.Item1 == GuidVo.Item1).Should().BeTrue();

            (GuidVo.Item1 != GuidVo.Item2).Should().BeTrue();
            (GuidVo.Item1 == GuidVo.Item2).Should().BeFalse();

            GuidVo.Item1.Equals(GuidVo.Item1).Should().BeTrue();
            (GuidVo.Item1 == GuidVo.Item1).Should().BeTrue();

            var original = GuidVo.Item1;
            var other = GuidVo.Item1;

            ((original as IEquatable<GuidVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<GuidVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var g1 = NewtonsoftJsonGuidVo.Item1;

            string serializedGuid = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serializedGuid, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonGuidVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedString = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonGuidVo.Item1.Value;
            var vo = NewtonsoftJsonGuidVo.Item1;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonGuidVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = NewtonsoftJsonGuidVo.Item1.Value;
            var vo = SystemTextJsonGuidVo.Item1;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonGuidVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var vo = BothJsonGuidVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedString2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedString1);
            Assert.Equal(serializedVo2, serializedString2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var vo = NoJsonGuidVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + vo.Value + "\",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonGuidVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterGuidVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + vo.Value + "\"}";

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

            var original = new EfCoreTestEntity { Id = EfCoreGuidVo.Item1 };
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

            IEnumerable<DapperGuidVo> results = await connection.QueryAsync<DapperGuidVo>("SELECT '00000000-0000-0000-0000-000000000001'");

            var value = Assert.Single(results);
            Assert.Equal(value, DapperGuidVo.Item1);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity
            {
                Id = LinqToDbGuidVo.Item1
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
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonGuidVo));
            var id = converter.ConvertFrom("00000000-0000-0000-0000-000000000001");
            Assert.IsType<NoJsonGuidVo>(id);
            Assert.Equal(NoJsonGuidVo.Item1, id);

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
                             .HasConversion(new EfCoreGuidVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreGuidVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Guid)]
            [ValueConverter(ConverterType = typeof(LinqToDbGuidVo.LinqToDbValueConverter))]
            public LinqToDbGuidVo Id { get; set; }
        }
    }
}
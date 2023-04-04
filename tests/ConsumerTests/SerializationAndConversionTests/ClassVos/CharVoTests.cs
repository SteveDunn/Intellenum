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
// ReSharper disable RedundantOverflowCheckingContext
// ReSharper disable ConvertToLocalFunction
// ReSharper disable EqualExpressionComparison

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
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
            CharVo.A.Equals(CharVo.A).Should().BeTrue();
            (CharVo.A == CharVo.A).Should().BeTrue();

            (CharVo.A != CharVo.B).Should().BeTrue();
            (CharVo.A == CharVo.B).Should().BeFalse();

            CharVo.A.Equals(CharVo.A).Should().BeTrue();
            (CharVo.A == CharVo.A).Should().BeTrue();

            var original = CharVo.A;
            var other = CharVo.A;

            ((original as IEquatable<CharVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<CharVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonCharVo.A;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedShort = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedShort);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonCharVo.A;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedShort = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(NewtonsoftJsonCharVo.A);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonCharVo>(serializedShort);

            Assert.Equal(NewtonsoftJsonCharVo.A, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonCharVo.A;
            var serializedShort = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonCharVo>(serializedShort);

            Assert.Equal(SystemTextJsonCharVo.A, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var vo = BothJsonCharVo.A;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedShort1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedShort2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedShort1);
            Assert.Equal(serializedVo2, serializedShort2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var vo = NoJsonCharVo.A;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"a\",\"Name\":\"A\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonCharVo.A;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterCharVo.A;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

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

            var original = new EfCoreTestEntity { Id = EfCoreCharVo.A };
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

            IEnumerable<DapperCharVo> results = await connection.QueryAsync<DapperCharVo>("SELECT 'a'");

            var value = Assert.Single(results);
            Assert.Equal(DapperCharVo.A, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbCharVo.A };
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
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonCharVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonCharVo>(id);
            Assert.Equal(NoJsonCharVo.A, id);

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
                            .HasConversion(new EfCoreCharVo.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public EfCoreCharVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Char)]
            [ValueConverter(ConverterType = typeof(LinqToDbCharVo.LinqToDbValueConverter))]
            public LinqToDbCharVo Id { get; set; }
        }
    }
}
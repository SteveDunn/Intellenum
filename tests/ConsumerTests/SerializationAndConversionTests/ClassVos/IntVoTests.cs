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
    [Intellenum(underlyingType: typeof(int))]
    [Instance("Item1", 1)]
    [Instance("Item2", 2)]
    public partial class AnotherIntVo { }

    public class IntVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            IntVo.Item1.Equals(IntVo.Item1).Should().BeTrue();
            (IntVo.Item1 == IntVo.Item1).Should().BeTrue();

            (IntVo.Item1 != IntVo.Item2).Should().BeTrue();
            (IntVo.Item1 == IntVo.Item2).Should().BeFalse();

            IntVo.Item1.Equals(IntVo.Item1).Should().BeTrue();
            (IntVo.Item1 == IntVo.Item1).Should().BeTrue();

            var original = IntVo.Item1;
            var other = IntVo.Item1;

            ((original as IEquatable<IntVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<IntVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToInt_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonIntVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedInt = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedInt);
        }

        [Fact]
        public void CanSerializeToInt_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonIntVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedInt = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedInt).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromInt_WithNewtonsoftJsonProvider()
        {
            var value = 123;
            var vo = NewtonsoftJsonIntVo.Item1;
            var serializedInt = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonIntVo>(serializedInt);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromInt_WithSystemTextJsonProvider()
        {
            var value = 123;
            var vo = SystemTextJsonIntVo.Item1;
            var serializedInt = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonIntVo>(serializedInt);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromInt_WithSystemTextJsonProvider_treating_numbers_as_string()
        {
            var vo = SystemTextJsonIntVo_Treating_numbers_as_string.Item1;
            var serializedInt = SystemTextJsonSerializer.Serialize(vo);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonIntVo_Treating_numbers_as_string>(serializedInt);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToInt_WithBothJsonConverters()
        {
            var vo = BothJsonIntVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedInt1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedInt2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedInt1);
            Assert.Equal(serializedVo2, serializedInt2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueProperty()
        {
            var vo = NoJsonIntVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + "}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonIntVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoTypeConverter_SerializesWithValueProperty()
        {
            var vo = NoConverterIntVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + "}";

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

            var original = new EfCoreTestEntity { Id = EfCoreIntVo.Item1 };
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

            IEnumerable<DapperIntVo> results = await connection.QueryAsync<DapperIntVo>("SELECT 123");

            var value = Assert.Single(results);
            Assert.Equal(DapperIntVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbIntVo.Item1 };
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
        [InlineData(123)]
        [InlineData("123")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonIntVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonIntVo>(id);
            Assert.Equal(NoJsonIntVo.Item1, id);

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
                             .HasConversion(new EfCoreIntVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreIntVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Int32)]
            [ValueConverter(ConverterType = typeof(LinqToDbIntVo.LinqToDbValueConverter))]
            public LinqToDbIntVo Id { get; set; }
        }
    }
}
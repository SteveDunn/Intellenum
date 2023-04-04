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

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(byte))]
    [Instance("Eighteen", 18)]    
    [Instance("Nineteen", 19)]    
    public partial class AnotherByteVo { }

    public class ByteVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            AnotherByteVo.Eighteen.Equals(AnotherByteVo.Eighteen).Should().BeTrue();
            (AnotherByteVo.Eighteen == AnotherByteVo.Eighteen).Should().BeTrue();

            (AnotherByteVo.Eighteen != AnotherByteVo.Nineteen).Should().BeTrue();
            (AnotherByteVo.Eighteen == AnotherByteVo.Nineteen).Should().BeFalse();

            AnotherByteVo.Eighteen.Equals(AnotherByteVo.Eighteen).Should().BeTrue();
            (AnotherByteVo.Eighteen == AnotherByteVo.Eighteen).Should().BeTrue();

            var original = AnotherByteVo.Eighteen;
            var other = AnotherByteVo.Eighteen;

            ((original as IEquatable<ByteVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<ByteVo>).Equals(original)).Should().BeTrue();
        }


        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonByteVo.Item1;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedShort = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedShort);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonByteVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedShort = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            byte value = 1;
            var vo = NewtonsoftJsonByteVo.Item1;
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonByteVo>(serializedShort);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            byte value = 1;
            var vo = SystemTextJsonByteVo.Item1;
            var serializedShort = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonByteVo>(serializedShort);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var vo = BothJsonByteVo.Item1;

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
            var vo = NoJsonByteVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + ",\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonByteVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterByteVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":" + vo.Value + ",\"Name\":\"Item1\"}";

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

            var original = new EfCoreTestEntity { Id = EfCoreByteVo.Item1 };
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

            IEnumerable<DapperByteVo> results = await connection.QueryAsync<DapperByteVo>("SELECT 1");

            var value = Assert.Single(results);
            Assert.Equal(DapperByteVo.Item1, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbByteVo.Item1 };
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
        [InlineData((byte) 1)]
        [InlineData("1")]
        public void TypeConverter_CanConvertToAndFrom(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonByteVo));
            var id = converter.ConvertFrom(value);
            Assert.IsType<NoJsonByteVo>(id);
            Assert.Equal(NoJsonByteVo.Item1, id);

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
                            .HasConversion(new EfCoreByteVo.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public EfCoreByteVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Byte)]
            [ValueConverter(ConverterType = typeof(LinqToDbByteVo.LinqToDbValueConverter))]
            public LinqToDbByteVo Id { get; set; }
        }
    }
}
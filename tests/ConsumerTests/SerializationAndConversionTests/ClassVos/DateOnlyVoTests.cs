#if NET6_0_OR_GREATER

#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
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
    [Intellenum(underlyingType: typeof(DateOnly))]
    public partial class AnotherDateOnlyVo {
        static AnotherDateOnlyVo()
        {
            Instance("JanFirst", new DateOnly(2021, 1, 1));
            Instance("JanSecond", new DateOnly(2021, 1, 2));
        }

 }

    public class DateOnlyVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            DateOnlyVo.JanFirst.Equals(DateOnlyVo.JanFirst).Should().BeTrue();
            (DateOnlyVo.JanFirst == DateOnlyVo.JanFirst).Should().BeTrue();

            (DateOnlyVo.JanFirst != DateOnlyVo.JanSecond).Should().BeTrue();
            (DateOnlyVo.JanFirst == DateOnlyVo.JanSecond).Should().BeFalse();

            DateOnlyVo.JanFirst.Equals(DateOnlyVo.JanFirst).Should().BeTrue();
            (DateOnlyVo.JanFirst == DateOnlyVo.JanFirst).Should().BeTrue();

            var original = DateOnlyVo.JanFirst;
            var other = DateOnlyVo.JanFirst;

            ((original as IEquatable<DateOnlyVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<DateOnlyVo>).Equals(original)).Should().BeTrue();
        }

#if NET7_0_OR_GREATER
        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var g1 = NewtonsoftJsonDateOnlyVo.JanFirst;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serialized, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonDateOnlyVo.JanFirst;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedString = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonDateOnlyVo.JanFirst.Value;
            var vo = NewtonsoftJsonDateOnlyVo.JanFirst;
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonDateOnlyVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }
        
        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonDateOnlyVo.JanFirst;
            var serializedString = SystemTextJsonSerializer.Serialize(NewtonsoftJsonDateOnlyVo.JanFirst.Value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonDateOnlyVo>(serializedString);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var vo = BothJsonDateOnlyVo.JanFirst;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(vo);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(vo);
            var serializedString2 = SystemTextJsonSerializer.Serialize(vo.Value);

            Assert.Equal(serializedVo1, serializedString1);
            Assert.Equal(serializedVo2, serializedString2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueProperty()
        {
            var vo = NoJsonDateOnlyVo.JanFirst;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + NewtonsoftJsonDateOnlyVo.JanFirst.Value.ToString("O") + "\"}";

            serialized.Should().Be(expected);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonDateOnlyVo.JanFirst;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{NewtonsoftJsonDateOnlyVo.JanFirst.Value:o}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoTypeConverter_SerializesWithValueProperty()
        {
            var vo = NoConverterDateOnlyVo.JanFirst;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":\"" + NewtonsoftJsonDateOnlyVo.JanFirst.Value.ToString("O") + "\"}";

            newtonsoft.Should().Be(expected);
            systemText.Should().Be(expected);
        }
#endif
        
        [Fact]
        public void WhenEfCoreValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            var original = new EfCoreTestEntity { Id = EfCoreDateOnlyVo.JanFirst };
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

            IEnumerable<DapperDateOnlyVo> results = await connection.QueryAsync<DapperDateOnlyVo>("SELECT '2021-01-01'");

            DapperDateOnlyVo actual = Assert.Single(results);

            var expected = DapperDateOnlyVo.JanFirst;
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbDateOnlyVo.JanFirst };
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
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonDateOnlyVo));
            var id = converter.ConvertFrom("2021-01-01");
            Assert.IsType<NoJsonDateOnlyVo>(id);
            Assert.Equal(NoJsonDateOnlyVo.JanFirst, id);

            var reconverted = converter.ConvertTo(id, typeof(string));
            Assert.Equal("2021-01-01", reconverted);
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
                             .HasConversion(new EfCoreDateOnlyVo.EfCoreValueConverter())
                             .ValueGeneratedNever();
                     });
             }
        }

        public class EfCoreTestEntity
        {
            public EfCoreDateOnlyVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Date)]
            [ValueConverter(ConverterType = typeof(LinqToDbDateOnlyVo.LinqToDbValueConverter))]
            public LinqToDbDateOnlyVo Id { get; set; }
        }
    }
}

#endif
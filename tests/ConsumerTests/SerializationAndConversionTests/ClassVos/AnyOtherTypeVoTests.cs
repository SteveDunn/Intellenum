#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(Bar))]
    public partial class FooVo
    {
        public static readonly FooVo Fred = new FooVo("Item1", new Bar(42, "Fred"));
        public static readonly FooVo Wilma = new FooVo("Item2", new Bar(52, "Wilma"));
    }

    public class AnyOtherTypeVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            FooVo.Fred.Equals(FooVo.Fred).Should().BeTrue();
            (FooVo.Fred == FooVo.Fred).Should().BeTrue();

            (FooVo.Fred != FooVo.Wilma).Should().BeTrue();
            (FooVo.Fred == FooVo.Wilma).Should().BeFalse();

            FooVo.Fred.Equals(FooVo.Fred).Should().BeTrue();
            (FooVo.Fred == FooVo.Fred).Should().BeTrue();

            var original = FooVo.Fred;
            var other = FooVo.Fred;

            ((original as IEquatable<FooVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<FooVo>).Equals(original)).Should().BeTrue();
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            NewtonsoftJsonFooVo g1 = NewtonsoftJsonFooVo.Item1;

            string serialized = NewtonsoftJsonSerializer.SerializeObject(g1);
            string serializedString = NewtonsoftJsonSerializer.SerializeObject(g1.Value);

            Assert.Equal(serialized, serializedString);
        }


        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonFooVo.Item1;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedString = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedString).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = NewtonsoftJsonFooVo.Item1;

            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonFooVo>(serializedString);

            Assert.Equal(NewtonsoftJsonFooVo.Item1, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = SystemTextJsonFooVo.Item1;
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonFooVo>(serializedString);

            Assert.Equal(SystemTextJsonFooVo.Item1, deserializedVo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var ie = BothJsonFooVo.Item1;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedString2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedString1);
            Assert.Equal(serializedVo2, serializedString2);
        }

        [Fact]
        public void CanSerializeToStringClass_WithBothJsonConverters()
        {
            var ie = BothJsonFooVoClass.Item1;

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
            var ie = NoJsonFooVo.Item1;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = """
                {"Value":{"Age":42,"Name":"Fred"},"Name":"Item1"}
                """;

            Assert.Equal(expected, serialized);
        }

        /// <summary>
        /// There is no way for newtonsoft, via a type converter, to convert
        /// the underlying non-native type to json.
        /// </summary>
        [Fact]
        public void WithTypeConverterButNoJsonConverters_NewtonsoftSerializesWithValueProperty()
        {
            NoJsonFooVo foo = NoJsonFooVo.Item1;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(foo);

            var expected = "{\"Value\":{\"Age\":42,\"Name\":\"Fred\"},\"Name\":\"Item1\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WithTypeConverterButNoJsonConverters_SystemTextJsonSerializesWithValueProperty()
        {
            var ie = NoConverterFooVo.Item1;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":{\"Age\":1,\"Name\":\"One\"},\"Name\":\"Item1\"}";

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

            var original = new EfCoreTestEntity { FooField = EfCoreFooVo.Item1 };
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
                Assert.Equal(original.FooField, retrieved.FooField);
            }
        }

        [Fact]
        public async Task WhenDapperValueConverterUsesValueConverter()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            IEnumerable<DapperFooVo> results = await connection.QueryAsync<DapperFooVo>("SELECT '{\"Age\":42,\"Name\":\"Fred\"}'");

            var value = Assert.Single(results);
            Assert.Equal(value, DapperFooVo.Item1);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { FooField = LinqToDbFooVo.Item1 };
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
                Assert.Equal(original.FooField, retrieved.FooField);
            }
        }

        [Fact]
        public void TypeConverter_CanConvertToAndFrom()
        {
            var converter = TypeDescriptor.GetConverter(typeof(NoJsonFooVo));

            object ie = converter.ConvertFrom(NoJsonFooVo.Item1.Value);

            Assert.IsType<NoJsonFooVo>(ie);

            Assert.Equal(NoJsonFooVo.Item1, ie);

            object reconverted = converter.ConvertTo(ie, typeof(Bar));
            Assert.IsType<Bar>(reconverted);
            Assert.Equal(((NoJsonFooVo) ie).Value, reconverted);
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
                            .Property(x => x.FooField)
                            .HasConversion(new EfCoreFooVo.EfCoreValueConverter());
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public int Id { get; set; }

            public EfCoreFooVo FooField { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.VarChar)]
            [ValueConverter(ConverterType = typeof(LinqToDbFooVo.LinqToDbValueConverter))]
            public LinqToDbFooVo FooField { get; set; }
        }
    }
}
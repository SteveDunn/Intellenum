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
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using Intellenum.IntegrationTests.TestTypes.ClassVos;
// ReSharper disable RedundantOverflowCheckingContext
// ReSharper disable ConvertToLocalFunction

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace Intellenum.IntegrationTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(bool))]
    [Instance("No", false)]
    [Instance("Yes", true)]
    public partial class BoolVo { }

    public class BoolVoTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            BoolVo.Yes.Equals(BoolVo.Yes).Should().BeTrue();
            (BoolVo.No == BoolVo.No).Should().BeTrue();

            (BoolVo.Yes != BoolVo.No).Should().BeTrue();
            (BoolVo.Yes == BoolVo.No).Should().BeFalse();

            BoolVo.Yes.Equals(BoolVo.Yes).Should().BeTrue();
            (BoolVo.Yes == BoolVo.Yes).Should().BeTrue();

            var original = BoolVo.Yes;
            var other = BoolVo.Yes;

            ((original as IEquatable<BoolVo>).Equals(other)).Should().BeTrue();
            ((other as IEquatable<BoolVo>).Equals(original)).Should().BeTrue();
        }


        [Fact]
        public void CanSerializeToShort_WithNewtonsoftJsonProvider()
        {
            var vo = NewtonsoftJsonBoolVo.Yes;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(vo);
            string serializedBool = NewtonsoftJsonSerializer.SerializeObject(vo.Value);

            Assert.Equal(serializedVo, serializedBool);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var vo = SystemTextJsonBoolVo.Yes;

            string serializedVo = SystemTextJsonSerializer.Serialize(vo);
            string serializedShort = SystemTextJsonSerializer.Serialize(vo.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            bool value = true;
            var vo = NewtonsoftJsonBoolVo.Yes;
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonBoolVo>(serializedShort);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            bool value = true;
            var vo = SystemTextJsonBoolVo.Yes;
            var serializedShort = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonBoolVo>(serializedShort);

            Assert.Equal(vo, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var vo = BothJsonBoolVo.Yes;

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
            var vo = NoJsonBoolVo.Yes;

            var serialized = SystemTextJsonSerializer.Serialize(vo);

            var expected = "{\"Value\":true,\"Name\":\"Yes\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var vo = NoJsonBoolVo.Yes;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(vo);

            var expected = $"\"{(bool)vo.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var vo = NoConverterBoolVo.Yes;

            var newtonsoft = SystemTextJsonSerializer.Serialize(vo);
            var systemText = SystemTextJsonSerializer.Serialize(vo);

            var expected = """{"Value":true,"Name":"Yes"}""";

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

            var original = new EfCoreTestEntity { Id = EfCoreBoolVo.Yes };
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

            IEnumerable<DapperBoolVo> results = await connection.QueryAsync<DapperBoolVo>("SELECT true");

            var value = Assert.Single(results);
            Assert.Equal(DapperBoolVo.Yes, value);
        }

        [Fact]
        public void WhenLinqToDbValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var original = new LinqToDbTestEntity { Id = LinqToDbBoolVo.Yes };
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
        [InlineData("true",  "True")]
        [InlineData("True",  "True")]
        public void TypeConverter_CanConvertToAndFrom_strings_1(object input, string expectedString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolVo));
            
            object converted = converter.ConvertFrom(input);
            Assert.IsType<NoJsonBoolVo>(converted);
            Assert.Equal(NoJsonBoolVo.Yes, converted);

            object reconverted = converter.ConvertTo(converted, input.GetType());
            Assert.Equal(expectedString, reconverted);
        }

        [Theory]
        [InlineData("false", "False")]
        [InlineData("False", "False")]
        public void TypeConverter_CanConvertToAndFrom_strings_2(object input, string expectedString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolVo));
            
            object converted = converter.ConvertFrom(input);
            Assert.IsType<NoJsonBoolVo>(converted);
            Assert.Equal(NoJsonBoolVo.No, converted);

            object reconverted = converter.ConvertTo(converted, input.GetType());
            Assert.Equal(expectedString, reconverted);
        }

        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void TypeConverter_CanConvertToAndFrom_bools_and_the_string_output_conversion_is_the_string_representation_of_the_VALUE_as_opposed_to_ToString_which_is_the_name(bool input, string expectedString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolVo));

            NoJsonBoolVo v = NoJsonBoolVo.FromValue(input);
            
            object asString = converter.ConvertTo(v, typeof(string));
            Assert.Equal(expectedString, asString);
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
                            .HasConversion(new EfCoreBoolVo.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public EfCoreBoolVo Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Boolean)]
            [ValueConverter(ConverterType = typeof(LinqToDbBoolVo.LinqToDbValueConverter))]
            public LinqToDbBoolVo Id { get; set; }
        }
    }
}
﻿#nullable disable
using System.ComponentModel;
using System.Threading.Tasks;
using ConsumerTests.TestEnums;
using Dapper;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable RedundantOverflowCheckingContext
// ReSharper disable ConvertToLocalFunction

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

namespace ConsumerTests.SerializationAndConversionTests.ClassVos
{
    [Intellenum(underlyingType: typeof(bool))]
    [Member("No", false)]
    [Member("Yes", true)]
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
            var ie = NewtonsoftJsonBoolEnum.Yes;

            string serializedVo = NewtonsoftJsonSerializer.SerializeObject(ie);
            string serializedBool = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            Assert.Equal(serializedVo, serializedBool);
        }

        [Fact]
        public void CanSerializeToShort_WithSystemTextJsonProvider()
        {
            var ie = SystemTextJsonBoolEnum.Yes;

            string serializedVo = SystemTextJsonSerializer.Serialize(ie);
            string serializedShort = SystemTextJsonSerializer.Serialize(ie.Value);

            serializedVo.Equals(serializedShort).Should().BeTrue();
        }

        [Fact]
        public void CanDeserializeFromShort_WithNewtonsoftJsonProvider()
        {
            bool value = true;
            var ie = NewtonsoftJsonBoolEnum.Yes;
            var serializedShort = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedVo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonBoolEnum>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanDeserializeFromShort_WithSystemTextJsonProvider()
        {
            bool value = true;
            var ie = SystemTextJsonBoolEnum.Yes;
            var serializedShort = SystemTextJsonSerializer.Serialize(value);

            var deserializedVo = SystemTextJsonSerializer.Deserialize<SystemTextJsonBoolEnum>(serializedShort);

            Assert.Equal(ie, deserializedVo);
        }

        [Fact]
        public void CanSerializeToShort_WithBothJsonConverters()
        {
            var ie = BothJsonBoolEnum.Yes;

            var serializedVo1 = NewtonsoftJsonSerializer.SerializeObject(ie);
            var serializedShort1 = NewtonsoftJsonSerializer.SerializeObject(ie.Value);

            var serializedVo2 = SystemTextJsonSerializer.Serialize(ie);
            var serializedShort2 = SystemTextJsonSerializer.Serialize(ie.Value);

            Assert.Equal(serializedVo1, serializedShort1);
            Assert.Equal(serializedVo2, serializedShort2);
        }

        [Fact]
        public void WhenNoJsonConverter_SystemTextJsonSerializesWithValueAndNameProperties()
        {
            var ie = NoJsonBoolEnum.Yes;

            var serialized = SystemTextJsonSerializer.Serialize(ie);

            var expected = "{\"Value\":true,\"Name\":\"Yes\"}";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_NewtonsoftSerializesWithoutValueProperty()
        {
            var ie = NoJsonBoolEnum.Yes;

            var serialized = NewtonsoftJsonSerializer.SerializeObject(ie);

            var expected = $"\"{(bool)ie.Value}\"";

            Assert.Equal(expected, serialized);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueAndNameProperties()
        {
            var ie = NoConverterBoolEnum.Yes;

            var newtonsoft = SystemTextJsonSerializer.Serialize(ie);
            var systemText = SystemTextJsonSerializer.Serialize(ie);

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

            var original = new EfCoreTestEntity { Id = EfCoreBoolEnum.Yes };
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

            var original = new LinqToDbTestEntity { Id = LinqToDbBoolEnum.Yes };
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
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolEnum));
            
            object converted = converter.ConvertFrom(input);
            Assert.IsType<NoJsonBoolEnum>(converted);
            Assert.Equal(NoJsonBoolEnum.Yes, converted);

            object reconverted = converter.ConvertTo(converted, input.GetType());
            Assert.Equal(expectedString, reconverted);
        }

        [Theory]
        [InlineData("false", "False")]
        [InlineData("False", "False")]
        public void TypeConverter_CanConvertToAndFrom_strings_2(object input, string expectedString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolEnum));
            
            object converted = converter.ConvertFrom(input);
            Assert.IsType<NoJsonBoolEnum>(converted);
            Assert.Equal(NoJsonBoolEnum.No, converted);

            object reconverted = converter.ConvertTo(converted, input.GetType());
            Assert.Equal(expectedString, reconverted);
        }

        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void TypeConverter_CanConvertToAndFrom_bools_and_the_string_output_conversion_is_the_string_representation_of_the_VALUE_as_opposed_to_ToString_which_is_the_name(bool input, string expectedString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(NoJsonBoolEnum));

            NoJsonBoolEnum v = NoJsonBoolEnum.FromValue(input);
            
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
                            .HasConversion(new EfCoreBoolEnum.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class EfCoreTestEntity
        {
            public EfCoreBoolEnum Id { get; set; }
        }

        public class LinqToDbTestEntity
        {
            [Column(DataType = DataType.Boolean)]
            [ValueConverter(ConverterType = typeof(LinqToDbBoolEnum.LinqToDbValueConverter))]
            public LinqToDbBoolEnum Id { get; set; }
        }
    }
}
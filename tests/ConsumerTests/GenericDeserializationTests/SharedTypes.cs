#if NET7_0_OR_GREATER

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Intellenum;

namespace ConsumerTests.GenericDeserializationTests;

#region Enums
[Intellenum<int>(Conversions.DapperTypeHandler | Conversions.EfCoreValueConverter | Conversions.LinqToDbValueConverter | Conversions.NewtonsoftJson | Conversions.SystemTextJson | Conversions.TypeConverter)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class MyIntEnum
{
}

[Intellenum<string>(Conversions.DapperTypeHandler | Conversions.EfCoreValueConverter | Conversions.LinqToDbValueConverter | Conversions.NewtonsoftJson | Conversions.SystemTextJson | Conversions.TypeConverter)]
[Member("Item1", "Item1!")]
[Member("Item2", "Item2!")]
public partial class MyStringEnum
{
}
#endregion

#region DBContext
public class DeserializationValidationDbContext : DbContext
{
    public DbSet<DeserializationValidationTestIntEntity>? IntEntities { get; set; }
    public DbSet<DeserializationValidationTestStringEntity>? StringEntities { get; set; }


    public DeserializationValidationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<DeserializationValidationTestIntEntity>(builder =>
            {
                builder
                    .Property(x => x.Id)
                    .HasConversion(new MyIntEnum.EfCoreValueConverter())
                    .ValueGeneratedNever();
            });
        modelBuilder
            .Entity<DeserializationValidationTestStringEntity>(builder =>
            {
                builder
                    .Property(x => x.Id)
                    .HasConversion(new MyStringEnum.EfCoreValueConverter())
                    .ValueGeneratedNever();
            });
    }
}

public class DeserializationValidationDataConnection : DataConnection
{
    // public ITable<DeserializationValidationTestIntEntity> IntEntities => GetTable<DeserializationValidationTestIntEntity>();
    // public ITable<DeserializationValidationTestStringEntity> StringEntities => GetTable<DeserializationValidationTestStringEntity>();

    public DeserializationValidationDataConnection(SqliteConnection connection)
        : base(
              SQLiteTools.GetDataProvider("SQLite.MS"),
              connection,
              disposeConnection: false)
    { }
}
#endregion

#region Entities
#region EF
public class DeserializationValidationTestIntEntity
{
    public MyIntEnum? Id { get; set; }
}

public class DeserializationValidationTestStringEntity
{
    public MyStringEnum? Id { get; set; }
}
#endregion

#region LinqToDB
public class DeserializationValidationTestLinqToDbTestIntEntity
{
    [Column(DataType = DataType.Int32)]
    [ValueConverter(ConverterType = typeof(MyIntEnum.LinqToDbValueConverter))]
    public MyIntEnum? Id { get; set; }
}

public class DeserializationValidationTestLinqToDbTestStringEntity
{
    [Column(DataType = DataType.VarChar)]
    [ValueConverter(ConverterType = typeof(MyStringEnum.LinqToDbValueConverter))]
    public MyStringEnum? Id { get; set; }
}
#endregion
#endregion
#endif
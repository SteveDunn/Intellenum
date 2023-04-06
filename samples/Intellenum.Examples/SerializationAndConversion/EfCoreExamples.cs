using System;
using System.Linq;
using System.Threading.Tasks;
using Intellenum.Examples.Types;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Intellenum.Examples.SerializationAndConversion
{
    public class EfCoreExamples : IScenario
    {
        public Task Run()
        {
            EfCoreValueConverterUsesValueConverter();
            return Task.CompletedTask;
        }

        private void EfCoreValueConverterUsesValueConverter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            var original = new TestEntity { Id = EfCoreStringEnum.Item1 };
            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Entities.Add(original);
                context.SaveChanges();
            }
            using (var context = new TestDbContext(options))
            {
                var all = context.Entities.ToList();
                var retrieved = all.Single();

                Console.WriteLine(retrieved);
            }
        }

        public class TestDbContext : DbContext
        {
            public DbSet<TestEntity> Entities { get; set; }

            public TestDbContext(DbContextOptions options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder
                    .Entity<TestEntity>(builder =>
                    {
                        builder
                            .Property(x => x.Id)
                            .HasConversion(new EfCoreStringEnum.EfCoreValueConverter())
                            .ValueGeneratedNever();
                    });
            }
        }

        public class TestEntity
        {
            public EfCoreStringEnum Id { get; set; }
        }
    }
}
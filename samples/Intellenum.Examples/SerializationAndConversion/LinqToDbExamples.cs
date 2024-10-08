﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Intellenum.Examples.Types;
using JetBrains.Annotations;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Microsoft.Data.Sqlite;

namespace Intellenum.Examples.SerializationAndConversion;

[UsedImplicitly]
public class LinqToDbExamples : IScenario
{
	public Task Run()
	{
		LinqToDbValueConverterUsesValueConverter();
		return Task.CompletedTask;
	}

	private static void LinqToDbValueConverterUsesValueConverter()
	{
		var connection = new SqliteConnection("DataSource=:memory:");
		connection.Open();

		var original = new TestEntity { Id = LinqToDbStringEnum.Item1};
		using (var context = new TestDbContext(connection))
		{
			context.CreateTable<TestEntity>();
			context.Insert(original);
		}
		using (var context = new TestDbContext(connection))
		{
			var all = context.Entities.ToList();
			var retrieved = all.Single();

			Console.WriteLine(retrieved);
		}
	}

	public class TestDbContext : DataConnection
	{
		public ITable<TestEntity> Entities => GetTable<TestEntity>();

		public TestDbContext(SqliteConnection connection)
			: base(
				SQLiteTools.GetDataProvider("SQLite.MS"),
				connection,
				disposeConnection: false)
		{ }
	}

	public class TestEntity
	{
		[PrimaryKey]
		[Column(DataType = DataType.VarChar)]
		[ValueConverter(ConverterType = typeof(LinqToDbStringEnum.LinqToDbValueConverter))]
		public LinqToDbStringEnum Id { get; set; }
	}
}
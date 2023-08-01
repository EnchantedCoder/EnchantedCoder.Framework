﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.DataSeeds;
using Havit.Data.Patterns.DataSeeds;
using Havit.Data.EntityFrameworkCore.Patterns.Tests.DataSeeds.Infrastructure;
using Havit.Data.EntityFrameworkCore.Patterns.DataSeeds.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Havit.Data.EntityFrameworkCore.Patterns.Tests.DataSeeds;

[TestClass]
public class DbDataSeedPersisterTests
{
	[TestMethod]
	public void DbDataSeedPersister_CheckConditions_EntityWithoutGeneratedKey()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			DataSeedConfiguration<ItemWithoutGeneratedKey> configuration = new DataSeedConfiguration<ItemWithoutGeneratedKey>(new ItemWithoutGeneratedKey[0]);
			configuration.PairByExpressions = new List<Expression<Func<ItemWithoutGeneratedKey, object>>> { item => item.Id };

			// Act
			persister.CheckConditions(configuration);

			// Assert
			// no exception was thrown
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForInserting_ItemWithDefaultValue()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act 
			var result = persister.GetPropertiesForInserting(FindEntityType<ItemWithDefaultValue>(dbContextFactory));

			// Assert
			Assert.AreEqual(3, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithDefaultValue.Symbol)));
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithDefaultValue.CommentWithDefaultValue)));
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithDefaultValue.Id))); // je volitelně generované sekvencí, budeme hodnotu nastavovat
		});
	}

	[TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
	public void DbDataSeedPersister_CheckConditions_EntityWithAutogeneratedKey()
	{
		// Arrange
		var dbContextFactory = GetDbContextFactories().First();
		var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

		DataSeedConfiguration<ItemWithAutogeneratedKey> configuration = new DataSeedConfiguration<ItemWithAutogeneratedKey>(new ItemWithAutogeneratedKey[0]);
		configuration.PairByExpressions = new List<Expression<Func<ItemWithAutogeneratedKey, object>>> { item => item.Id };

		// Act
		persister.CheckConditions(configuration);

		// Assert
		// exception was thrown
	}

	[TestMethod]
	public void DbDataSeedPersister_CheckConditions_EntityWithKeyGeneratedBySequence()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			DataSeedConfiguration<ItemWithKeyGeneratedBySequence> configuration = new DataSeedConfiguration<ItemWithKeyGeneratedBySequence>(new ItemWithKeyGeneratedBySequence[0]);
			configuration.PairByExpressions = new List<Expression<Func<ItemWithKeyGeneratedBySequence, object>>> { item => item.Id };

			// Act
			persister.CheckConditions(configuration);

			// Assert
			// no exception was thrown
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForInserting_EntityWithoutGeneratedKey()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForInserting(FindEntityType<ItemWithoutGeneratedKey>(dbContextFactory));

			// Assert
			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithoutGeneratedKey.Value)));
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithoutGeneratedKey.Id))); // není automaticky generované, budeme hodnotu nastavovat
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForInserting_EntityWithAutogeneratedKey()
	{
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			// Arrange
			IEntityType entityType;
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());
			using (var dbContext = dbContextFactory.CreateDbContext())
			{
				entityType = dbContext.Model.FindEntityType(typeof(ItemWithAutogeneratedKey));
			}

			// Act
			var result = persister.GetPropertiesForInserting(entityType);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithAutogeneratedKey.Symbol)));
			Assert.IsFalse(result.Any(item => item.Name == nameof(ItemWithAutogeneratedKey.Id)));
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForInserting_EntityWithKeyGeneratedBySequence()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForInserting(FindEntityType<ItemWithKeyGeneratedBySequence>(dbContextFactory));

			// Assert
			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithKeyGeneratedBySequence.Value)));
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithKeyGeneratedBySequence.Id))); // je volitelně generované sekvencí, budeme hodnotu nastavovat
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForUpdating_EntityWithoutGeneratedKey()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForUpdating<ItemWithoutGeneratedKey>(FindEntityType<ItemWithoutGeneratedKey>(dbContextFactory), null);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithoutGeneratedKey.Value)));
			Assert.IsFalse(result.Any(item => item.Name == nameof(ItemWithAutogeneratedKey.Id))); // primární klíč nelze aktualizovat
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForUpdating_EntityWithAutogeneratedKey()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForUpdating<ItemWithAutogeneratedKey>(FindEntityType<ItemWithAutogeneratedKey>(dbContextFactory), null);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithAutogeneratedKey.Symbol)));
			Assert.IsFalse(result.Any(item => item.Name == nameof(ItemWithAutogeneratedKey.Id))); // primární klíč nelze aktualizovat
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForUpdating_EntityWithKeyGeneratedBySequence()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForUpdating<ItemWithKeyGeneratedBySequence>(FindEntityType<ItemWithKeyGeneratedBySequence>(dbContextFactory), null);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(item => item.Name == nameof(ItemWithKeyGeneratedBySequence.Value)));
			Assert.IsFalse(result.Any(item => item.Name == nameof(ItemWithKeyGeneratedBySequence.Id))); // primární klíč nelze aktualizovat
		});
	}

	[TestMethod]
	public void DbDataSeedPersister_GetPropertiesForUpdating_DoesNotReturnExcludedProperties()
	{
		// Arrange
		GetDbContextFactories().ForEach(dbContextFactory =>
		{
			var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

			// Act
			var result = persister.GetPropertiesForUpdating<ItemWithAutogeneratedKey>(
				FindEntityType<ItemWithAutogeneratedKey>(dbContextFactory),
				new List<Expression<Func<ItemWithAutogeneratedKey, object>>> { item => item.Id, item => item.Symbol });

			// Assert
			Assert.AreEqual(0, result.Count);
		});
	}

	[TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
	public void DbDataSeedPersister_Save_ThrowsExceptionWhenSeedDataContainsDuplicate()
	{
		// Arrange
		var dbContextFactory = GetDbContextFactories().First();
		var persister = new DbDataSeedPersister(dbContextFactory, new DbDataSeedTransactionContext());

		DataSeedConfiguration<ItemWithoutGeneratedKey> configuration = new DataSeedConfiguration<ItemWithoutGeneratedKey>(new ItemWithoutGeneratedKey[] { new ItemWithoutGeneratedKey { Id = 1 }, new ItemWithoutGeneratedKey { Id = 1 } }); // duplicate
		configuration.PairByExpressions = new List<Expression<Func<ItemWithoutGeneratedKey, object>>> { item => item.Id };

		// Act
		persister.Save(configuration);

		// Assert
		// exception was thrown
	}

	private List<IDbContextFactory> GetDbContextFactories()
	{
		return new List<IDbContextFactory>
		{
			new LambdaDbContextFactory(() => new DbDataSeedPersisterTestsDbContext(new DbContextOptionsBuilder<DbDataSeedPersisterTestsDbContext>().UseSqlServer("FAKE").Options)),
			new LambdaDbContextFactory(() => new DbDataSeedPersisterTestsDbContext(new DbContextOptionsBuilder<DbDataSeedPersisterTestsDbContext>().UseInMemoryDatabase("FAKE").Options))
		};
	}

	private IEntityType FindEntityType<TEntity>(IDbContextFactory factory)
	{
		using (var dbContext = factory.CreateDbContext())
		{
			return dbContext.Model.FindEntityType(typeof(TEntity));
		}
	}

	internal class DbDataSeedPersisterTestsDbContext : DbContext
	{
		public DbDataSeedPersisterTestsDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void CustomizeModelCreating(ModelBuilder modelBuilder)
		{
			base.CustomizeModelCreating(modelBuilder);

			modelBuilder.HasSequence<int>("MySequence");

			modelBuilder.Entity<ItemWithDefaultValue>().Property(a => a.Id).ValueGeneratedNever();
			modelBuilder.Entity<ItemWithDefaultValue>().Property(a => a.CommentWithDefaultValue).HasDefaultValue("");
			modelBuilder.Entity<ItemWithAutogeneratedKey>();
			modelBuilder.Entity<ItemWithoutGeneratedKey>();
			modelBuilder.Entity<ItemWithKeyGeneratedBySequence>().Property(i => i.Id).HasDefaultValueSql("NEXT VALUE FOR MySequence");
		}
	}

	internal class ItemWithAutogeneratedKey
	{
		public int Id { get; set; }

		public string Symbol { get; set; }
	}

	internal class ItemWithoutGeneratedKey
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Value { get; set; }
	}

	internal class ItemWithKeyGeneratedBySequence
	{
		public int Id { get; set; }

		public string Value { get; set; }
	}

	internal class ItemWithDefaultValue
	{
		public int Id { get; set; }
		public string CommentWithDefaultValue { get; set; }
		public string Symbol { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.Attributes;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Conventions
{
	[TestClass]
	public class ConventionSuppressionsExtensionsTests
	{
		private const string TestCustomConventionIdentifier = nameof(TestCustomConventionIdentifier);

		[TestMethod]
		public void ConventionSuppressionsExtensionsTest_IsConventionSuppressed_ReturnsTrueForSuppressedConventions()
		{
			// Arrange
			DbContext dbContext = new TestDbContext();

			// Act in DbContext

			// Assert
			Assert.IsTrue(dbContext.Model.FindEntityType(typeof(EntityWithSuppression)).IsConventionSuppressed(TestCustomConventionIdentifier));
			Assert.IsTrue(dbContext.Model.FindEntityType(typeof(EntityWithSuppression)).FindProperty(nameof(EntityWithoutSuppression.Value)).IsConventionSuppressed(TestCustomConventionIdentifier));
		}

		[TestMethod]
		public void ConventionSuppressionsExtensionsTest_IsConventionSuppressed_ReturnsFalseForNotSuppressedConventions()
		{
			// Arrange
			DbContext dbContext = new TestDbContext();

			// Act in DbContext

			// Assert
			Assert.IsFalse(dbContext.Model.FindEntityType(typeof(EntityWithoutSuppression)).IsConventionSuppressed(TestCustomConventionIdentifier));
			Assert.IsFalse(dbContext.Model.FindEntityType(typeof(EntityWithoutSuppression)).FindProperty(nameof(EntityWithoutSuppression.Value)).IsConventionSuppressed(TestCustomConventionIdentifier));
		}

		public class TestDbContext : DbContext
		{
			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				base.OnConfiguring(optionsBuilder);
				optionsBuilder.UseInMemoryDatabase(nameof(TestDbContext));
			}

			protected override void CustomizeModelCreating(ModelBuilder modelBuilder)
			{
				base.CustomizeModelCreating(modelBuilder);

				// Act
				modelBuilder.Entity<EntityWithoutSuppression>();
				modelBuilder.Entity<EntityWithSuppression>();
			}
		}

		[SuppressConvention(TestCustomConventionIdentifier)]
		public class EntityWithSuppression
		{
			public int Id { get; set; }

			[SuppressConvention(TestCustomConventionIdentifier)]
			public string Value { get; set; }
		}

		public class EntityWithoutSuppression
		{
			public int Id { get; set; }
			public string Value { get; set; }
		}
	}
}
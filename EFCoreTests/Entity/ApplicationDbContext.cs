using System;
using EnchantedCoder.Data.EntityFrameworkCore;
using EnchantedCoder.EFCoreTests.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EnchantedCoder.EFCoreTests.Entity
{
	public class ApplicationDbContext : EnchantedCoder.Data.EntityFrameworkCore.DbContext
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
			// NOOP
		}

		/// <inheritdoc />
		protected override void CustomizeModelCreating(ModelBuilder modelBuilder)
		{
			base.CustomizeModelCreating(modelBuilder);

			modelBuilder.RegisterModelFromAssembly(typeof(EnchantedCoder.EFCoreTests.Model.Person).Assembly);
			modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
		}
	}
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnchantedCoder.Data.EntityFrameworkCore
{
	/// <inheritdoc />
	public class DbContextFactory<TDbContext> : IDbContextFactory
		where TDbContext : EnchantedCoder.Data.EntityFrameworkCore.DbContext
	{
		private readonly IDbContextFactory<TDbContext> dbContextFactory;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public DbContextFactory(IDbContextFactory<TDbContext> dbContextFactory)
		{
			this.dbContextFactory = dbContextFactory;
		}

		/// <inheritdoc />
		public IDbContext CreateDbContext() => dbContextFactory.CreateDbContext();
	}
}

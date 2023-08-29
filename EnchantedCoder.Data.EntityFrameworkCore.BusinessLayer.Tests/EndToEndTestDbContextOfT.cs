using System;
using Microsoft.EntityFrameworkCore;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Tests
{
	public class EndToEndTestDbContext<TEntity> : EndToEndTestDbContext
		where TEntity : class
	{
		public EndToEndTestDbContext(Action<ModelBuilder> onModelCreating = default)
			: base(onModelCreating)
		{ }

		public DbSet<TEntity> Entities { get; }
	}
}

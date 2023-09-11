using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using EnchantedCoder.Data.EntityFrameworkCore.Tests.Metadata.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Metadata.Infrastructure.Entity
{
	public class ApplicationEntityTestDbContext : DbContext
	{
		protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseInMemoryDatabase(nameof(ApplicationEntityTestDbContext));
		}

		protected override void CustomizeModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
		{
			base.CustomizeModelCreating(modelBuilder);

			modelBuilder.Entity<DefaultApplicationEntity>();
			modelBuilder.Entity<ExplicitApplicationEntity>().HasAnnotation(ApplicationEntityAnnotationConstants.IsApplicationEntityAnnotationName, true);
			modelBuilder.Entity<NotApplicationEntity>().HasAnnotation(ApplicationEntityAnnotationConstants.IsApplicationEntityAnnotationName, false);
		}
	}
}

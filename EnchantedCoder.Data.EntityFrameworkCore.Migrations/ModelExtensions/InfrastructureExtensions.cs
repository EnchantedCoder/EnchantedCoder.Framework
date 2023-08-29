﻿using System;
using EnchantedCoder.Data.EntityFrameworkCore.Migrations.Infrastructure.ModelExtensions;
using EnchantedCoder.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace EnchantedCoder.Data.EntityFrameworkCore.Migrations.ModelExtensions
{
	/// <summary>
	/// Extension metódy pre registráciu Extended Migrations infraštruktúrnych služieb používané podporou Model Extensions.
	/// </summary>
	public static class InfrastructureExtensions
	{
		/// <summary>
		/// Registruje Extended Migrations infraštruktúrne služby používané podporou pre Model Extensions.
		/// </summary>
		public static void UseExtendedMigrationsInfrastructure(this DbContextOptionsBuilder optionsBuilder)
		{
			Contract.Requires<ArgumentNullException>(optionsBuilder != null);

			IDbContextOptionsBuilderInfrastructure builder = optionsBuilder;

			// handle multiple invocations of UseExtendedMigrationsInfrastructure
			var compositeMigrationsAnnotationProviderExtension =
				optionsBuilder.Options.FindExtension<CompositeMigrationsAnnotationProviderExtension>()
				?? new CompositeMigrationsAnnotationProviderExtension().WithAnnotationProvider<SqlServerMigrationsAnnotationProvider>();
			builder.AddOrUpdateExtension(compositeMigrationsAnnotationProviderExtension);

			var compositeRelationalAnnotationProviderExtension =
				optionsBuilder.Options.FindExtension<CompositeRelationalAnnotationProviderExtension>()
				?? new CompositeRelationalAnnotationProviderExtension().WithAnnotationProvider<SqlServerAnnotationProvider>();
			builder.AddOrUpdateExtension(compositeRelationalAnnotationProviderExtension);

			var compositeMigrationsSqlGeneratorExtension =
				optionsBuilder.Options.FindExtension<CompositeMigrationsSqlGeneratorExtension>()
				?? new CompositeMigrationsSqlGeneratorExtension();
			builder.AddOrUpdateExtension(compositeMigrationsSqlGeneratorExtension);
		}
	}
}
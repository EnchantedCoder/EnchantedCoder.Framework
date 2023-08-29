﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Migrations.Tests
{
	public class MigrationsEndToEndTestDbContext : TestDbContext
	{
		private readonly Action<ModelBuilder> onModelCreating;

		public MigrationsEndToEndTestDbContext(Action<ModelBuilder> onModelCreating = default)
		{
			this.onModelCreating = onModelCreating;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			onModelCreating?.Invoke(modelBuilder);
		}

		public IReadOnlyList<MigrationOperation> Diff(DbContext target)
		{
			var differ = this.GetService<IMigrationsModelDiffer>();
			return differ.GetDifferences(this.GetService<IDesignTimeModel>().Model.GetRelationalModel(), target.GetService<IDesignTimeModel>().Model.GetRelationalModel());
		}

		public IReadOnlyList<MigrationCommand> Migrate(DbContext target)
		{
			var diff = Diff(target);
			var generator = this.GetService<IMigrationsSqlGenerator>();
			return generator.Generate(diff, this.Model);
		}
	}
}
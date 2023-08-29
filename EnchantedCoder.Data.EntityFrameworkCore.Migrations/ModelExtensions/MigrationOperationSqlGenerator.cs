﻿using EnchantedCoder.Data.EntityFrameworkCore.Migrations.Infrastructure.ModelExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Migrations.ModelExtensions
{
	/// <summary>
	/// Base implementation of <see cref="IMigrationOperationSqlGenerator"/> with virtual implementations.s
	/// </summary>
	public class MigrationOperationSqlGenerator : IMigrationOperationSqlGenerator
	{
		/// <inheritdoc />
		public virtual void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}

		/// <inheritdoc />
		public virtual void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}

		/// <inheritdoc />
		public virtual void Generate(AlterTableOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}

		/// <inheritdoc />
		public virtual void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}

		/// <inheritdoc />
		public virtual void Generate(AlterDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}

		/// <inheritdoc />
		public virtual void Generate(SqlServerCreateDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
		}
	}
}
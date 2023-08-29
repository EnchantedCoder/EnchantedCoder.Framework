﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using EnchantedCoder.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.Migrations.ModelExtensions
{
	/// <inheritdoc />
	public class ModelExtensionsAssembly : IModelExtensionsAssembly
	{
		private IReadOnlyCollection<TypeInfo> modelExtenders;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ModelExtensionsAssembly(
			ICurrentDbContext currentDbContext,
			IDbContextOptions dbContextOptions)
		{
			Contract.Requires<ArgumentNullException>(currentDbContext != null);
			Contract.Requires<ArgumentNullException>(dbContextOptions != null);

			Assembly = dbContextOptions.FindExtension<ModelExtensionsExtension>()?.ExtensionsAssembly ??
					   currentDbContext.Context.GetType().GetTypeInfo().Assembly;
		}

		/// <inheritdoc />
		public IReadOnlyCollection<TypeInfo> ModelExtenders
		{
			get
			{
				IReadOnlyCollection<TypeInfo> Create()
				{
					return Assembly != null ?
						Assembly.DefinedTypes.Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition && t.GetInterface(nameof(IModelExtender)) != null).ToImmutableArray() :
						ImmutableArray<TypeInfo>.Empty;
				}

				return modelExtenders ??= Create();
			}
		}

		/// <inheritdoc />
		public Assembly Assembly { get; }

		/// <inheritdoc />
		public IModelExtender CreateModelExtender(TypeInfo modelExtenderClass)
		{
			Contract.Requires<ArgumentNullException>(modelExtenderClass != null);

			return (IModelExtender)Activator.CreateInstance(modelExtenderClass.AsType());
		}
	}
}
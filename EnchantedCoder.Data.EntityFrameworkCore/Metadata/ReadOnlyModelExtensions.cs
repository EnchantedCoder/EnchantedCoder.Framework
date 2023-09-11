﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnchantedCoder.Linq;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnchantedCoder.Data.EntityFrameworkCore.Metadata
{
	/// <summary>
	/// Extension metody k IModel.
	/// </summary>
	public static class ModelExtension
	{
		/// <summary>
		/// Vrací entity, které nejsou systémové, nejsou Owned a nejsou QueryType.
		/// </summary>
		public static IEnumerable<IReadOnlyEntityType> GetApplicationEntityTypes(this IReadOnlyModel model, bool includeManyToManyEntities = true)
		{
			return model.GetEntityTypes()
				.Where(entityType => entityType.IsApplicationEntity())
				.WhereIf(!includeManyToManyEntities, entityType => !entityType.IsManyToManyEntity());
		}
	}
}

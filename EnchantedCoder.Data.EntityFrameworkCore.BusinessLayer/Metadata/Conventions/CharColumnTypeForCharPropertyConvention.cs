﻿using System.Linq;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Metadata.Conventions
{
	/// <summary>
	/// Konvencia pre nastavenie typu stĺpca char(1), resp. nchar(1) pre property s typom Char (EF Core štandardne použije nvarchar(1)).
	/// </summary>
	public class CharColumnTypeForCharPropertyConvention : IPropertyAddedConvention
	{
		public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
		{
			// Systémové tabulky nechceme změnit.
			if (propertyBuilder.Metadata.DeclaringEntityType.IsSystemType())
			{
				return;
			}

			if (propertyBuilder.Metadata.DeclaringEntityType.IsConventionSuppressed(ConventionIdentifiers.CharColumnTypeForCharPropertyConvention) || propertyBuilder.Metadata.IsConventionSuppressed(ConventionIdentifiers.CharColumnTypeForCharPropertyConvention))
			{
				return;
			}

			if (propertyBuilder.Metadata.ClrType == typeof(char))
			{
				propertyBuilder.HasColumnType("nchar(1)", fromDataAnnotation: false /* Convention */);
			}
		}
	}
}
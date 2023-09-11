﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Metadata.Conventions
{
	/// <summary>
	/// Konvence nastavuje hodnotu z atributu <see cref="DefaultValueAttribute"/> jako DefaultValueSql, pokud dosud žádná výchozí hodnota nebyla nastavena.
	/// </summary>
	public class DefaultValueAttributeConvention : PropertyAttributeConventionBase<DefaultValueAttribute>
	{
		public DefaultValueAttributeConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
		{
		}

		protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, DefaultValueAttribute attribute, MemberInfo clrMember, IConventionContext context)
		{
			// Systémové tabulky - nemá cenu řešit, nebudou mít attribut.
			// Podpora pro suppress - nemá význam, stačí nepoužít attribut.

			propertyBuilder.HasDefaultValue(attribute.Value, fromDataAnnotation: true /* DataAnnotation */);
			propertyBuilder.ValueGenerated(Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never, fromDataAnnotation: true /* DataAnnotation */); // https://stackoverflow.com/questions/40655968/how-to-force-default-values-in-an-insert-with-entityframework-core
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Metadata.Conventions
{
	/// <summary>
	/// Registruje PrefixedTablePrimaryKeysConvention do ConventionSetu.
	/// </summary>
	internal class PrefixedTablePrimaryKeysConventionPlugin : IConventionSetPlugin
	{
		public ConventionSet ModifyConventions(ConventionSet conventionSet)
		{
			conventionSet.KeyAddedConventions.Add(new PrefixedTablePrimaryKeysConvention());
			return conventionSet;
		}
	}
}

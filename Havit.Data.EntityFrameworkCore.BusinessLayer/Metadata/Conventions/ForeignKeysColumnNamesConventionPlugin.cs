﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Data.EntityFrameworkCore.BusinessLayer.Metadata.Conventions
{
	/// <summary>
	/// Registruje ForeignKeysColumnNamesConvention do ConventionSetu.
	/// </summary>
	internal class ForeignKeysColumnNamesConventionPlugin : IConventionSetPlugin
	{
		public ConventionSet ModifyConventions(ConventionSet conventionSet)
		{			
			conventionSet.ForeignKeyAddedConventions.Add(new ForeignKeysColumnNamesConvention());
			return conventionSet;
		}
	}
}

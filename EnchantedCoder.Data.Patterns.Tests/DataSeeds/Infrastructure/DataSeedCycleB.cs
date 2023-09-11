using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.Patterns.DataSeeds;
using EnchantedCoder.Data.Patterns.DataSeeds.Profiles;

namespace EnchantedCoder.Data.Patterns.Tests.DataSeeds.Infrastructure
{
	internal class DataSeedCycleB : DataSeed<DefaultProfile>
	{
		public override void SeedData()
		{
			// NOOP
		}

		public override IEnumerable<Type> GetPrerequisiteDataSeeds()
		{
			yield return typeof(DataSeedCycleB);
		}
	}
}

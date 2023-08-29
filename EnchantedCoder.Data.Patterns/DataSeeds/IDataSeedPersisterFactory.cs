using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Data.Patterns.DataSeeds
{
	/// <summary>
	/// Factory pro IDataSeedPersister.
	/// </summary>
	public interface IDataSeedPersisterFactory
	{
		/// <summary>
		/// Vytváří/vrací IDataSeedPersister.
		/// </summary>
		IDataSeedPersister CreateService();

		/// <summary>
		/// Uvolňuje službu.
		/// </summary>
		void ReleaseService(IDataSeedPersister service);
	}
}

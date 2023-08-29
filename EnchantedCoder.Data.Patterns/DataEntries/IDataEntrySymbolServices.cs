using System;
using EnchantedCoder.Data.Patterns.Exceptions;

namespace EnchantedCoder.Data.Patterns.DataEntries
{
	/// <summary>
	/// Zajišťuje mapování párovacích symbolů a identifikátorů objektů, resp. získání identifikátoru (primárního klíče) na základě symbolu.
	/// </summary>
	public interface IDataEntrySymbolService<TEntity>
	{
		/// <summary>
		/// Vrací hodnotu identifikátoru (primárního klíče) na základě symbolu.
		/// </summary>
		/// <param name="entry">"Symbol".</param>
		/// <exception cref="EnchantedCoder.Data.Patterns.Exceptions.ObjectNotFoundException">Pokud není identifikátor dle symbolu nalezen.</exception>
		int GetEntryId(Enum entry);
	}
}

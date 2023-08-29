using System.Collections.Generic;

namespace EnchantedCoder.Data.Patterns.Localizations.Internal
{
	/// <summary>
	/// Úložiště párování culture na jazyk.
	/// </summary>
	public interface ILanguageByCultureStorage
	{
		/// <summary>
		/// Párování culture na jazyk.
		/// </summary>
		Dictionary<string, int> Value { get; set; }
	}
}

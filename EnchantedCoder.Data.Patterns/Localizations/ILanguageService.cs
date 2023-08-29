using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Localizations
{
	/// <summary>
	/// Služba vrací dohledání jazyka dle culture.
	/// Jazykem se rozumí instance třídy modelu (implementující ILanguage).
	/// </summary>
	public interface ILanguageService
	{
		/// <summary>
		/// Vrací jazyk pro danou culture.
		/// </summary>
		ILanguage GetLanguage(string cultureName);

		/// <summary>
		/// Vrací výchozí jazyk.
		/// </summary>
		ILanguage GetDefaultLanguage();

	}
}
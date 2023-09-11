using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.Patterns.DataEntries;
using EnchantedCoder.Data.Patterns.Infrastructure;
using EnchantedCoder.Data.Patterns.Localizations.Internal;
using EnchantedCoder.Data.Patterns.Repositories;
using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Localizations
{
	/// <summary>
	/// Služba vrací aktuální jazyk nebo jazyk dle culture.
	/// Jazykem se rozumí instance třídy modelu (implementující <see cref="ILanguage"/>).
	/// </summary>
	public class LanguageService<TLanguage> : ILanguageService
		where TLanguage : class, ILanguage
	{
		private readonly IRepository<TLanguage> languageRepository;
		private readonly ILanguageByCultureService languageByCultureService;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public LanguageService(IRepository<TLanguage> languageRepository, ILanguageByCultureService languageByCultureService)
		{
			this.languageRepository = languageRepository;
			this.languageByCultureService = languageByCultureService;
		}

		/// <summary>
		/// Vrací výchozí jazyk (vyhledáním pro prázdnou cultureName).
		/// </summary>
		public virtual ILanguage GetDefaultLanguage()
		{
			return GetLanguage("");
		}

		/// <summary>
		/// Vrací jazyk pro danou culture.
		/// </summary>
		public ILanguage GetLanguage(string cultureName)
		{
			int languageId = languageByCultureService.GetLanguageId(cultureName);
			return languageRepository.GetObject(languageId);
		}
	}
}

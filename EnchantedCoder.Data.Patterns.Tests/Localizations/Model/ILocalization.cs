using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Tests.Localizations.Model
{
	public interface ILocalization<TLocalizedEntity> : ILocalization<TLocalizedEntity, Language>
	{
		new TLocalizedEntity Parent { get; set; }
		int ParentId { get; set; }

		new Language Language { get; set; }
		int LanguageId { get; set; }
	}
}

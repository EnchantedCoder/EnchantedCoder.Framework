using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Tests.Localizations.Model
{
	public interface ILocalized<TLocalizationEntity> : ILocalized<TLocalizationEntity, Language>
	{
		new List<TLocalizationEntity> Localizations { get; set; }
	}
}

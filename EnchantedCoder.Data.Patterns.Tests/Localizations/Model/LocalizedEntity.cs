using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Tests.Localizations.Model
{
    public class LocalizedEntity : ILocalized<LocalizedEntityLocalization>
	{
		public int Id { get; set; }

		public List<LocalizedEntityLocalization> Localizations { get; set; }
	}
}

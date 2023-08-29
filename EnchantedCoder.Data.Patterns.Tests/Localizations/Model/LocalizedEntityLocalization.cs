using System.Runtime.InteropServices;
using EnchantedCoder.Model.Localizations;

namespace EnchantedCoder.Data.Patterns.Tests.Localizations.Model
{
	public class LocalizedEntityLocalization : ILocalization<LocalizedEntity>
	{
		public int Id { get; set; }

		public LocalizedEntity Parent { get; set; }
		public int ParentId { get; set; }

		public Language Language { get; set; }
		public int LanguageId { get; set; }
	}
}
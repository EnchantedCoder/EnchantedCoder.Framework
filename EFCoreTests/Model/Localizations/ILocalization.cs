﻿namespace Havit.EFCoreTests.Model.Localizations
{
	/// <summary>
	/// Lokalizace.
	/// </summary>
	public interface ILocalization<TLocalizedEntity> : Havit.Model.Localizations.ILocalization<TLocalizedEntity, Language>
	{
		new TLocalizedEntity Parent { get; set; } // new - Havit.Model.Localizations.ILocalization<,> již má vlastnost Parent
		int ParentId { get; set; }

		new Language Language { get; set; } // new - Havit.Model.Localizations.ILocalization<,> již má vlastnost Language
		int LanguageId { get; set; }

	}
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Havit.Business
{
	/// <summary>
	/// Rozhran� ozna�uj�c� lokalizovan� objekt.
	/// </summary>
	public interface ILocalizable
	{
		/// <summary>
		/// Vytvo�� polo�ku lokalizace pro dan� jazyk.
		/// </summary>
		BusinessObjectBase CreateLocalization(ILanguage language);

		/// <summary>
		/// Lokalizace.
		/// </summary>
		ILocalizationCollection Localizations { get; }
	}
}

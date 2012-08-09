using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Havit.Business
{
	/// <summary>
	/// Rozhran� kolekce lokaliza�n�ch business objekt�.
	/// </summary>
	public interface ILocalizationCollection: ICollection
	{
		/// <summary>
		/// Vr�t� business objekt pro aktu�ln� jazyk.
		/// </summary>
		BusinessObjectBase Current { get;}

		/// <summary>
		/// Vr�t� business objekt pro zadan� jazyk.
		/// </summary>
		BusinessObjectBase this[ILanguage language] { get; }
	}
}

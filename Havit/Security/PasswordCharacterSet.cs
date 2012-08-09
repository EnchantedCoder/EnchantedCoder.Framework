using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Security
{
	/// <summary>
	/// Sada znak�, z n� <see cref="PasswordGenerator"/> vyb�r� znaky pro generov�n� hesla.
	/// </summary>
	public enum PasswordCharacterSet
	{
		/// <summary>
		/// Pouze mal� p�smena.
		/// </summary>
		LowerCaseLetters,

		/// <summary>
		/// Velk� a mal� p�smena.
		/// </summary>
		Letters,

		/// <summary>
		/// P�smena (velk� i mal�) a ��slice.
		/// </summary>
		LettersAndDigits,

		/// <summary>
		/// P�smena (velk� i mal�), ��slice a speci�ln� znaky.
		/// </summary>
		LettersDigitsAndSpecialCharacters
	}
}

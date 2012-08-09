using System;
using System.Text;
using System.Globalization;

namespace Havit
{
	/// <summary>
	/// Roz�i�uj�c� funkce pro pr�ci s textov�mi �et�zci <see cref="System.String"/>.
	/// T��da poskytuje statick� metody a konstanty, je neinstan�n�.
	/// </summary>
	public static class StringExt
	{
		#region Left, Right
		/// <summary>
		/// Returns a string containing a specified number of characters from the left side of a string.
		/// </summary>
		/// <param name="str">String expression from which the leftmost characters are returned.</param>
		/// <param name="length">Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in Str, the entire string is returned.</param>
		/// <returns>string containing a specified number of characters from the left side of a string</returns>
		public static string Left(string str, int length)
		{
			if (length < 0)
			{
				throw new ArgumentException("Argument length nesm� b�t men�� ne� 0.", "length");
			}
			if ((length == 0) || (str == null))
			{
				return String.Empty;
			}
			if (length >= str.Length)
			{
				return str;
			}
			return str.Substring(0, length);
		}

		/// <summary>
		/// Returns a string containing a specified number of characters from the right side of a string.
		/// </summary>
		/// <param name="str">String expression from which the rightmost characters are returned.</param>
		/// <param name="length">Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in <c>str</c>, the entire string is returned.</param>
		/// <returns>string containing a specified number of characters from the right side of a string</returns>
		public static string Right(string str, int length)
		{
			if (length < 0)
			{
				throw new ArgumentException("Argument length nesm� b�t men�� ne� 0.", "length");
			}
			if ((length == 0) || (str == null))
			{
				return String.Empty;
			}
			int strLength = str.Length;
			if (length >= strLength)
			{
				return str;
			}
			return str.Substring(strLength - length, length);
		}
		#endregion

		#region OdeberDiakritiku
		/// <summary>
		/// Odebere diakritiku z textu, tj. p�evede na text bez diakritiky.
		/// </summary>
		/// <remarks>Odeb�r� ve�kerou diakritiku v�ech n�rodn�ch znak� obecn�.</remarks>
		/// <param name="text">Text, kter�mu m� b�t diakritika odebr�na.</param>
		/// <returns>text bez diakritiky</returns>
		public static string OdeberDiakritiku(string text)
		{
			StringBuilder sb = new StringBuilder();

			text = text.Normalize(NormalizationForm.FormD);

			for (int i = 0; i < text.Length; i++)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(text[i]) != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(text[i]);
				}
			}

			return sb.ToString();

			/*
			 * p�vodn� implementace pro .NET Framework 1.1
			const string s		= "�����������Ӛ����؞������ڝ�������iI������";
			const string bez	= "aAeEiIyYeEoOsScCrRzZnNuUuUtTdDaAeEiIoOuUlL";
  
			for (int i=0; i <= s.Length-1; i++)
			{
				text = text.Replace(s[i], bez[i]);
			}
			return text;
			*/
		}
		#endregion

		#region IntToHex
		/// <summary>
		/// Vr�t� char-reprezentaci (0..9, A..F) �estn�ctkov� ��slice (0-15).
		/// </summary>
		/// <remarks>Z d�vodu rychlosti neprov�d� kontrolu rozsahu a p�evede nap�. i ��slici 16 jako G.</remarks>
		/// <param name="cislice">��slice (0..15)</param>
		/// <returns>char-reprezentace (0..9, A..F) �estn�ctkov� ��slice (0-15).</returns>
		public static char IntToHex(int cislice)
		{
			if (cislice <= 9)
			{
				return (char) ((ushort) (cislice + 0x30));
			}
			return (char) ((ushort) ((cislice - 10) + 0x61));
		}
		#endregion
	}
}

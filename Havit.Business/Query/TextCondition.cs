using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky testuj�c� textov� �et�zec.
	/// </summary>
	public static class TextCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� �et�zec na rovnost. Citlivost na velk� a mal� p�smena, diakritiku apod. vych�z� z nastaven� serveru.
		/// </summary>
		public static Condition CreateEquals(PropertyInfo property, string value)
		{
			return new BinaryCondition(BinaryCondition.EqualsPattern, property, ValueOperand.Create(value));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� �et�zec na podobnost oper�torem LIKE. Citlivost na velk� a mal� p�smena, diakritiku apod. vych�z� z nastaven� serveru.
		/// </summary>
		public static Condition CreateLike(PropertyInfo property, string value)
		{
			return new BinaryCondition(BinaryCondition.LikePattern, property, ValueOperand.Create(GetLikeExpression(value)));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� �et�zec na podobnost oper�torem LIKE.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value">
		/// Podporov�na hv�zdi�kov� konvence takto:
		///		- pokud parametr neobsahuje hv�zdi�ku, hled� se LIKE parametr%
		///		- pokud parametr obsahuje hv�zdi�ku, zam�n� se hv�zdi�ka za procento a hled� se LIKE parametr.
		///	Pokud parametr obsahuje speci�ln� znaky pro oper�tor LIKE jako procento nebo podtr��tko,
		///	jsou tyto znaky p�ek�dov�ny, tak�e nemaj� funk�n� v�znam.
		/// </param>
		/// <example>
		///	P�. Hled�n� v�razu "k_lo*" nenajde "kolo" ani "koloto�" proto�e _ nem� funk�n� v�znam, ale najde "k_lo" i "k_oloto�".
		/// </example>
		public static Condition CreateWildcards(PropertyInfo property, string value)
		{
			return new BinaryCondition(BinaryCondition.LikePattern, property, ValueOperand.Create(GetWildCardsLikeExpression(value)));
		}

		#region GetLikeExpression, GetWildCardsLikeExpression
		/// <summary>
		/// Transformuje �et�zec na�et�zec, kter� je mo�n� pou��t jako hodnota k oper�toru like.
		/// Nahrazuje % na [%] a _ na [_].
		/// </summary>
		public static string GetLikeExpression(string text)
		{
			string result;
			result = text.Trim().Replace("%", "[%]");
			result = result.Replace("_", "[_]");
			return result;
		}

		/// <summary>
		/// Transformuje �et�zec na�et�zec, kter� je mo�n� pou��t jako hodnota k oper�toru like. 
		/// Nav�c je vzat ohled na hv�zdi�kovou konvenci.
		/// Nahrazuje % na [%] a _ na [_] a jako posledn� zam�n� * za %.
		/// P��klad "*text1%text2*text3" bude transformov�no na "%text1[%]text2%text3".
		/// </summary>
		public static string GetWildCardsLikeExpression(string text)
		{
			string result;
			result = GetLikeExpression(text);

			if (result.Contains("*"))
			{
				result = result.Replace("*", "%");
			}
			else
			{
				result += "%";
			}

			return result;
		}
		#endregion
	
	}
}

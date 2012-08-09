using System;
using System.Collections.Generic;
using System.Text;
using Havit.Business.Conditions;

namespace Havit.Business.Conditions
{
	public static class TextCondition
	{
		public static ICondition CreateEquals(Property property, string value)
		{
			return new BinaryCondition(BinaryCondition.EqualsPattern, property, ValueOperand.FromString(value));
		}

		public static ICondition CreateLike(Property property, string value)
		{
			return new BinaryCondition(BinaryCondition.LikePattern, property, ValueOperand.FromString(GetLikeExpression(value)));
		}

		public static ICondition CreateWildcards(Property property, string value)
		{
			return new BinaryCondition(BinaryCondition.LikePattern, property, ValueOperand.FromString(GetWildCardsLikeExpression(value)));
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
				result = result.Replace("*", "%");
			else
				result += "%";

			return result;
		}
		#endregion
	
	}
}

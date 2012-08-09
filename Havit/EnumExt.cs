using System;

namespace Havit
{
	/// <summary>
	/// Poskytuje metody t�kaj�c� se z�kladn�ho v��tov�ho typu System.Enum.
	/// </summary>
	/// <remarks>
	/// T��da samostn� nen� potomkem System.Enum, proto�e ze System.Enum nelze d�dit.
	/// </remarks>
	public sealed class EnumExt
	{
		/// <summary>
		/// Vr�t� hodnotu atributu [Description("...")] ur�it� hodnoty zadan�ho v��tov�ho typu.
		/// </summary>
		/// <param name="enumType">v��tov� typ</param>
		/// <param name="hodnota">hodnota, jej� Description chceme</param>
		/// <returns>hodnota atributu [Description("...")]</returns>
		/// <remarks>Nen�-li atribut Description definov�n, vr�t� pr�zdn� �et�zec.</remarks>
		/// <example>
		///	<code>
		/// using System.ComponentModel;
		/// 
		/// public enum Barvy
		/// {
		///		[Description("�erven�")]
		///		Cervena,
		///		
		///		[Description("modr�")]
		///		Modra
		///	}
		///	</code>
		/// </example>
		public static string GetDescription(Type enumType, object hodnota)
		{
			string strRet = "";

			try
			{
				System.Reflection.FieldInfo objInfo =
					enumType.GetField(System.Enum.GetName(enumType, hodnota));

				System.ComponentModel.DescriptionAttribute objDescription =
					(System.ComponentModel.DescriptionAttribute)objInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true)[0];

				strRet = objDescription.Description;
			}
			catch(Exception)
			{
				// chyb� description
			}

			return strRet;
		}

		
		/// <summary>
		/// Pr�zdn� private constructor zamezuj�c� vytvo�en� instance t��dy.
		/// </summary>
		private EnumExt() {}
	}
}

using System;
using System.Text.RegularExpressions;
using Havit.Text.RegularExpressions;

namespace Havit
{
	/// <summary>
	/// Matematick� funkce, konstanty a r�zn� dal�� pom�cky.
	/// T��da poskytuje statick� metody a konstanty, je neinstan�n�.
	/// </summary>
	public sealed class MathExt
	{
		#region IsEven, IsOdd
		/// <summary>
		/// Vr�t� true, je-li zadan� ��slo sud�.
		/// </summary>
		/// <param name="d">��slo</param>
		/// <returns>true, je-li ��slo sud�</returns>
		public static bool IsEven(double d)
		{
			return ((d % 2) == 0);
		}


		/// <summary>
		/// Vr�t� true, je-li zadan� ��slo lich�.
		/// </summary>
		/// <param name="d">��slo</param>
		/// <returns>true, je-li ��slo lich�</returns>
		public static bool IsOdd(double d)
		{
			return !IsEven(d);
		}
		#endregion

		#region IsInteger
		/// <summary>
		/// Ov���, zda-li je zadan� textov� �et�zec cel�m ��slem.
		/// </summary>
		/// <remarks>
		/// Ov��uje se v��i regul�rn�mu v�razu <see cref="Havit.Text.RegularExpressions.RegexPatterns.Integer"/>.<br/>
		/// Pokud je text null, vr�t� false.
		/// </remarks>
		/// <param name="text">ov��ovan� textov� �et�zec</param>
		/// <returns>true, je-li text cel�m ��slem; jinak false</returns>
		public static bool IsInteger(string text)
		{
			return ((text != null) && Regex.IsMatch(text, RegexPatterns.Integer));
		}
		#endregion

		#region RoundToMultiple, CeilingToMultiple, FloorToMultiple
		/// <summary>
		/// Zaokrouhl� (aritmeticky) ��slo na nejbli��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� (aritmeticky) na nejbli�� n�sobek (multiple)</returns>
		public static double RoundToMultiple(double d, double multiple)
		{
			return Math.Round(d / multiple) * multiple;
		}

		/// <summary>
		/// Zaokrouhl� (aritmeticky) ��slo na nejbli��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� (aritmeticky) na nejbli�� n�sobek (multiple)</returns>
		public static int RoundToMultiple(double d, int multiple)
		{
			return (int)Math.Round(d / multiple) * multiple;
		}

		/// <summary>
		/// Zaokrouhl� ��slo na nejbli��� vy��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� na nejbli�� vy��� n�sobek (multiple)</returns>
		public static double CeilingToMultiple(double d, double multiple)
		{
			return Math.Ceiling(d / multiple) * multiple;
		}

		/// <summary>
		/// Zaokrouhl� ��slo na nejbli��� vy��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� na nejbli�� vy��� n�sobek (multiple)</returns>
		public static int CeilingToMultiple(double d, int multiple)
		{
			return (int)Math.Ceiling(d / multiple) * multiple;
		}

		/// <summary>
		/// Zaokrouhl� ��slo na nejbli��� ni��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� na nejbli�� ni��� n�sobek (multiple)</returns>
		public static double FloorToMultiple(double d, double multiple)
		{
			return Math.Floor(d / multiple) * multiple;
		}

		/// <summary>
		/// Zaokrouhl� ��slo na nejbli��� ni��� n�sobek (multiple) jin�ho ��sla.
		/// </summary>
		/// <param name="d">��slo k zaohrouhlen�</param>
		/// <param name="multiple">��slo, na jeho� n�sobek se m� zaokrouhlit (multiple)</param>
		/// <returns>��slo zaokrouhlen� na nejbli�� ni��� n�sobek (multiple)</returns>
		public static int FloorToMultiple(double d, int multiple)
		{
			return (int)Math.Floor(d / multiple) * multiple;
		}
		#endregion

		#region Max(params), Min(params)
		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static int Max(params int[] values)
		{
			int result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				if (values[i] > result)
				{
					result = values[i];
				}
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static double Max(params double[] values)
		{
			double result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Max(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static float Max(params float[] values)
		{
			float result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Max(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static decimal Max(params decimal[] values)
		{
			decimal result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Max(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static byte Max(params byte[] values)
		{
			byte result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				if (values[i] > result)
				{
					result = values[i];
				}
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static int Min(params int[] values)
		{
			int result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				if (values[i] < result)
				{
					result = values[i];
				}
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static double Min(params double[] values)
		{
			double result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Min(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static float Min(params float[] values)
		{
			float result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Min(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static decimal Min(params decimal[] values)
		{
			decimal result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				result = Math.Min(result, values[i]);
			}
			return result;
		}

		/// <summary>
		/// Vr�t� nejv�t�� ze zadan�ch ��sel.
		/// </summary>
		/// <param name="values">��sla k porovn�n�</param>
		/// <returns>nejv�t�� z values</returns>
		public static byte Min(params byte[] values)
		{
			byte result = values[0];
			int length = values.Length;
			for (int i = 1; i < length; i++)
			{
				if (values[i] < result)
				{
					result = values[i];
				}
			}
			return result;
		}
		#endregion

		#region private constructor
		/// <summary>
		/// Pr�zdn� private constructor zamezuj�c� vytvo�en� instance t��dy.
		/// </summary>
		private MathExt() {}
		#endregion
	}
}

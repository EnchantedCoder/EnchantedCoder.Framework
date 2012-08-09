using System;

namespace Havit.Text.RegularExpressions
{
	/// <summary>
	/// Typick� vyhled�vac� vzory pro regul�rn� v�razy.
	/// </summary>
	public sealed class RegexPatterns
	{
		/// <summary>
		/// Pattern pro kontrolu b�n�ho e-mailu:
		/// <list type="bullet">
		///		<item>povoleny jsou pouze znaky anglick� abecedy, te�ky, podtr��tka, poml�ky a plus</item>
		///		<item>dva r�zn� symboly nesm� n�sledovat po sob�, stejn� (s v�jimkou te�ky) mohou [test--test@test.com] projde, [test..test@test.com] neprojde</item>
		///		<item>nesm� za��nat symbolem</item>
		///		<item>TLD mus� m�t 2-6 znak� (.museum)</item>
		///		<item>v dom�n� sm� b�t te�ky a poml�ky, ale nesm� n�sledovat</item>
		///		<item>nep��j�m� IP adresy</item>
		///		<item>nep�ij�m� roz���en� syntax typu [Petr Novak &lt;novak@test.com&gt;]</item>
		/// </list>
		/// </summary>
		/// <remarks>
		/// http://www.regexlib.com/REDetails.aspx?regexp_id=295
		/// </remarks>
		// JK: Fix defectu 2011:
		//public const string EmailStrict = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+"
		//                                + @"@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";
		public const string EmailStrict = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.?)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+"
										+ @"@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";
		
		/// <summary>
		/// Pattern pro kontrolu identifik�tor�.
		/// Identifik�tor mus� za��nat p�smenem nebo podtr��tkem, nesledovat mohou i ��slice.
		/// </summary>
		public const string Identifier = @"^[a-zA-Z_]{1}[a-zA-Z0-9_]+$";

		/// <summary>
		/// Pattern pro kontrolu �asu. 24-hodinnov� form�t, od�lova� dvojte�ka, nepovinn� vte�iny. Nap�. 23:59:00.
		/// Nep�ij�m� 24:00.
		/// </summary>
		public const string Time24h = @"^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$";

		/// <summary>
		/// Pattern pro kontrolu IP adresy v4.
		/// </summary>
		public const string IPAddress = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\."
										+ @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\."
										+ @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\."
										+ @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";

		/// <summary>
		/// Pattern pro ov��en� cel�ch ��sel.
		/// </summary>
		/// <remarks>
		/// P�ij�m�: [1], [+15], [0], [-10], [+0]<br/>
		/// Odm�t�: [1.0], [abc], [+], [1,15]
		/// </remarks>
		public const string Integer = @"^[-+]?\d+$";

		#region private constructor
		/// <summary>
		/// private constructor k zabr�n�n� instanciace statick� t��dy.
		/// </summary>
		private RegexPatterns() {}
		#endregion
	}
}

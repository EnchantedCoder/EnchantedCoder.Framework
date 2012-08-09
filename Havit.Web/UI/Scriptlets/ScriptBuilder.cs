using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// Jednoduch� skl�da� �et�zce. 
    /// Vnit�n� pou��v� StringBuilder, na rozd�l od n�j umo��uje text
    /// jen p�id�vat, neumo��uje jej modifikovat.
    /// </summary>
	public class ScriptBuilder
	{
		#region Private fields
		private StringBuilder builder = new StringBuilder();
		#endregion

		#region Append, AppendLine, AppendFormat
		/// <summary>
		/// P�ipoj� �et�zec na konec textu.
		/// </summary>
		/// <param name="value">P�id�van� hodnota.</param>
		public void Append(string value)
		{
			builder.Append(value);
		}
		
		/// <summary>
		/// P�ipoj� �et�zec na konec textu a vlo�� symbol konce ��dku (Environment.NewLine).
		/// </summary>
		/// <param name="value">P�id�van� hodnota.</param>
		public void AppendLine(string value)
		{
			this.Append(value);
			builder.Append(Environment.NewLine);
		}

		/// <summary>
		/// Po zform�tov�n� p�ipoj� �et�zec na konec textu.
		/// </summary>
		/// <param name="value">�ablona p�id�van� hodnota.</param>
		/// <param name="parameters">Parametry �ablony.</param>
		public void AppendFormat(string value, params object[] parameters)
		{
			builder.AppendFormat(value, parameters);
		}
		
		/// <summary>
		/// Po zform�tov�n� p�ipoj� �et�zec na konec textu a dopln� jej symbolem pro konec ��dky.
		/// </summary>
		/// <param name="value">�ablona p�id�van� hodnota.</param>
		/// <param name="parameters">Parametry �ablony.</param>
		public void AppendLineFormat(string value, params object[] parameters)
		{
			this.AppendFormat(value, parameters);
			builder.Append(Environment.NewLine);
		}
		#endregion

		#region ToString
		/// <summary>
        /// Vr�t� slo�en� text.
        /// </summary>
        /// <returns>Slo�en� text.</returns>
		public override string ToString()
		{
			return builder.ToString();
		}
		#endregion
	}
}

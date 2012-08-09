using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// Argumenty ud�losti ScriptSubstituing.
	/// </summary>
	public class ClientScriptSubstituingEventArgs: EventArgs
	{
		#region Constructor
		/// <summary>
		/// Konstuktor.
		/// </summary>
		/// <param name="clientScript">Klientsk� skript k substituci.</param>
		public ClientScriptSubstituingEventArgs(string clientScript)
		{
			this.clientScript = clientScript;
		}
		#endregion

		#region ClientScript
		/// <summary>
		/// Klientsk� skript k substituci.
		/// </summary>
		public string ClientScript
		{
			get { return clientScript; }
			set { clientScript = value; }
		}
		private string clientScript;
		#endregion
	}
}

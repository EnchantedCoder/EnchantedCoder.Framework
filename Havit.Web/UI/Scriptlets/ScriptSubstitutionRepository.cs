using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// Repository substituc�.
	/// </summary>
    public class ScriptSubstitutionRepository : List<IScriptSubstitution>, IScriptSubstitution
    {
		#region Default (static)
		/// <summary>
		/// V�choz� substituce. Pou�ita, pokud nen� scriptletu nastaveno jinak.
		/// </summary>
		public static ScriptSubstitutionRepository Default
		{
			get
			{
				lock (typeof(ScriptSubstitutionRepository))
				{
					if (_default == null)
					{
						_default = new ScriptSubstitutionRepository();
					}
				}
				return _default;
			}
		}
		private static ScriptSubstitutionRepository _default = null;
		#endregion

		#region Substitute
		/// <summary>
		/// Provede substituci t�m zp�sobem, �e zavol� postupn� substituce
		/// na v�ech instanc�ch v repository.
		/// </summary>
		/// <param name="script">Skript, ve kter�m m� doj�t k substituci.</param>
		/// <returns>Substituovan� skript.</returns>
		public string Substitute(string script)
        {
            string result = script;
			foreach (IScriptSubstitution scriptSubstitution in this)
			{
				result = scriptSubstitution.Substitute(result);
			}
            return result;                
        }
		#endregion
	}
}
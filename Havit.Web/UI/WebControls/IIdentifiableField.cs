using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Sloupec, kter� se um� identifikovat (m� ID).
	/// </summary>
	public interface IIdentifiableField
	{
		/// <summary>
		/// ID sloupce. Umo�n� GridView hledat sloupec podle ID.
		/// </summary>
		string ID { get; }
	}
}

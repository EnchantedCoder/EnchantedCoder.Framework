using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Interface, kter� implementuj� controly, kdy� cht�j� b�t nositelem skinovateln�ch properties pro CommandField.
	/// Implementuje nap�. <see cref="GridViewExt"/>, aby p�es GridView mohl b�t skinov�n jeho CommandField.
	/// </summary>
	public interface ICommandFieldStyle
	{
		CommandFieldStyle CommandFieldStyle { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Deleg�t na metodu GetObject.
	/// </summary>
	/// <param name="objectID">ID objektu, kter� se m� metodou vr�tit.</param>
	/// <returns>Business objekt na z�klad� ID.</returns>
	public delegate BusinessObjectBase GetObjectDelegate(int objectID);
}

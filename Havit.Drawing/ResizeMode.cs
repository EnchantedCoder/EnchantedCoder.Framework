using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Drawing
{
	/// <summary>
	/// Re�imy pro ImageExt.Resize()
	/// </summary>
	public enum ResizeMode
	{
		/// <summary>
		/// Uprav� rozm�ry obr�zku tak, aby se jeho vn�j�� obrys ve�el do po�adovan�ch rozm�r�. Pom�r stran zachov�.
		/// </summary>
		PreserveAspectRatioFitBox = 1,

		/// <summary>
		/// Pokud je obr�zek v�t�� ne� po�adovan� rozm�ry, je redukov�n tak, aby se jeho vn�j�� rozm�r ve�el do po�adovan�ho boxu.
		/// Pom�r stran je zachov�n. Pokud je obr�zek men��, nen� zv�t�en, ale z�st�v� nezm�n�n.
		/// </summary>
		PreserveAspectRatioFitBoxReduceOnly = 2,

		/// <summary>
		/// Obr�zek se uprav� na p�esn� po�adovan� rozm�ry. V p��pad� pot�eby je nat�hnut, nemus� b�t zachov�n pom�r stran.
		/// </summary>
		AdjustToBox = 3
	}
}

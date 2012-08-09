using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// P�edek generick�ho typu <see cref="PropertyHolder{T}"/>. 
	/// Pot�ebujeme kolekci PropertyHolder� a kolekci generick�ch typ� nelze ud�lat.
	/// </summary>
	public abstract class PropertyHolderBase
	{
		/// <summary>
		/// Zru�� p��znak zm�ny hodnoty, hodnota se pot� chov� jako nezm�n�n�.
		/// </summary>
		internal abstract void ClearDirty();
	}
}

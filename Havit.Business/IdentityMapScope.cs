using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// <see cref="Scope{T}"/> pro <see cref="IdentityMap"/>.
	/// </summary>
	public class IdentityMapScope : Scope<IdentityMap>
	{
		/// <summary>
		/// Vytvo�� <see cref="IdentityMapScope"/> obaluj�c� novou <see cref="IdentityMap"/>.
		/// </summary>
		public IdentityMapScope()
			: base(new IdentityMap(), true)
		{
		}
	}
}

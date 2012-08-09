using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// B�zov� t��da pro v�echny property-info objektu.
	/// </summary>
	public abstract class PropertyInfo
	{
		/// <summary>
		/// T��da, kter� property n�le��.
		/// </summary>
		public ObjectInfo Parent
		{
			get { return parent; }
			internal set { parent = value; }
		}
		private ObjectInfo parent;
	}
}

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
		/// Inicializuje instanci.
		/// </summary>
		/// <param name="owner"></param>
		protected virtual void Initialize(ObjectInfo owner)
		{
			this.owner = owner;
			this.isInitialized = true;
		}
		private bool isInitialized = false;

		/// <summary>
		/// T��da, kter� property n�le��.
		/// </summary>
		public ObjectInfo Owner
		{
			get { return owner; }
		}
		private ObjectInfo owner;

		/// <summary>
		/// Ov���, �e byla instance inicializov�na. Pokud ne, vyhod� v�jimku.
		/// </summary>
		protected void CheckInitialization()
		{
			if (!isInitialized)
			{
				throw new InvalidOperationException("Instance nebyla inicializov�na.");
			}
		}
	}
}

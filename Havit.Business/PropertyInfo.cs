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
		#region Initialize
		/// <summary>
		/// Inicializuje instanci.
		/// </summary>
		/// <param name="owner">ObjectInfo vlastn�c� property.</param>
		/// <param name="propertyName">N�zev vlastnosti.</param>
		protected virtual void Initialize(ObjectInfo owner, string propertyName)
		{
			this.owner = owner;
			this.propertyName = propertyName;
			this.isInitialized = true;
		}
		#endregion

		#region isInitialized (private field)
		private bool isInitialized = false;
		#endregion

		#region Owner
		/// <summary>
		/// T��da, kter� property n�le��.
		/// </summary>
		public ObjectInfo Owner
		{
			get { return owner; }
		}
		private ObjectInfo owner;
		#endregion

		#region PropertyName
		/// <summary>
		/// N�zev property reprezentovan� instanc�.
		/// </summary>
		public string PropertyName
		{
			get { return propertyName; }
		}
		private string propertyName;
		#endregion

		#region CheckInitialization
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
		#endregion

	}
}

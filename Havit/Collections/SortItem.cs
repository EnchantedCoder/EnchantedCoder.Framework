using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Havit.Collections
{
	/// <summary>
	/// Reprezentuje polo�ku �azen�.
	/// </summary>
	[Serializable]	
	public class SortItem
	{
		#region Constructors
		/// <summary>
		/// Vytvo�� pr�zdnou instanci po�ad�.
		/// </summary>
		public SortItem()
		{
		}

		/// <summary>
		/// Vytvo�� polo�ku �azen� podle expression a sm�ru �azen�.
		/// </summary>
		public SortItem(string expression, SortDirection direction)
			: this()
		{
			this.expression = expression;
			this.direction = direction;
		}
		#endregion

		#region Properties
		/// <summary>
		/// V�raz, dle kter�ho se �ad�.
		/// </summary>
		public virtual string Expression
		{
			get { return expression; }
			set { expression = value; }
		}
		private string expression;

		/// <summary>
		/// Sm�r �azen�.
		/// </summary>
		public virtual SortDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		private SortDirection direction = SortDirection.Ascending;
		#endregion
	}
}

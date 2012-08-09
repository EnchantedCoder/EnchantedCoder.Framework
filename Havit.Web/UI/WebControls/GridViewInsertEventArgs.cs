using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Havit.Web.UI.WebControls
{
	public class GridViewInsertEventArgs : CancelEventArgs
	{
		/// <summary>
		/// Vrac� index ��dku GridView, v kter�m se odehr�v� Insert.
		/// </summary>
		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}
		private int _rowIndex;

		/// <summary>
		/// Vytvo�� instanci.
		/// </summary>
		/// <param name="rowIndex">index ��dku GridView, v kter�m se odehr�v� Insert</param>
		public GridViewInsertEventArgs(int rowIndex)
			: base(false)
		{
			this._rowIndex = rowIndex;
		}

	}
}

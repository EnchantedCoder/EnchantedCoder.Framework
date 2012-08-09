using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	public class ListControlItemDataBoundEventArgs : EventArgs
	{
		/// <summary>
		/// Prvek, kter�ho se ud�lost t�k�.
		/// </summary>
		public ListItem Item
		{
			get
			{
				return this._item;
			}
		}
		private ListItem _item;

		/// <summary>
		/// Data, na jejich z�klad� prvek vzniknul.
		/// </summary>
		public object DataItem
		{
			get
			{
				return _dataItem;
			}
			set
			{
				_dataItem = value;
			}
		}
		private object _dataItem;

		/// <summary>
		/// Vytvo�� instanci.
		/// </summary>
		/// <param name="item">Prvek, kter�ho se ud�lost t�k�.</param>
		public ListControlItemDataBoundEventArgs(ListItem item, object dataItem)
		{
			this._item = item;
			this._dataItem = dataItem;
		}
	}
}

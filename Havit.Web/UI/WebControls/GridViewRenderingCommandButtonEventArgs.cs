using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Argumenty ud�losti <see cref="GridViewExt.RowCustomizingCommandButton"/>.
	/// </summary>
	public class GridViewRowCustomizingCommandButtonEventArgs : EventArgs
	{
		#region CommandName
		/// <summary>
		/// Vr�t� CommandName tla��tka.
		/// </summary>
		public string CommandName
		{
			get
			{
				return _commandName;
			}
		}
		private string _commandName;
		#endregion

		#region Visible
		/// <summary>
		/// Vlastnost Visible tla��tka.
		/// </summary>
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}
		private bool _visible;
		#endregion

		#region Enabled
		/// <summary>
		/// Vlastnosti Enabled tla��tka.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
			}
		}
		private bool _enabled;
		#endregion

		#region RowIndex
		/// <summary>
		/// Index ��dku, kter�ho se ud�lost t�k�.
		/// </summary>
		public int RowIndex
		{
			get
			{
				return _rowIndex;
			}
		}
		private int _rowIndex;
		#endregion

		#region DataItem
		/// <summary>
		/// DataItem ��dku, kter�ho se ud�lost t�k�.
		/// </summary>
		public object DataItem
		{
			get
			{
				return _dataItem;
			}
		}
		private object _dataItem;
		#endregion

		#region GridViewRowCustomizingCommandButtonEventArgs
		/// <summary>
		/// Vytvo�� instanci t��dy <see cref="GridViewRowCustomizingCommandButtonEventArgs"/>.
		/// </summary>
		/// <param name="commandName">CommandName obsluhovan�ho buttonu</param>
		public GridViewRowCustomizingCommandButtonEventArgs(string commandName, int rowIndex, object dataItem)
		{
			this._commandName = commandName;
			this._rowIndex = rowIndex;
			this._dataItem = dataItem;
		}
		#endregion
	}
}

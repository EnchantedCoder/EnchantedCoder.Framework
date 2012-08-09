using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// GridViewInsertedEventArgs.
	/// </summary>
	public class GridViewInsertedEventArgs : EventArgs
	{
		/// <summary>
		/// Indikuje, zda-li m� GridView z�stat po zpracov�n� ud�lost v re�imu editace nov�ho ��dku.
		/// </summary>
		public bool KeepInEditMode
		{
			get
			{
				return this._keepInEditMode;
			}
			set
			{
				this._keepInEditMode = value;
			}
		}
		private bool _keepInEditMode = false;
	}
}

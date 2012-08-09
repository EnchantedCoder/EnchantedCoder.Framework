using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// EnterprisGridView poskytuje:
	/// - hled�n� kl��e ��dku, ve kter�m do�lo k ud�losti
	/// - hled�n� sloupce (IEnterpriseField) na z�klad� ID sloupce
	/// - str�nkov�n�
	/// - zve�ej�uje vlastnost RequiresDataBinding
	/// - automatick� databinding p�i prvn�m na�ten� str�nky nebo nastaven� RequiresDataBinding na true (podm�n�no vlastnost� AutoDataBind)
	/// - p�echod na str�nku 0 p�i zm�n� �azen�
	/// </summary>
	public class EnterpriseGridView : GridViewExt
	{
		#region Constructor
		/// <summary>
		/// Vytvo�� instanci EnterpriseGridView. Nastavuje defaultn� DataKeyNames na ID.
		/// </summary>
		public EnterpriseGridView()
		{
			this.DataKeyNames = new string[] { "ID" };
		}
		#endregion

		#region GetRowID - Hled�n� kl��e polo�ky
		/// <summary>
		/// Nalezne hodnotu ID kl��e polo�ky, ve kter�m se nach�z� control.
		/// </summary>
		/// <param name="control">Control. Hled� se ��dek, ve kter�m se GridViewRow nal�z� a DataKey ��dku.</param>
		/// <returns>Vrac� hodnotu kl��e.</returns>
		public int GetRowID(Control control)
		{
			return (int)GetRowKey(control).Value;
		}

		/// <summary>
		/// Nalezne hodnotu ID kl��e polo�ky na z�klad� ud�losti.
		/// </summary>
		/// <param name="e">Ud�lost, ke kter� v gridu do�lo.</param>
		/// <returns>Vrac� hodnotu kl��e dan�ho ��dku.</returns>
		public int GetRowID(GridViewCommandEventArgs e)
		{
			return (int)GetRowKey(e).Value;
		}

		/// <summary>
		/// Nalezne hodnotu ID kl��e polo�ky na z�klad� indexu ��dku v gridu.
		/// </summary>
		/// <param name="rowIndex">index ��dku</param>
		/// <returns>Vrac� hodnotu kl��e dan�ho ��dku.</returns>
		public int GetRowID(int rowIndex)
		{
			return (int)GetRowKey(rowIndex).Value;
		}
		#endregion

		//region Zru�en� DataSourceID (Na�e �azen� jej nepodporuje.)
		///// <summary>
		///// Zru��me mo�nost nastaven� DataSourceID. P�i pokusu nastavit not-null hodnotu
		///// dojde k vyvol�n� v�jimky.
		///// </summary>
		//public override string DataSourceID
		//{
		//    get
		//    {
		//        return base.DataSourceID;
		//    }
		//    set
		//    {
		//        if (value != null)
		//            throw new ArgumentException("DataSourceID not supported.");
		//        base.DataSourceID = value;
		//    }
		//}
		//endregion
	}
}

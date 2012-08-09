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
	public class EnterpriseGridView : SortingGridView
	{
		#region Constructor
		/// <summary>
		/// Vytvo�� instanci EnterpriseGridView.
		/// Vypne automatick� generov�n� sloupc� p�i databindingu (nastav� AutoGenerateColumns na false).
		/// </summary>
		public EnterpriseGridView()
		{
			AutoGenerateColumns = false;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Nastavuje automatick� databind na GridView.		
		/// </summary>
		public bool AutoDataBind
		{
			get
			{
				return (bool)(ViewState["AutoDataBind"] ?? true);
			}
			set
			{
				ViewState["AutoDataBind"] = value;
			}
		}
		
		/// <summary>
		/// Zp��stup�uje pro �ten� chr�n�nou vlastnost RequiresDataBinding.
		/// </summary>
		public new bool RequiresDataBinding
		{
			get
			{
				return base.RequiresDataBinding;
			}
			protected set
			{
				base.RequiresDataBinding = true;
			}
		}
		#endregion

		#region OnInit
		/// <summary>
		/// Inicializuje EnterpriseGridView.
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// Pokud dojde k vyvol�n� ud�losti, kter� nem� obsluhu, je vyvol�na v�jimka.
			// Proto�e ale n�kter� z�le�itosti �e��me sami, nastav�me "pr�zdn�" obsluhy ud�lost�
			// (nasm�rujeme je do �ern� d�ry).			
			this.PageIndexChanging += new GridViewPageEventHandler(EnterpriseGridView_EventBlackHole);
			this.RowCancelingEdit += new GridViewCancelEditEventHandler(EnterpriseGridView_EventBlackHole);
			this.RowDeleting += new GridViewDeleteEventHandler(EnterpriseGridView_EventBlackHole);
			this.RowEditing += new GridViewEditEventHandler(EnterpriseGridView_EventBlackHole);
			this.RowUpdating += new GridViewUpdateEventHandler(EnterpriseGridView_EventBlackHole);			
		}

		private void EnterpriseGridView_EventBlackHole(object sender, EventArgs e)
		{
		}
		#endregion

		#region Hled�n� kl��e polo�ky
		/// <summary>
		/// Nalezne hodnotu kl��e polo�ky, ve kter�m se nach�z� control.
		/// </summary>
		/// <param name="control">Control. Hled� se ��dek, ve kter�m se GridViewRow nal�z� 
		/// a DataKey ��dku.</param>
		/// <returns>Vrac� hodnotu kl��e. Pokud nen� kl�� nalezen
		/// (Control nen� v GridView), vrac� null.
		/// </returns>
		public object GetKeyValue(Control control)
		{
			if (control == null || control.Parent == null)
				return null;
			if (control is GridViewRow && control.Parent.Parent == this)
				return DataKeys[((GridViewRow)control).RowIndex].Value;
			return GetKeyValue(control.NamingContainer);
		}

		/// <summary>
		/// Nalezne hodnotu kl��e polo�ky na z�klad� ud�losti.
		/// </summary>
		/// <param name="e">Ud�lost, ke kter� v gridu do�lo.</param>
		/// <returns>Vrac� hodnotu kl��e dan�ho ��dku. Pokud nen� kl�� nalezen vrac� null.</returns>
		public object GetKeyValue(GridViewCommandEventArgs e)
		{
			if ((string)e.CommandArgument != String.Empty)
			{
				int index = int.Parse((string)e.CommandArgument);
				return DataKeys[index].Value;
			}

			if (e.CommandSource is Control)
			{
				return GetKeyValue((Control)e.CommandSource);
			}
			
			return null;
		}
		#endregion

		#region Hled�n� sloupc�
		/// <summary>
		/// Vyhled� sloupec podle id. Vyhled�v� jen sloupce implementuj�c� rozhran� IEnterpriseField.
		/// </summary>
		/// <param name="id">ID, podle kter�ho se sloupec vyhled�v�.</param>
		/// <returns>Nalezen� sloupec nebo null, pokud nen� nalezen.</returns>
		public DataControlField FindColumn(string id)
		{
			foreach (DataControlField field in Columns)
				if ((field is IEnterpriseField) && ((IEnterpriseField)field).ID == id)
					return field;
			return null;
		}
		#endregion

		#region Str�nkov�n�
		/// <summary>
		/// Pokud nen� str�nkov�n� stornov�no, zm�n�me str�nku na c�lovou.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPageIndexChanging(GridViewPageEventArgs e)
		{
			base.OnPageIndexChanging(e);
			if (!e.Cancel)
			{
				PageIndex = e.NewPageIndex;
				RequiresDataBinding = true;
			}
		}

		/// <summary>
		/// Po set��d�n� podle sloupce zajist� u v�cestr�nkov�ch grid� n�vrat na prvn� str�nku
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSorted(EventArgs e)
		{
			base.OnSorted(e);
			PageIndex = 0;
		}

		#endregion

		//#region Zru�en� DataSourceID (Na�e �azen� jej nepodporuje.)
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
		//#endregion

		#region OnPreRender
		/// <summary>
		/// Zajist�me DataBinding, pokud maj� vlastnosti AutoDataBind a RequiresDataBinding hodnotu true.
		/// </summary>
		protected override void OnPreRender(EventArgs e)
		{
			if (AutoDataBind && RequiresDataBinding)
			{
				DataBind();
			}

			base.OnPreRender(e);
		}
		#endregion

		#region SetRequiresDataBinding
		/// <summary>
		/// Nastav� RequiresDataBinding na true.
		/// </summary>
		public void SetRequiresDatabinding()
		{
			RequiresDataBinding = true;
		}
		#endregion
	}
}

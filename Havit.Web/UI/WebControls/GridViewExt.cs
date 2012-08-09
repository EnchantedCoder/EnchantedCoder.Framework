using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// GridView implementuj�c� hightlighting, sorting a v�choz� obsluhu ud�lost� editace, str�nkov�n�, ...
	/// </summary>
	public class GridViewExt : SortingGridView
	{
		#region GridViewExt
		/// <summary>
		/// Vytvo�� instanci GridViewExt. Nastavuje defaultn� AutoGenerateColumns na false.
		/// </summary>
		public GridViewExt()
		{
			AutoGenerateColumns = false;
		}
		#endregion

		#region AutoDataBind
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
		#endregion

		#region RequiresDataBinding (new), SetRequiresDatabinding
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

		/// <summary>
		/// Nastav� RequiresDataBinding na true.
		/// </summary>
		public void SetRequiresDatabinding()
		{
			RequiresDataBinding = true;
		}
		#endregion

		#region OnInit (EventBlackHole)
		/// <summary>
		/// Inicializuje EnterpriseGridView.
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// Pokud dojde k vyvol�n� ud�losti, kter� nem� obsluhu, je vyvol�na v�jimka.
			// Proto�e ale n�kter� z�le�itosti �e��me sami, nastav�me "pr�zdn�" obsluhy ud�lost�
			// (nasm�rujeme je do �ern� d�ry).			
			this.PageIndexChanging += new GridViewPageEventHandler(GridViewExt_EventBlackHole);
			this.RowCancelingEdit += new GridViewCancelEditEventHandler(GridViewExt_EventBlackHole);
			this.RowDeleting += new GridViewDeleteEventHandler(GridViewExt_EventBlackHole);
			this.RowEditing += new GridViewEditEventHandler(GridViewExt_EventBlackHole);
			this.RowUpdating += new GridViewUpdateEventHandler(GridViewExt_EventBlackHole);
		}

		private void GridViewExt_EventBlackHole(object sender, EventArgs e)
		{
			// NOOP
		}
		#endregion

		#region GetKeyValue - Hled�n� kl��e polo�ky
		/// <summary>
		/// Nalezne hodnotu kl��e polo�ky, ve kter�m se nach�z� control.
		/// </summary>
		/// <param name="control">Control. Hled� se ��dek, ve kter�m se GridViewRow nal�z� a DataKey ��dku.</param>
		/// <returns>Vrac� hodnotu kl��e.</returns>
		public DataKey GetRowKey(Control control)
		{
			if (DataKeyNames.Length == 0)
			{
				throw new InvalidOperationException("Nen� nastavena property DataKeyNames, nelze pracovat s kl��i.");
			}
			if ((control == null) || (control.Parent == null))
			{
				throw new ArgumentException("Z controlu se nepoda�ilo kl�� dohledat.", "control");
			}

			if ((control is GridViewRow) && (control.Parent.Parent == this))
			{
				return DataKeys[((GridViewRow)control).RowIndex];
			}
			return GetRowKey(control.NamingContainer);
		}

		/// <summary>
		/// Nalezne hodnotu kl��e polo�ky na z�klad� ud�losti.
		/// </summary>
		/// <param name="e">Ud�lost, ke kter� v gridu do�lo.</param>
		/// <returns>Vrac� hodnotu kl��e dan�ho ��dku.</returns>
		public DataKey GetRowKey(GridViewCommandEventArgs e)
		{
			if (DataKeyNames.Length == 0)
			{
				throw new InvalidOperationException("Nen� nastavena property DataKeyNames, nelze pracovat s kl��i.");
			}

			if ((string)e.CommandArgument != String.Empty)
			{
				int rowIndex = int.Parse((string)e.CommandArgument);
				return GetRowKey(rowIndex);
			}

			if (e.CommandSource is Control)
			{
				return GetRowKey((Control)e.CommandSource);
			}

			throw new ArgumentException("Ud�lost neobsahuje data, z kter�ch by se dal kl�� ur�it.");
		}

		/// <summary>
		/// Nalezne hodnotu kl��e polo�ky na z�klad� indexu ��dku v gridu.
		/// </summary>
		/// <param name="rowIndex">index ��dku</param>
		/// <returns>Vrac� hodnotu kl��e dan�ho ��dku.</returns>
		public DataKey GetRowKey(int rowIndex)
		{
			if (DataKeyNames.Length == 0)
			{
				throw new InvalidOperationException("Nen� nastavena property DataKeyNames, nelze pracovat s kl��i.");
			}

			return this.DataKeys[rowIndex];
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
			{
				if ((field is IIdentifiableField) && ((IIdentifiableField)field).ID == id)
				{
					return field;
				}
			}
			return null;
		}
		#endregion

		#region OnRowEditing
		/// <summary>
		/// V�choz� chov�n� RowEditing - nastav� editovan� ��dek.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowEditing(GridViewEditEventArgs e)
		{
			this.EditIndex = e.NewEditIndex;
			base.OnRowEditing(e);
		}
		#endregion

		#region OnRowUpdating
		/// <summary>
		/// V�choz� chov�n� RowUpdating - pokud nen� zvoleno e.Cancel, pak vypne editaci ��dku.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowUpdating(GridViewUpdateEventArgs e)
		{
			base.OnRowUpdating(e);

			if (!e.Cancel)
			{
				this.EditIndex = -1;
			}
		}
		#endregion

		#region OnRowCancelingEdit
		/// <summary>
		/// V�choz� chov�n� RowUpdating - pokud nen� zvoleno e.Cancel, pak vypne editaci ��dku.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowCancelingEdit(GridViewCancelEditEventArgs e)
		{
			base.OnRowCancelingEdit(e);

			if (!e.Cancel)
			{
				this.EditIndex = -1;
			}
		}
		#endregion

		#region OnPageIndexChanging
		/// <summary>
		/// V�choz� chov�n� ud�losti OnPageIndexChanging Pokud nen� str�nkov�n� stornov�no, zm�n�me str�nku na c�lovou.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnPageIndexChanging(GridViewPageEventArgs e)
		{
			base.OnPageIndexChanging(e);

			if (!e.Cancel)
			{
				PageIndex = e.NewPageIndex;
				RequiresDataBinding = true;
			}
		}
		#endregion

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
	}
}

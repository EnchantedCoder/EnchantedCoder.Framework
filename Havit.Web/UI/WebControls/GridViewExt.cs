using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Globalization;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// GridView implementuj�c� hightlighting, sorting a v�choz� obsluhu ud�lost� editace, str�nkov�n�, ...
	/// </summary>
	public class GridViewExt : SortingGridView
	{
		#region GetInsertRowDataItem
		/// <summary>
		/// Metoda, kter� vrac� data-item nov�ho Insert ��dku. Obvykle p�ednastaveno default hodnotami.
		/// </summary>
		public GetInsertRowDataItemDelegate GetInsertRowDataItem
		{
			get
			{
				return _getInsertRowDataItem;
			}
			set
			{
				_getInsertRowDataItem = value;
			}
		}
		private GetInsertRowDataItemDelegate _getInsertRowDataItem;
		#endregion

		#region AllowInserting
		/// <summary>
		/// Indikuje, zda-li je povoleno p�id�v�n� nov�ch polo�ek ��dkem Insert.
		/// </summary>
		/// <remarks>
		/// Spolu s AllowInserting je pot�eba nastavit deleg�ta <see cref="GetInsertRowDataItem"/>
		/// pro z�sk�v�n� v�choz�ch dat pro novou polo�ku. D�le lze nastavit pozici pomoc� <see cref="InsertPosition"/>.
		/// </remarks>
		[Browsable(true), DefaultValue("true"), Category("Behavior")]
		public bool AllowInserting
		{
			get
			{
				object o = ViewState["_AllowInserting"];
				return o == null ? false : (bool)o;
			}
			set
			{
				ViewState["_AllowInserting"] = value;
			}
		}
		#endregion

		#region InsertPosition
		/// <summary>
		/// Indikuje, zda-li je povoleno p�id�v�n� nov�ch polo�ek.
		/// </summary>
		public GridViewInsertRowPosition InsertRowPosition
		{
			get
			{
				object o = ViewState["_InsertRowPosition"];
				return o == null ? GridViewInsertRowPosition.Bottom : (GridViewInsertRowPosition)o;
			}
			set
			{
				ViewState["_InsertRowPosition"] = value;
			}
		}
		#endregion

		#region InsertRowDataSourceIndex
		/// <summary>
		/// Pozice ��dku pro insert.
		/// </summary>
		/// <remarks>
		///  Nutno ukl�dat do viewstate kv�li zp�tn� rekonstrukci ��dk� bez data-bindingu. Jinak nechod� spr�vn� eventy.
		/// </remarks>
		protected int InsertRowDataSourceIndex
		{
			get
			{
				object o = ViewState["_InsertRowDataSourceIndex"];
				return o == null ? -1 : (int)o;
			}
			set
			{
				ViewState["_InsertRowDataSourceIndex"] = value;
			}
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

		#region Events - RowInserting, RowInserted
		/// <summary>
		/// Ud�lost, kter� se vol� p�i vlo�en� nov�ho ��dku (kliknut� na tla��tko Insert).
		/// </summary>
		/// <remarks>
		/// Obsluha ud�losti m� vyzvednout data a zalo�it nov� z�znam.
		/// </remarks>
		[Category("Action")]
		public event GridViewInsertEventHandler RowInserting
		{
			add { base.Events.AddHandler(EventItemInserting, value); }
			remove { base.Events.RemoveHandler(EventItemInserting, value); }
		}

		/// <summary>
		/// Ud�lost, kter� se vol� po vlo�en� nov�ho ��dku (po ud�losti RowInserting).
		/// </summary>
		[Category("Action")]
		public event GridViewInsertedEventHandler RowInserted
		{
			add { base.Events.AddHandler(EventItemInserted, value); }
			remove { base.Events.RemoveHandler(EventItemInserted, value); }
		}

		private static readonly object EventItemInserting = new object();
		private static readonly object EventItemInserted = new object();
		#endregion

		#region _insertRowIndex
		/// <summary>
		/// Skute�n� index InsertRow na str�nce.
		/// </summary>
		private int insertRowIndex = -1;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci GridViewExt. Nastavuje defaultn� AutoGenerateColumns na false.
		/// </summary>
		public GridViewExt()
		{
			AutoGenerateColumns = false;
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

		#region FindColumn - Hled�n� sloupc�
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

		#region PerformDataBinding (override - Insert)
		/// <summary>
		/// Zaji��uje data-binding dat na GridView.
		/// </summary>
		/// <param name="data">data</param>
		protected override void PerformDataBinding(IEnumerable data)
		{
			insertRowIndex = -1;

			// INSERTING
			if (AllowInserting)
			{
				if (GetInsertRowDataItem == null)
				{
					throw new InvalidOperationException("P�i AllowInserting mus�te nastavit GetInsertRowData");
				}
				ArrayList newData = new ArrayList();

				object insertRowDataItem = GetInsertRowDataItem();
				foreach (object item in data)
				{
					newData.Add(item);
				}
				if (AllowPaging)
				{
					int pageCount = (newData.Count + this.PageSize) - 1;
					if (pageCount < 0)
					{
						pageCount = 1;
					}
					pageCount = pageCount / this.PageSize;

					for (int i = 0; i < this.PageIndex; i++)
					{
						newData.Insert(0, insertRowDataItem);
					}
					for (int i = this.PageIndex + 1; i < pageCount; i++)   // p�epo��tat
					{
						newData.Add(insertRowDataItem);
					}
				}
				if (EditIndex < 0)
				{
					switch (InsertRowPosition)
					{
						case GridViewInsertRowPosition.Top:
							this.InsertRowDataSourceIndex = (this.PageSize * this.PageIndex);
							break;
						case GridViewInsertRowPosition.Bottom:
							if (AllowPaging)
							{
								this.InsertRowDataSourceIndex = Math.Min((((this.PageIndex + 1) * this.PageSize) - 1), newData.Count);
							}
							else
							{
								this.InsertRowDataSourceIndex = newData.Count;
							}
							break;
					}
					newData.Insert(InsertRowDataSourceIndex, insertRowDataItem);
				}
				data = newData;
			}
			base.PerformDataBinding(data);
		}
		#endregion

		#region CreateRow (override - Insert)
		protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
		{
			GridViewRow row = base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);

			// ��dek s nov�m objektem p�ep�n�me do stavu Insert, co� zajist� zvolen� EditItemTemplate a spr�vn� chov�n� CommandFieldu.
			if ((rowType == DataControlRowType.DataRow)
				&& (AllowInserting)
				&& (dataSourceIndex == InsertRowDataSourceIndex))
			{
				insertRowIndex = rowIndex;
				row.RowState = DataControlRowState.Insert;
			}

			// abychom m�li na str�nce v�dy stejn� po�et ��dek, tak u insertingu p�i editaci skr�v�me posledn� ��dek
			if ((AllowInserting) && (insertRowIndex < 0) && (rowIndex == (this.PageSize - 1)))
			{
				row.Visible = false;
			}
			return row;
		}
		#endregion

		#region OnRowCommand (override -Insert)
		/// <summary>
		/// Metoda, kter� spou�t� ud�lost RowCommand.
		/// </summary>
		/// <remarks>
		/// Implementace cachyt�v� a obsluhuje p��kaz Insert.
		/// </remarks>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowCommand(GridViewCommandEventArgs e)
		{
			base.OnRowCommand(e);

			bool causesValidation = false;
			string validationGroup = String.Empty;
			if (e != null)
			{
				IButtonControl control = e.CommandSource as IButtonControl;
				if (control != null)
				{
					causesValidation = control.CausesValidation;
					validationGroup = control.ValidationGroup;
				}
			}

			switch (e.CommandName)
			{
				case DataControlCommands.InsertCommandName:
					this.HandleInsert(Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture), causesValidation);
					break;
			}
		}
		#endregion

		#region HandleInsert
		/// <summary>
		/// Metoda, kter� ��d� logiku p��kazu Insert.
		/// </summary>
		/// <param name="rowIndex">index ��dku, kde insert prob�h�</param>
		/// <param name="causesValidation">p��znak, zda-li m� prob�hat validace</param>
		protected virtual void HandleInsert(int rowIndex, bool causesValidation)
		{
			if ((!causesValidation || (this.Page == null)) || this.Page.IsValid)
			{
				GridViewInsertEventArgs argsInserting = new GridViewInsertEventArgs(rowIndex);
				this.OnRowInserting(argsInserting);
				if (!argsInserting.Cancel)
				{
					GridViewInsertedEventArgs argsInserted = new GridViewInsertedEventArgs();
					this.OnRowInserted(argsInserted);
					if (!argsInserted.KeepInEditMode)
					{
						this.EditIndex = -1;
						this.InsertRowDataSourceIndex = -1;
						base.RequiresDataBinding = true;
					}
				}
			}
		}
		#endregion

		#region OnRowEditing
		/// <summary>
		/// Spou�t� ud�lost RowEditing.
		/// </summary>
		/// <remarks>Implementace zaji��uje nastaven� edit-��dku.</remarks>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowEditing(GridViewEditEventArgs e)
		{
			base.OnRowEditing(e);

			if (!e.Cancel)
			{
				this.EditIndex = e.NewEditIndex;
				if ((AllowInserting) && (this.InsertRowDataSourceIndex >= 0) && (this.insertRowIndex < e.NewEditIndex))
				{
					this.EditIndex = this.EditIndex - 1;
					SetRequiresDatabinding();
				}
				this.InsertRowDataSourceIndex = -1;
			}
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
				this.SetRequiresDatabinding();
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

		#region OnRowDeleting
		/// <summary>
		/// Spou�t� ud�lost RowDeleting.
		/// </summary>
		/// <remarks>
		/// Implementace vyp�n� editaci.
		/// </remarks>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnRowDeleting(GridViewDeleteEventArgs e)
		{
			base.OnRowDeleting(e);

			if (!e.Cancel)
			{
				this.EditIndex = -1;
				this.SetRequiresDatabinding();
			}
		}
		#endregion

		#region OnRowInserting, OnRowInserted
		/// <summary>
		/// Spou�t� ud�lost RowInserting.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected virtual void OnRowInserting(GridViewInsertEventArgs e)
		{
			GridViewInsertEventHandler h = (GridViewInsertEventHandler)base.Events[EventItemInserting];
			if (h != null)
			{
				h(this, e);
			}
		}

		/// <summary>
		/// Spou�t� ud�lost RowInserted.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected virtual void OnRowInserted(GridViewInsertedEventArgs e)
		{
			GridViewInsertedEventHandler h = (GridViewInsertedEventHandler)base.Events[EventItemInserted];
			if (h != null)
			{
				h(this, e);
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

	/// <summary>
	/// Reprezentuje metodu, kter� obsluhuje ud�lost RowInserting controlu GridViewExt.
	/// </summary>
	/// <param name="sender">odes�latel ud�losti (GridView)</param>
	/// <param name="e">argumenty ud�losti</param>
	public delegate void GridViewInsertEventHandler(object sender, GridViewInsertEventArgs e);

	/// <summary>
	/// Reprezentuje metodu, kter� obsluhuje ud�lost RowInserted controlu GridViewExt.
	/// </summary>
	/// <param name="sender">odes�latel ud�losti (GridView)</param>
	/// <param name="e">argumenty ud�losti</param>
	public delegate void GridViewInsertedEventHandler(object sender, GridViewInsertedEventArgs e);


	/// <summary>
	/// Deleg�t k metod� pro z�sk�v�n� data-item pro nov� Insert ��dek GridView.
	/// </summary>
	/// <returns></returns>
	public delegate object GetInsertRowDataItemDelegate();
}

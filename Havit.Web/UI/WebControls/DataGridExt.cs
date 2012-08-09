using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// DataGrid roz���en� o dal�� funkcionalitu:
	/// </summary>
	public class DataGridExt : System.Web.UI.WebControls.DataGrid
	{
		#region Data members (podle abecedy)
		/// <summary>
		/// Povoluje/zakazuje automatick� zpracov�n� ud�losti <see cref="DataGrid.CancelCommand"/>.
		/// </summary>
		/// <value><b>true</b>, pokud m� b�t pou�it <see cref="AutoCancelCommandHandler"/>, jinak <b>false</b>; Default je <b>true</b>.</value>
		public bool AutoHandleCancelCommand
		{
			get
			{
				object tmp = ViewState["AutoHandleCancelCommand"];
				if (tmp != null)
				{
					return (bool)tmp;
				}
				return true;
			}
			set
			{
				ViewState["AutoHandleCancelCommand"] = value;
			}
		}

		/// <summary>
		/// Povoluje/zakazuje automatick� zpracov�n� ud�losti <see cref="DataGrid.EditCommand"/>.
		/// </summary>
		/// <value><b>true</b>, pokud m� b�t pou�it <see cref="AutoEditCommandHandler"/>, jinak <b>false</b>; Default je <b>true</b>.</value>
		public bool AutoHandleEditCommand
		{
			get
			{
				object obj = ViewState["AutoHandleEditCommand"];
				if (obj != null)
				{
					return (bool)obj;
				}
				return true;
			}
			set
			{
				ViewState["AutoHandleEditCommand"] = value;
			}
		}

		/// <summary>
		/// Povoluje/zakazuje automatick� zpracov�n� ud�losti <see cref="DataGrid.PageIndexChanged"/>.
		/// </summary>
		/// <value><b>true</b>, pokud m� b�t pou�it <see cref="AutoPageIndexChangedHandler"/>, jinak <b>false</b>; Default je <b>true</b>.</value>
		public bool AutoHandlePageIndexChanged
		{
			get
			{
				object obj = ViewState["AutoHandlePageIndexChanged"];
				if (obj != null)
				{
					return (bool)obj;
				}
				return true;
			}
			set
			{
				ViewState["AutoHandlePageIndexChanged"] = value;
			}
		}

		/// <summary>
		/// Povoluje/zakazuje automatick� zpracov�n� ud�losti <see cref="DataGrid.SortCommand"/> ve vztahu k <see cref="SortExpression"/>.
		/// </summary>
		/// <value><b>true</b>, pokud m� b�t pou�it <see cref="AutoCancelCommandHandler"/>, jinak <b>false</b>; Default je <b>true</b>.</value>
		public bool AutoHandleSortCommand
		{
			get
			{
				object obj = ViewState["AutoHandleSortCommand"];
				if (obj != null)
				{
					return (bool)obj;
				}
				return true;
			}
			set
			{
				ViewState["AutoHandleSortCommand"] = value;
			}
		}

		/// <summary>
		/// SortExpression do DataView nebo SQL.
		/// M�n� se SortCommandem, ukl�d� do ViewState.
		/// Pro obousm�n� sort�n� je pot�eba povolit ViewState !!!
		/// </summary>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public string SortExpression
		{
			get
			{
				string str = (string)ViewState["SortExpression"];
				if (str != null)
				{
					return str;
				}
				return String.Empty;
			}
			set
			{
				ViewState["SortExpression"] = value;
			}
		}

		/// <summary>
		/// CssClass symbolu sort�n� do Header sloupce.
		/// </summary>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public string SortHeaderImageCssClass
		{
			get
			{
				string str = (string)ViewState["SortHeaderImageCssClass"];
				if (str != null)
				{
					return str;
				}
				return String.Empty;
			}
			set
			{
				ViewState["SortHeaderImageCssClass"] = value;
			}
		}

		/// <summary>
		/// URL obr�zku symbolu vzestupn�ho sort�n� do Header sloupce.
		/// </summary>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public string SortHeaderImageUrlAsc
		{
			get
			{
				string str = (string)ViewState["SortImageUrlAsc"];
				if (str != null)
				{
					return str;
				}
				return String.Empty;
			}
			set
			{
				ViewState["SortImageUrlAsc"] = value;
			}
		}

		/// <summary>
		/// URL obr�zku symbolu sestupn�ho sort�n� do Header sloupce.
		/// </summary>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public string SortHeaderImageUrlDesc
		{
			get
			{
				string str = (string)ViewState["SortHeaderImageUrlDesc"];
				if (str != null)
				{
					return str;
				}
				return String.Empty;
			}
			set
			{
				ViewState["SortHeaderImageUrlDesc"] = value;
			}
		}

		/// <summary>
		/// Povoluje/zakazuje validaci na p��kazu Update 
		/// </summary>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public bool UpdateCausesValidation
		{
			get
			{
				object obj = ViewState["CausesValidation"];
				if (obj != null)
				{
					return (bool)obj;
				}
				return true;
			}
			set
			{
				ViewState["CausesValidation"] = value;
			}
		}

		#endregion

		#region DataBindRequest (event)
		/// <summary>
		/// Vyskytne se, kdy� DataGridExt pot�ebuje p�ebindovat data.
		/// </summary>
		/// <remarks>
		/// Ud�lost je vyvol�v�na smart-extenzemi v situaci, kdy je pot�eba na DataGrid znovu nabindovat data.
		/// Nap�. po standardn� obsluze ud�lost� EditCommand, CancelCommand, PageIndexChanged, atp.
		/// </remarks>
		public event EventHandler DataBindRequest;

		/// <summary>
		/// Vyvol� ud�lost <see cref="Havit.Web.UI.WebControls.DataGridExt.DataBindRequest"/>.
		/// </summary>
		/// <param name="e">data ud�losti</param>
		protected virtual void OnDataBindRequest(EventArgs e)
		{
			if (this.DataBindRequest != null)
			{
				this.DataBindRequest(this, e);
			}
		}
		#endregion

		#region OnSortCommand
		/// <summary>
		/// Default implementace sort�n�.
		/// P�i zapnut�m ViewStatu zaji��uje obousm�rn� p�ep�n�n� SortExpression.
		/// P�i vypnut�m ViewStatu zaji��uje pouze b�n� p�ep�n�n� SortExpression.
		/// Lze zapnout/vypnout pomoc� AutoHandleSortCommand.
		/// </summary>
		/// <param name="e">DataGirdSortCommandEventArgs</param>
		protected override void OnSortCommand(DataGridSortCommandEventArgs e)
		{
			if (this.AutoHandleSortCommand)
			{
				this.AutoSortCommandHandler(e);
			}
			base.OnSortCommand(e);
		}

		/// <summary>
		/// Automatick� handler SortCommandu.
		/// M��e b�t p�eps�n v dce�inn� implementaci.
		/// </summary>
		/// <param name="e">args</param>
		protected virtual void AutoSortCommandHandler(DataGridSortCommandEventArgs e)
		{
			string[] previousSorts = this.SortExpression.Split(',');
			string[] wantedSorts = e.SortExpression.Split(',');

			if (wantedSorts.Length == 0)
			{
				this.SortExpression = String.Empty;
			}
			else if ((previousSorts.Length != wantedSorts.Length)
				|| (!this.EnableViewState)	// nen�-li pou�iteln� ViewState, funguje jen jednosm�rn� sort�n�
				|| (!Page.EnableViewState)	// nen� z�ejm�, co se stane, kdy� DG bude jako child-control s vypnut�m ViewState
				|| (this.SortExpression == null)
				|| (this.SortExpression.Length == 0)) // r�zn� po�et znamen� r�zn� sort�n�
			{
				// jednosm�rn� sort�n�
				this.SortExpression = e.SortExpression;
			}
			else
			{
				// obousm�rn� sort�n�
				int length = wantedSorts.Length;
				for (int i = 0; i < length; i++) // porovn�n� slo�ek
				{
					string wpart = wantedSorts[i].ToLower();
					if (wpart.Trim().IndexOf(' ') != -1)
					{
						wpart = wpart.Substring(0, wpart.Trim().IndexOf(' '));
					}
					if (!previousSorts[i].ToLower().StartsWith(wpart))
					{
						this.SortExpression = e.SortExpression;
						return;
					}
				}
				this.SortExpression = String.Empty;
				foreach (string part in previousSorts)
				{
					string newPart = part;
					if (newPart.IndexOf(' ') < 0)
					{
						newPart = newPart + " asc";
					}
					newPart = newPart.Replace("desc", "!a!");
					newPart = newPart.Replace("asc", "desc");
					newPart = newPart.Replace("!a!", "asc");
					this.SortExpression = this.SortExpression + newPart + ",";
				}
				this.SortExpression = this.SortExpression.Trim(',');
			}

			this.OnDataBindRequest(EventArgs.Empty);
		}
		#endregion

		#region OnPageIndexChanged
		/// <summary>
		/// Zaji��uje vol�n� ud�losti PageIndexChanged.
		/// P�i povolen�m AutoHandlePageIndexChanged nejprve zavol� AutoPageIndexChangedHandler.
		/// </summary>
		/// <param name="e">DataGridPageChangedEventArgs</param>
		protected override void OnPageIndexChanged(DataGridPageChangedEventArgs e)
		{
			if (this.AutoHandlePageIndexChanged)
			{
				AutoPageIndexChangedHandler(e);
			}
			base.OnPageIndexChanged (e);
		}

		/// <summary>
		/// Automatick� handler - nastavuje this.CurrentPageIndex = e.NewPageIndex a DataBind().
		/// Lze p�epsat v odvozen�m controlu.
		/// </summary>
		/// <param name="e">DataGridPageChangedEventArgs</param>
		protected virtual void AutoPageIndexChangedHandler(DataGridPageChangedEventArgs e)
		{
			this.CurrentPageIndex = e.NewPageIndex;
			this.OnDataBindRequest(EventArgs.Empty);
		}
		#endregion

		#region OnItemCreated
		/// <summary>
		/// Zaji��uje vol�n� ud�losti ItemCreated.
		/// Implementace p�id�v� do Headeru symboly sm�ru sort�n�.
		/// </summary>
		/// <param name="e">args</param>
		protected override void OnItemCreated(DataGridItemEventArgs e)
		{
			DataGridItem item = e.Item;

			base.OnItemCreated (e);

			// vypnut� CausesValidation u Update p��kazu
			if ((e.Item.ItemIndex == this.EditItemIndex) && (item.HasControls()))
			{
				for (int i = 0; i < item.Cells.Count; i++)
				{
					foreach (Control ctrl in item.Cells[i].Controls)
					{
						if ((ctrl is LinkButton) && (((LinkButton)ctrl).CommandName == DataGrid.UpdateCommandName))
						{
							((LinkButton)ctrl).CausesValidation = this.UpdateCausesValidation;
						}
						else if ((ctrl is Button) && (((Button)ctrl).CommandName == DataGrid.UpdateCommandName))
						{
							((Button)ctrl).CausesValidation = this.UpdateCausesValidation;
						}
					}
				}
			}

			// SortImage v Headeru
			if ((this.AllowSorting == true) && (item.ItemType == ListItemType.Header))
			{
				string se = this.SortExpression.ToLower().Replace("asc", "").Replace("desc", "").Replace(" ","");
				int colnum = -1;
				for (int i = 0; i < this.Columns.Count; i++)
				{
					if (this.Columns[i].SortExpression.ToLower().Replace("asc", "").Replace("desc", "").Replace(" ", "") == se)
					{
						colnum = i;
					}
				}
				if (colnum >= 0)
				{
					int iasc = this.SortExpression.ToLower().IndexOf(" asc");
					int idesc = this.SortExpression.ToLower().IndexOf(" desc");

					if ( (idesc > 0) && ((iasc < 0) || (idesc < iasc)) )
					{
						if ((this.SortHeaderImageUrlDesc != null) && (this.SortHeaderImageUrlDesc.Length > 0))
						{
							HtmlImage img = new HtmlImage();
							img.ID = "SortImg";
							img.Src = this.SortHeaderImageUrlDesc;
							img.Attributes["class"] = this.SortHeaderImageCssClass;
							img.Alt = "A";
							item.Cells[colnum].Controls.Add(img);
						}
					}
					else
					{
						if ((this.SortHeaderImageUrlAsc != null) && (this.SortHeaderImageUrlAsc.Length > 0))
						{
							HtmlImage img = new HtmlImage();
							img.ID = "SortImg";
							img.Src = this.SortHeaderImageUrlAsc;
							img.Attributes["class"] = this.SortHeaderImageCssClass;
							img.Alt = "A";
							item.Cells[colnum].Controls.Add(img);
						}
					}
				}
			}
		}
		#endregion

		#region OnEditCommand
		/// <summary>
		/// Zajist� vol�n� obsluhy ud�losti EditCommand.
		/// Pokud je AutoHandleEditCommand true, provede default obsluhu AutoEditCommandHandler().
		/// </summary>
		/// <param name="e">DataGridCommandEventArgs</param>
		protected override void OnEditCommand(DataGridCommandEventArgs e)
		{
			if (this.AutoHandleEditCommand)
			{
				AutoEditCommandHandler(e);
			}
			base.OnEditCommand (e);
		}

		/// <summary>
		/// Default obsluha EditCommand ud�losti.
		/// </summary>
		/// <param name="e">DataGridCommandEventArgs</param>
		protected virtual void AutoEditCommandHandler(DataGridCommandEventArgs e)
		{
			this.EditItemIndex = e.Item.ItemIndex;
			this.OnDataBindRequest(EventArgs.Empty);
		}
		#endregion

		#region OnCancelCommand
		/// <summary>
		/// Zajist� vol�n� obsluhy ud�losti CancelCommand.
		/// Pokud je AutoHandleCancelCommand true, provede default obsluhu AutoCancelCommandHandler().
		/// </summary>
		/// <param name="e">DataGridCommandEventArgs</param>
		protected override void OnCancelCommand(DataGridCommandEventArgs e)
		{
			if (this.AutoHandleCancelCommand)
			{
				AutoCancelCommandHandler(e);
			}
			base.OnCancelCommand(e);
		}

		/// <summary>
		/// Default obsluha EditCommand ud�losti.
		/// </summary>
		/// <param name="e">DataGridCommandEventArgs</param>
		protected virtual void AutoCancelCommandHandler(DataGridCommandEventArgs e)
		{
			this.EditItemIndex = -1;
			this.OnDataBindRequest(EventArgs.Empty);
		}
		#endregion
	}
}
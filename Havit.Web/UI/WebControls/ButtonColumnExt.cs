using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Roz���en� <see cref="System.Web.UI.WebControls.ButtonColumn"/> k <see cref="System.Web.UI.WebControls.DataGrid"/>u,
	/// nap�. o confirmation, zap�n�n�/vyp�n�n� validace, atp.
	/// </summary>
	public class ButtonColumnExt : System.Web.UI.WebControls.ButtonColumn
	{
		#region Data Members (abecedn�)
		/// <summary>
		/// Vr�t�/nastav� hodnotu indikuj�c�, zda-li se m� p�i kliknut� na tla��tko prov�st validace.
		/// </summary>
		/// <value>Default hodnota je <b>false</b>!!! Aby bylo chov�n� stejn� jako <see cref="ButtonColumn"/></value>
		public bool CausesValidation
		{
			get
			{
				object obj = ViewState["CausesValidation"]; 
				if (obj != null)
				{
					return (bool)obj;
				}
				return false;
			}
			set
			{
				ViewState["CausesValidation"] = value; 
				this.OnColumnChanged();
			}
		}

		/// <summary>
		/// Vr�t�/nastav� statick� text, kter� se m� zobrazovat do confirmation dialogu p�i kliknut� tla��tka.
		/// </summary>
		/// <remarks>
		/// Pro dynamick� text lze pou��t t� <see cref="ConfirmationDataField"/> a <see cref="ConfirmationDataFormatString"/>.
		/// </remarks>
		public string ConfirmationText
		{
			get
			{
				string text = (string)ViewState["ConfirmationText"]; 
				if (text != null)
				{
					return text;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ConfirmationText"] = value;
				this.OnColumnChanged();
			}
		}

		/// <summary>
		/// Vr�t�/nastav� n�zev pole z DataSource, kter� m� b�t nabindovan� do confirmation dialogu.
		/// </summary>
		/// <remarks>
		/// Form�tov�n� se nastavuje pomoc� <see cref="ConfirmationDataFormatString"/>.
		/// </remarks>
		public string ConfirmationDataField
		{
			get
			{
				string text = (string)ViewState["ConfirmationTextField"]; 
				if (text != null)
				{
					return text;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ConfirmationTextField"] = value;
				this.OnColumnChanged();
			}
		}

		/// <summary>
		/// Vr�t�/nastav� FormatString dat bindovan�ch do confirmation dialogu.
		/// </summary>
		/// <remarks>
		/// Form�tuje <see cref="ConfirmationDataField"/>.
		/// Pokud chceme zobrazit pouze statick� text, pou�ije se <see cref="ConfirmationText"/>.
		/// </remarks>
		/// <value>Default je <see cref="String.Empty"/>.</value>
		public string ConfirmationDataFormatString
		{
			get
			{
				string text = (string)ViewState["ConfirmationTextFormatString"]; 
				if (text != null)
				{
					return text;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ConfirmationTextFormatString"] = value;
				this.OnColumnChanged();
			}
		}

		#endregion

		#region InitializeCell
		/// <summary>
		/// Resets a cell in the ButtonColumn to its initial state.
		/// </summary>
		/// <param name="cell">A <see cref="TableCell"/> that represents the cell to reset.</param>
		/// <param name="columnIndex">The column number where the cell is located.</param>
		/// <param name="itemType">One of the <see cref="ListItemType"/> values.</param>
		public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{
			base.InitializeCell (cell, columnIndex, itemType);

			// v headeru a footeru tla��tko nen� !!!
			if ((itemType != ListItemType.Header) && (itemType != ListItemType.Footer))
			{
				// button je podle SDK v�dy nult� control
				WebControl button = (WebControl)cell.Controls[0];

				// nastaven� CausesValidation
				if (button is Button)
				{
					((Button)button).CausesValidation = this.CausesValidation;
				}
				else
				{
					((LinkButton)button).CausesValidation = this.CausesValidation;
				}

				// DataBinding se mus� d�lat v DataBinding ud�losti, proto�e nyn� nem�me p��stup k Item prvku (dat�m)
				// tam si do�e��me t�eba ConfirmationDialog
				button.DataBinding += new EventHandler(this.OnDataBindColumn);
			}
		}
		#endregion

		#region OnDataBindColumn
		private PropertyDescriptor _confirmationFieldDescriptor;

		/// <summary>
		/// Zajist� nav�z�n� dat na tla��tko.
		/// </summary>
		/// <param name="sender">sender.Namingcontainer je <see cref="DataGridItem"/></param>
		/// <param name="e">pr�zdn�</param>
		private void OnDataBindColumn(object sender, EventArgs e)
		{
			WebControl control = (WebControl)sender;
			DataGridItem item = (DataGridItem)control.NamingContainer;
			object dataItem = item.DataItem;

			// nastaven� Confirmation (inspirov�no nastavov�n�m pole Text z Reflectoru)
			string confirmationText = this.ConfirmationText;
			if (this.ConfirmationDataField.Length > 0)
			{
				if (this._confirmationFieldDescriptor == null)
				{
					string confirmationDataField = this.ConfirmationDataField;
					this._confirmationFieldDescriptor = TypeDescriptor.GetProperties(dataItem).Find(confirmationDataField, true);
					if (this._confirmationFieldDescriptor == null)
					{
						throw new HttpException("Field Not Found - " + confirmationDataField);
					}
				}
				if (this._confirmationFieldDescriptor != null)
				{
					object confirmationDataValue = this._confirmationFieldDescriptor.GetValue(dataItem);
					if ((confirmationDataValue != null) && (confirmationDataValue != DBNull.Value))
					{
						if (this.ConfirmationDataFormatString.Length > 0)
						{
							confirmationText = String.Format(this.ConfirmationDataFormatString, confirmationDataValue);
						}
						else
						{
							confirmationText = confirmationDataValue.ToString();
						}
					}
				}
			}
			if (confirmationText.Length > 0)
			{
				control.Attributes.Add("onClick", "return confirm('" + confirmationText + "');");
			}
		}
		#endregion
	}
}

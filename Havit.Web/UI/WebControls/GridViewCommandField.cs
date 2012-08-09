using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Globalization;
using System.Diagnostics;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Vylep�en� command-field ur�en� pro pou�it� s GridViewExt.
	/// </summary>
	/// <remarks>
	/// GridViewCommandField lze skinovat, pokud rodi�ovsk� GridView implementuje rozhran� <see cref="ICommandFieldStyle"/>,
	/// co� implementuje nap��klad <see cref="GridViewExt"/>.
	/// </remarks>
	/// <example>
	/// Skinovat lze pomoc�:
	/// <code>
	/// &lt;havit:GridViewExt ... &gt;
	///     &lt;CommandFieldStyle ButtonType=&quot;Image&quot; ... /&gt;
	/// &lt;/havit:GridViewExt&gt;
	/// </code>
	/// </example>
	public class GridViewCommandField : CommandFieldExt
	{
		#region DeleteConfirmationText
		/// <summary>
		/// Text, na kter� se m� pt�t jscript:confirm() p�ed smaz�n�m z�znamu. Pokud je pr�zdn�, na nic se nept�.
		/// </summary>
		public string DeleteConfirmationText
		{
			get
			{
				object tmp = ViewState["DeleteConfirmationText"];
				if (tmp != null)
				{
					return (string)tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["DeleteConfirmationText"] = value;
			}
		}
		#endregion

		#region Initialize
		/// <summary>
		/// Inicializuje field (vol� se jednou z GridView.CreateChildControls()).
		/// </summary>
		/// <param name="sortingEnabled">indikuje, zda-li je povolen� sorting</param>
		/// <param name="control">parent control (GridView)</param>
		/// <returns>false (v�dy)</returns>
		public override bool Initialize(bool sortingEnabled, System.Web.UI.Control control)
		{
			if (control is ICommandFieldStyle)
			{
				ApplyStyle(((ICommandFieldStyle)control).CommandFieldStyle, !String.IsNullOrEmpty(control.Page.Theme), !String.IsNullOrEmpty(control.Page.StyleSheetTheme));
			}

			return base.Initialize(sortingEnabled, control);
		}
		#endregion

		#region InitializeCell
		/// <summary>
		/// Inicializuje bu�ku.
		/// </summary>
		public override void InitializeCell(
			DataControlFieldCell cell,
			DataControlCellType cellType,
			DataControlRowState rowState,
			int rowIndex)
		{
			if (cellType != DataControlCellType.DataCell)
			{
				// Header a Footer �e�� korektn� ButtonFieldBase, p�edek CommandFieldu
				base.InitializeCell(cell, cellType, rowState, rowIndex);
			}
			else
			{
				bool showEditButton = this.ShowEditButton;
				bool showDeleteButton = this.ShowDeleteButton;
				bool showInsertButton = this.ShowInsertButton;
				bool showSelectButton = this.ShowSelectButton;
				bool showCancelButton = this.ShowCancelButton;
				bool insertSpace = true;
				bool causesValidation = this.CausesValidation;
				string validationGroup = this.ValidationGroup;

				if (cellType == DataControlCellType.DataCell)
				{
					LiteralControl child;
					if ((rowState & (DataControlRowState.Insert | DataControlRowState.Edit)) != DataControlRowState.Normal)
					{
						if (((rowState & DataControlRowState.Edit) != DataControlRowState.Normal) && showEditButton)
						{
							// stejn� jako CommandField
							this.AddButtonToCell(cell, "Update", this.UpdateText, causesValidation, validationGroup, rowIndex, this.UpdateImageUrl);
							if (showCancelButton)
							{
								child = new LiteralControl("&nbsp;");
								cell.Controls.Add(child);
								this.AddButtonToCell(cell, "Cancel", this.CancelText, false, string.Empty, rowIndex, this.CancelImageUrl);
							}
						}
						if (((rowState & DataControlRowState.Insert) != DataControlRowState.Normal) && showInsertButton)
						{
							// Nechceme Cancel
							this.AddButtonToCell(cell, "Insert", this.InsertText, causesValidation, validationGroup, rowIndex, this.InsertImageUrl);
							/*
							if (showCancelButton)
							{
								child = new LiteralControl("&nbsp;");
								cell.Controls.Add(child);
								this.AddButtonToCell(cell, "Cancel", this.CancelText, false, string.Empty, rowIndex, this.CancelImageUrl);
							}
							 */
						}
					}
					else
					{
						if (showEditButton)
						{
							this.AddButtonToCell(cell, "Edit", this.EditText, false, string.Empty, rowIndex, this.EditImageUrl);
							insertSpace = false;
						}
						if (showDeleteButton)
						{
							if (!insertSpace)
							{
								child = new LiteralControl("&nbsp;");
								cell.Controls.Add(child);
							}
							
							IButtonControl button = this.AddButtonToCell(cell, "Delete", this.DeleteText, false, string.Empty, rowIndex, this.DeleteImageUrl);

							// doplneni o DeleteConfirmText
							if (!String.IsNullOrEmpty(DeleteConfirmationText))
							{
								if (button is Button)
								{
									((Button)button).OnClientClick = String.Format("if (!confirm('{0}')) return false;", DeleteConfirmationText.Replace("'", "''"));
								}
								else if (button is LinkButton)
								{
									((LinkButton)button).OnClientClick = String.Format("if (!confirm('{0}')) return false;", DeleteConfirmationText.Replace("'", "''"));
								}
								else
								{
									Debug.Assert(button is ImageButton);
									((ImageButton)button).OnClientClick = String.Format("if (!confirm('{0}')) return false;", DeleteConfirmationText.Replace("'", "''"));
								}
							}

							insertSpace = false;
						}
						// U Insertu nechceme New
						/*
						if (showInsertButton)
						{
							if (!flag6)
							{
								child = new LiteralControl("&nbsp;");
								cell.Controls.Add(child);
							}
							this.AddButtonToCell(cell, "New", this.NewText, false, string.Empty, rowIndex, this.NewImageUrl);
							flag6 = false;
						}
						*/
						if (showSelectButton)
						{
							if (!insertSpace)
							{
								child = new LiteralControl("&nbsp;");
								cell.Controls.Add(child);
							}
							this.AddButtonToCell(cell, "Select", this.SelectText, false, string.Empty, rowIndex, this.SelectImageUrl);
							insertSpace = false;
						}
					}
				}
			}
		}
		#endregion

		#region AddButtonToCell
		private IButtonControl AddButtonToCell(DataControlFieldCell cell, string commandName, string buttonText, bool causesValidation, string validationGroup, int rowIndex, string imageUrl)
		{
			IButtonControl control;
			IPostBackContainer container = base.Control as IPostBackContainer;
			bool flag = true;
			switch (this.ButtonType)
			{
				case ButtonType.Button:
					if ((container == null) || causesValidation)
					{
						control = new Button();
					}
					else
					{
						control = new DataControlButtonExt(container);
						flag = false;
					}
					break;

				case ButtonType.Link:
					if ((container == null) || causesValidation)
					{
						control = new DataControlLinkButtonExt(null);
					}
					else
					{
						control = new DataControlLinkButtonExt(container);
						flag = false;
					}
					break;

				default:
					if ((container != null) && !causesValidation)
					{
						control = new DataControlImageButtonExt(container);
						flag = false;
					}
					else
					{
						control = new ImageButton();
					}
					((ImageButton)control).ImageUrl = imageUrl;
					break;
			}
			control.Text = buttonText;
			control.CommandName = commandName;
			control.CommandArgument = rowIndex.ToString(CultureInfo.InvariantCulture);
			if (flag)
			{
				control.CausesValidation = causesValidation;
			}
			control.ValidationGroup = validationGroup;
			cell.Controls.Add((WebControl)control);
			return control;
		}
		#endregion

		#region ApplyStyle
		/// <summary>
		/// Aplikuje CommandFieldStyle na field.
		/// </summary>
		/// <param name="style">styl k aplikov�n�</param>
		/// <param name="theme">re�im Theme (p�epsat v�e) zapnut</param>
		/// <param name="styleSheetTheme">re�im StyleSheetTheme (lok�ln� nastaven� maj� prioritu) zapnut</param>
		private void ApplyStyle(CommandFieldStyle style, bool theme, bool styleSheetTheme)
		{
			if (style != null)
			{
				if (styleSheetTheme)
				{
					// pokud sami nastaveni nejsme (ViewState je nepou�it�), pak vol�me styl

					if (ViewState["AccessibleHeaderText"] == null)
					{
						this.AccessibleHeaderText = style.AccessibleHeaderText;
					}
					if (ViewState["ButtonType"] == null)
					{
						this.ButtonType = style.ButtonType;
					}
					if (ViewState["CancelImageUrl"] == null)
					{
						this.CancelImageUrl = style.CancelImageUrl;
					}
					if (ViewState["CancelText"] == null)
					{
						this.CancelText = style.CancelText;
					}
					if (ViewState["CausesValidation"] == null)
					{
						this.CausesValidation = style.CausesValidation;
					}
					if (this.ControlStyle.IsEmpty)
					{
						this.ControlStyle.CopyFrom(style.ControlStyle);
					}
					if (ViewState["DeleteConfirmationText"] == null)
					{
						this.DeleteConfirmationText = style.DeleteConfirmationText;
					}
					if (ViewState["DeleteImageUrl"] == null)
					{
						this.DeleteImageUrl = style.DeleteImageUrl;
					}
					if (ViewState["DeleteText"] == null)
					{
						this.DeleteText = style.DeleteText;
					}
					if (ViewState["EditImageUrl"] == null)
					{
						this.EditImageUrl = style.EditImageUrl;
					}
					if (ViewState["EditText"] == null)
					{
						this.EditText = style.EditText;
					}
					if (this.FooterStyle.IsEmpty)
					{
						this.FooterStyle.CopyFrom(style.FooterStyle);
					}
					if (ViewState["FooterText"] == null)
					{
						this.FooterText = style.FooterText;
					}
					if (ViewState["HeaderImageUrl"] == null)
					{
						this.HeaderImageUrl = style.HeaderImageUrl;
					}
					if (ViewState["HeaderStyle.IsEmpty"] == null)
					{
						this.HeaderStyle.CopyFrom(style.HeaderStyle);
					}
					if (ViewState["HeaderText"] == null)
					{
						this.HeaderText = style.HeaderText;
					}
					if (ViewState["InsertImageUrl"] == null)
					{
						this.InsertImageUrl = style.InsertImageUrl;
					}
					if (ViewState["InsertText"] == null)
					{
						this.InsertText = style.InsertText;
					}
					if (this.ItemStyle.IsEmpty)
					{
						this.ItemStyle.CopyFrom(style.ItemStyle);
					}
					if (ViewState["NewImageUrl"] == null)
					{
						this.NewImageUrl = style.NewImageUrl;
					}
					if (ViewState["NewText"] == null)
					{
						this.NewText = style.NewText;
					}
					if (ViewState["SelectImageUrl"] == null)
					{
						this.SelectImageUrl = style.SelectImageUrl;
					}
					if (ViewState["SelectText"] == null)
					{
						this.SelectText = style.SelectText;
					}
					if (ViewState["ShowCancelButton"] == null)
					{
						this.ShowCancelButton = style.ShowCancelButton;
					}
					if (ViewState["ShowDeleteButton"] == null)
					{
						this.ShowDeleteButton = style.ShowDeleteButton;
					}
					if (ViewState["ShowEditButton"] == null)
					{
						this.ShowEditButton = style.ShowEditButton;
					}
					if (ViewState["ShowHeader"] == null)
					{
						this.ShowHeader = style.ShowHeader;
					}
					if (ViewState["ShowInsertButton"] == null)
					{
						this.ShowInsertButton = style.ShowInsertButton;
					}
					if (ViewState["ShowSelectButton"] == null)
					{
						this.ShowSelectButton = style.ShowSelectButton;
					}
					if (ViewState["UpdateImageUrl"] == null)
					{
						this.UpdateImageUrl = style.UpdateImageUrl;
					}
					if (ViewState["UpdateText"] == null)
					{
						this.UpdateText = style.UpdateText;
					}
					if (ViewState["ValidationGroup"] == null)
					{
						this.ValidationGroup = style.ValidationGroup;
					}
					if (ViewState["Visible"] == null)
					{
						this.Visible = style.Visible;
					}
				}

				if (theme)
				{
					// pokud je nastaven skin, pak vol�me v�dy styl

					if (style.ViewState["AccessibleHeaderText"] != null)
					{
						this.AccessibleHeaderText = style.AccessibleHeaderText;
					}
					if (style.ViewState["ButtonType"] != null)
					{
						this.ButtonType = style.ButtonType;
					}
					if (style.ViewState["CancelImageUrl"] != null)
					{
						this.CancelImageUrl = style.CancelImageUrl;
					}
					if (style.ViewState["CancelText"] != null)
					{
						this.CancelText = style.CancelText;
					}
					if (style.ViewState["CausesValidation"] != null)
					{
						this.CausesValidation = style.CausesValidation;
					}
					if (this.ControlStyle.IsEmpty)
					{
						this.ControlStyle.CopyFrom(style.ControlStyle);
					}
					if (style.ViewState["DeleteConfirmationText"] != null)
					{
						this.DeleteConfirmationText = style.DeleteConfirmationText;
					}
					if (style.ViewState["DeleteImageUrl"] != null)
					{
						this.DeleteImageUrl = style.DeleteImageUrl;
					}
					if (style.ViewState["DeleteText"] != null)
					{
						this.DeleteText = style.DeleteText;
					}
					if (style.ViewState["EditImageUrl"] != null)
					{
						this.EditImageUrl = style.EditImageUrl;
					}
					if (style.ViewState["EditText"] != null)
					{
						this.EditText = style.EditText;
					}
					if (this.FooterStyle.IsEmpty)
					{
						this.FooterStyle.CopyFrom(style.FooterStyle);
					}
					if (style.ViewState["FooterText"] != null)
					{
						this.FooterText = style.FooterText;
					}
					if (style.ViewState["HeaderImageUrl"] != null)
					{
						this.HeaderImageUrl = style.HeaderImageUrl;
					}
					if (style.ViewState["HeaderStyle.IsEmpty"] != null)
					{
						this.HeaderStyle.CopyFrom(style.HeaderStyle);
					}
					if (style.ViewState["HeaderText"] != null)
					{
						this.HeaderText = style.HeaderText;
					}
					if (style.ViewState["InsertImageUrl"] != null)
					{
						this.InsertImageUrl = style.InsertImageUrl;
					}
					if (style.ViewState["InsertText"] != null)
					{
						this.InsertText = style.InsertText;
					}
					if (this.ItemStyle.IsEmpty)
					{
						this.ItemStyle.CopyFrom(style.ItemStyle);
					}
					if (style.ViewState["NewImageUrl"] != null)
					{
						this.NewImageUrl = style.NewImageUrl;
					}
					if (style.ViewState["NewText"] != null)
					{
						this.NewText = style.NewText;
					}
					if (style.ViewState["SelectImageUrl"] != null)
					{
						this.SelectImageUrl = style.SelectImageUrl;
					}
					if (style.ViewState["SelectText"] != null)
					{
						this.SelectText = style.SelectText;
					}
					if (style.ViewState["ShowCancelButton"] != null)
					{
						this.ShowCancelButton = style.ShowCancelButton;
					}
					if (style.ViewState["ShowDeleteButton"] != null)
					{
						this.ShowDeleteButton = style.ShowDeleteButton;
					}
					if (style.ViewState["ShowEditButton"] != null)
					{
						this.ShowEditButton = style.ShowEditButton;
					}
					if (style.ViewState["ShowHeader"] != null)
					{
						this.ShowHeader = style.ShowHeader;
					}
					if (style.ViewState["ShowInsertButton"] != null)
					{
						this.ShowInsertButton = style.ShowInsertButton;
					}
					if (style.ViewState["ShowSelectButton"] != null)
					{
						this.ShowSelectButton = style.ShowSelectButton;
					}
					if (style.ViewState["UpdateImageUrl"] != null)
					{
						this.UpdateImageUrl = style.UpdateImageUrl;
					}
					if (style.ViewState["UpdateText"] != null)
					{
						this.UpdateText = style.UpdateText;
					}
					if (style.ViewState["ValidationGroup"] != null)
					{
						this.ValidationGroup = style.ValidationGroup;
					}
					if (style.ViewState["Visible"] != null)
					{
						this.Visible = style.Visible;
					}
				}
			}
		}
		#endregion
	}
}
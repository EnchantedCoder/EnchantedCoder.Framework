﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Sloupec pro heterogenní seznamy.
	/// </summary>
	public class BoundFieldExt : System.Web.UI.WebControls.BoundField, IIdentifiableField
	{
		#region ID (IIdentifiableField Members)
		/// <summary>
		/// Identifikátor fieldu na který se lze odkazovat pomocí <see cref="GridViewExt.FindColumn(string)"/>.
		/// </summary>
		public string ID
		{
			get
			{
				object tmp = ViewState["ID"];
				if (tmp != null)
				{
					return (string)tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ID"] = value;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Css třída vygenerované buňky.	
		/// Zamýšleno např. pro omezení šírky sloupce.
		/// Viz	<see cref="InitializeDataCell(System.Web.UI.WebControls.DataControlFieldCell, System.Web.UI.WebControls.DataControlRowState)" />.
		/// </summary>
		internal string CellCssClass
		{
#warning nevhodná implementace, předělat na ContentStyle typu Style (používá se na přidávání vnitřního obalového divu)
			get { return (string)(ViewState["CellCssClass"] ?? String.Empty); }
			set { ViewState["CellCssClass"] = value; }
		}

		/// <summary>
		/// Pokud se metodě GetValue nepodaří získat hodnotu z dat, použije se tato hodnota, pokud není null.
		/// Rozlišuje se null a prázdný řetězec.
		/// Viz metody GetNotFoundDataItem a GetValue.
		/// </summary>
		public string EmptyText
		{
			get
			{
				return (string)ViewState["EmptyText"];
			}
			set { ViewState["EmptyText"] = value; }
		}
		#endregion

		#region DataFormatString
		/// <summary>
		/// Gets or sets the string that specifies the display format for the value of the field.
		/// </summary>
		/// <remarks>
		/// Nastavením se přepne výchozí hodnota HtmlEncode na false.
		/// </remarks>
		public override string DataFormatString
		{
			get
			{
				return base.DataFormatString;
			}
			set
			{
				base.DataFormatString = value;
				
				// pokud není explicitně nastaveno HtmlEncode, pak ho vypneme
				if (ViewState["HtmlEncode"] == null)
				{
					this.HtmlEncode = false;
				}
			}
		}
		#endregion

		#region CreateField - override, v potomcích rovněž nutný override.
		/// <summary>
		/// Vyžadováno implementací Fieldu v .NETu. V potomcích nutno přepsat.
		/// </summary>
		/// <returns>Instance této třídy.</returns>
		protected override DataControlField CreateField()
		{
			return new Havit.Web.UI.WebControls.BoundFieldExt();
		}
		#endregion

		#region GetValue, FormatValue, GetNotFoundDataItem

		private static readonly char[] indexExprStartChars = new char[] { '[', '(' };

		/// <summary>
		/// Získá hodnotu pro zobrazení na základě datového zdroje a DataFieldu.
		/// </summary>
		/// <param name="controlContainer">Control container (řádek GridView), kterému se získává hodnota.</param>
		/// <returns></returns>
		protected override object GetValue(Control controlContainer)
		{
			if (controlContainer == null)
				throw new ArgumentNullException("controlContainer");

			object dataItem = DataBinder.GetDataItem(controlContainer);

			if (DesignMode)
				return GetDesignTimeValue();

			if (dataItem == null)
				throw new Exception("Nepodařilo se získat objekt s daty.");

			if (DataField == ThisExpression)
				return dataItem;

			object value = DataBinderExt.GetValue(dataItem, DataField);
			if ((value == null) || (value == DBNull.Value))
			{
				return GetNotFoundDataItem();
			}
			return value;
		}

		///// <summary>
		///// Získá hodnotu pro zobrazení z předaného objektu a dataField.
		///// </summary>
		///// <param name="dataItem">Položka dat z DataSource</param>
		///// <param name="dataField">DataField</param>
		///// <returns></returns>
		//protected object GetValue(object dataItem, string dataField)
		//{
		//    string[] expressionParts = dataField.Split('.');

		//    object currentValue = dataItem;

		//    int i = 0;
		//    int lastExpressionIndex = expressionParts.Length - 1;
		//    for (i = 0; i <= lastExpressionIndex; i++)
		//    {
		//        string expression = expressionParts[i];

		//        if (expression.IndexOfAny(indexExprStartChars) < 0)
		//        {
		//            currentValue = DataBinder.GetPropertyValue(currentValue, expression);
		//        }
		//        else
		//        {
		//            currentValue = DataBinder.GetIndexedPropertyValue(currentValue, expression);
		//        }

		//        if (currentValue == null) // && (i < lastExpressionIndex))
		//        {
		//            return GetNotFoundDataItem();
		//        }
		//    }

		//    return currentValue;
		//}

		/// <summary>
		/// Formátuje hodnotu k zobrazení.
		/// </summary>
		/// <param name="value">Data</param>
		/// <returns>Text k zobrazení.</returns>
		public virtual string FormatDataValue(object value)
		{
			// v tento okamžik je zde jako plán k přepsání (override)

			//if (NumberFormatter.IsNumber(value))
			//    return NumberFormatter.Format((IFormattable)value, DataFormatString);
			//else
			return base.FormatDataValue(value, SupportsHtmlEncode && HtmlEncode);
		}

		/// <summary>
		/// Metoda je volána, pokud se metodě GetValue nepodaří získat hodnotu.
		/// Není-li EmptyText rovno null, vrací se hodnota Empty text. 
		/// Jinak je vyhozena výjimka MissingMemberException.
		/// </summary>
		protected virtual object GetNotFoundDataItem()
		{
			if (EmptyText != null)
			{
				return EmptyText;
			}

			throw new InvalidOperationException(String.Format("Při zpracování hodnoty z DataFieldu \"{0}\" byla získána hodnota null nebo DBNull.Value, ale není nastavena hodnota vlastnosti EmptyText.", DataField));
		}
		#endregion

		#region InitializeDataCell, InitializeDataCellContent
		/// <summary>
		/// Pokud není CellCssClass prázdné, generuje se do buňky tabulky &lt;div="CellCssClass"&gt;...&lt;/div&gt;.
		/// Jinak se použije normálně samotná buňka tabulky.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="rowState"></param>
		protected override sealed void InitializeDataCell(System.Web.UI.WebControls.DataControlFieldCell cell, System.Web.UI.WebControls.DataControlRowState rowState)
		{
			if (!String.IsNullOrEmpty(CellCssClass))
			{
				Panel panel = new Panel();
				panel.CssClass = CellCssClass;
				cell.Controls.Add(panel);
				InitializeDataCellContent(panel, rowState);
			}
			else
			{
				InitializeDataCellContent(cell, rowState);
			}
		}

		/// <summary>
		/// Inicializuje obsah buňky daty.
		/// </summary>
		/// <param name="control">Control, do kterého se má obsah inicializovat.</param>
		/// <param name="rowState">RowState.</param>
		protected virtual void InitializeDataCellContent(Control control, DataControlRowState rowState)
		{			
			Literal literal = new Literal();
			control.Controls.Add(literal);
			literal.DataBinding += delegate(object sender, EventArgs e)
			{
				object value = GetValue(literal.NamingContainer);
				literal.Text = FormatDataValue(value);
			};
		}

		#endregion
	}
}
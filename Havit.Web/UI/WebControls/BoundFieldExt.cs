using System;
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
	/// Sloupec pro heterogenn� seznamy.
	/// </summary>
	public class BoundFieldExt : System.Web.UI.WebControls.BoundField, IEnterpriseField
	{
		#region IEnterpriseField Members
		/// <summary>
		/// ID sloupce.
		/// </summary>
		public string ID
		{
			get { return (string)(ViewState["ID"]); }
			set { ViewState["ID"] = value; }
		}
		#endregion

		#region Properties
		/// <summary>
		/// Css t��da vygenerovan� bu�ky.	
		/// Zam��leno nap�. pro omezen� ��rky sloupce.
		/// Viz	<see cref="InitializeDataCell(System.Web.UI.WebControls.DataControlFieldCell, System.Web.UI.WebControls.DataControlRowState)" />.
		/// </summary>
		public string CellCssClass
		{
			get { return (string)(ViewState["CellCssClass"] ?? String.Empty); }
			set { ViewState["CellCssClass"] = value; }
		}

		/// <summary>
		/// Pokud se metod� GetValue nepoda�� z�skat hodnotu z dat, pou�ije se tato hodnota, pokud nen� null.
		/// Rozli�uje se null a pr�zdn� �et�zec.
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

		#region CreateField - override, v potomc�ch rovn� nutn� override.
		/// <summary>
		/// Vy�adov�no implementac� Fieldu v .NETu. V potomc�ch nutno p�epsat.
		/// </summary>
		/// <returns>Instance t�to t��dy.</returns>
		protected override DataControlField CreateField()
		{
			return new Havit.Web.UI.WebControls.BoundFieldExt();
		}
		#endregion

		#region GetValue, FormatValue, GetNotFoundDataItem

		private static readonly char[] indexExprStartChars = new char[] { '[', '(' };

		/// <summary>
		/// Z�sk� hodnotu pro zobrazen� na z�klad� datov�ho zdroje a DataFieldu.
		/// </summary>
		/// <param name="dataItem">Polo�ka dat z DataSource</param>
		/// <returns></returns>
		protected override object GetValue(Control controlContainer)
		{
			if (controlContainer == null)
				throw new ArgumentNullException("controlContainer");

			object dataItem = DataBinder.GetDataItem(controlContainer);

			if (DesignMode)
				return GetDesignTimeValue();

			if (dataItem == null)
				throw new Exception("Nepoda�ilo se z�skat objekt s daty.");

			if (DataField == ThisExpression)
				return dataItem;

			return (GetValue(dataItem, DataField));
		}

		/// <summary>
		/// Z�sk� hodnotu pro zobrazen� z p�edan�ho objektu a dataField.
		/// </summary>
		/// <param name="dataItem">Polo�ka dat z DataSource</param>
		/// <param name="dataField">DataField</param>
		/// <returns></returns>
		protected object GetValue(object dataItem, string dataField)
		{
			string[] expressionParts = dataField.Split('.');

			object currentValue = dataItem;

			int i = 0;
			int lastExpressionIndex = expressionParts.Length - 1;
			for (i = 0; i <= lastExpressionIndex; i++)
			{
				string expression = expressionParts[i];

				if (expression.IndexOfAny(indexExprStartChars) < 0)
				{
					currentValue = DataBinder.GetPropertyValue(currentValue, expression);
				}
				else
				{
					currentValue = DataBinder.GetIndexedPropertyValue(currentValue, expression);
				}

				if ((currentValue == null) && (i < lastExpressionIndex))
					return GetNotFoundDataItem();
			}

			return currentValue;
		}

		/// <summary>
		/// Form�tuje hodnotu k zobrazen�.
		/// </summary>
		/// <param name="value">Data</param>
		/// <returns>Text k zobrazen�.</returns>
		public virtual string FormatDataValue(object value)
		{
			// v tento okam�ik je zde jako pl�n k p�eps�n� (override)

			//if (NumberFormatter.IsNumber(value))
			//    return NumberFormatter.Format((IFormattable)value, DataFormatString);
			//else
			return base.FormatDataValue(value, SupportsHtmlEncode && HtmlEncode);
		}

		/// <summary>
		/// Metoda je vol�na, pokud se metod� GetValue nepoda�� z�skat hodnotu.
		/// Nen�-li EmptyText rovno null, vrac� se hodnota Empty text. 
		/// Jinak je vyhozena v�jimka MissingMemberException.
		/// </summary>
		protected virtual object GetNotFoundDataItem()
		{
			if (EmptyText != null)
			{
				return EmptyText;
			}

			throw new MissingMemberException("Nepoda�ilo se vyhodnotit DataField.");
		}
		#endregion

		#region InitializeDataCell, InitializeDataCellContent
		/// <summary>
		/// Pokud nen� CellCssClass pr�zdn�, generuje se do bu�ky tabulky &lt;div="CellCssClass"&gt;...&lt;/div&gt;.
		/// Jinak se pou�ije norm�ln� samotn� bu�ka tabulky.
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
		/// Inicializuje obsah bu�ky daty.
		/// </summary>
		/// <param name="control">Control, do kter�ho se m� obsah inicializovat.</param>
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
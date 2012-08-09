using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// GridView, kter� automaticky zv�raz�uje polo�ku na z�klad� hodnoty ur�it� 
	/// property dat. Zv�razn�n� je provedeno nastaven�m hodnoty SelectedIndex.
	/// </summary>
	public abstract class HighlightingGridView : GridView
	{
		#region Properties
		/// <summary>
		/// Vlastnosti pro zv�razn�n� ��dku.
		/// </summary>
		public Highlighting Hightlighting
		{
			get
			{
				if (hightlighting == null)
					hightlighting = new Highlighting();
				return hightlighting;
			}
		}
		private Highlighting hightlighting;
		#endregion

		#region SaveViewState, LoadViewState
		/// <summary>
		/// Zajist� ulo�en� ViewState. Je p�id�no ulo�en� property Hightlighting.
		/// </summary>
		protected override object SaveViewState()
		{
			// Roz���� ViewState o objekt Highlighting.
			Pair viewStateData = new Pair();
			viewStateData.First = base.SaveViewState();
			viewStateData.Second = hightlighting;
			return viewStateData;
		}

		/// <summary>
		/// Zajist� na�ten� ViewState. Je p�id�no na�ten� property Hightlighting.
		/// </summary>
		protected override void LoadViewState(object savedState)
		{
			// Na�te roz���en� (viz SaveViewState) ViewState.
			Pair viewStateData = (Pair)savedState;
			base.LoadViewState(viewStateData.First);
			if (viewStateData.Second != null)
				hightlighting = (Highlighting)viewStateData.Second;
		}
		#endregion

		#region Zv�razn�n� ��dku
		/// <summary>
		/// Zajist� zv�razn�n� ��dku.
		/// </summary>
		protected override void PerformDataBinding(IEnumerable data)
		{
			// Nastav� SelectionIndex.
			HighlightRow(data);
			base.PerformDataBinding(data);
		}

		/// <summary>
		/// Prohled� data, pokud najde hodnotu rovnou HighlightValue, 
		/// vybere danou polo�ku.
		/// </summary>
		/// <param name="data"></param>
		protected virtual void HighlightRow(IEnumerable data)
		{
			if (Hightlighting.HighlightValue != null && data != null)
			{
				int index = 0;

				foreach (object o in data)
				{
					object value = DataBinder.GetPropertyValue(o, Hightlighting.DataField);
					if (Hightlighting.HighlightValue.Equals(value))
					{
						HighlightIndex(index);
						return;
					}
					index++;
				}
				HighlightIndex(-1);
			}
		}

		/// <summary>
		/// Vybere polo�ku s dan�m indexem. Je-li hodnota p��znaku AutoPageChangeEnabled
		/// true, provede p�estr�nkov�n�, pokud je pot�eba a nastav� hodnotu p��znaku
		/// na false, aby nedoch�zelo k ne��douc� zm�n� str�nky p�i zm�n� str�nky z 
		/// u�ivatelsk�ro rozhran� a n�sledn�ho databindingu.
		/// Hodnota indexu rovna -1 zru�� zv�razn�n� polo�ky.
		/// </summary>
		/// <param name="index">Index polo�ky. Po��t�no od nuly.</param>
		protected virtual void HighlightIndex(int index)
		{
			if (index >= 0)
			{
				int TargetPageIndex = (!AllowPaging) ? 0 : index / PageSize;
				int TargetSelectedIndex = index - TargetPageIndex * PageSize;

				if (Hightlighting.AutoPageChangeEnabled && Hightlighting.PageChangeEnabled)
				{
					PageIndex = TargetPageIndex;
					Hightlighting.PageChangeEnabled = false;
				}
				if (PageIndex == TargetPageIndex)
				{
					SelectedIndex = TargetSelectedIndex;
					return;
				}
			}
			SelectedIndex = -1;
		}
		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using Havit.Collections;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// SortingGridView zaji��uje �azen� polo�ek.
	/// Ukl�d� nastaven� �azen� dle u�ivatele a p��padn� zaji��uje automatick� �azen� pomoc� GenericPropertyCompareru.
	/// </summary>
	public abstract class SortingGridView: HighlightingGridView
	{
		#region Properties

		/// <summary>
		/// Nastavuje, zda m� p�i databindingu doj�t k se�azen� polo�ek podle nastaven�.
		/// </summary>
		public bool AutoSort
		{
			get { return (bool)(ViewState["AutoSort"] ?? false); }
			set { ViewState["AutoSort"] = value; }				
		}

		/// <summary>
		/// V�choz� �azen�, kter� se pou�ije, pokud je povoleno automatick� �azen� nastaven�m vlastnosti AutoSort 
		/// a u�ivatel dosu� ��dn� �azen� nezvolil.
		/// </summary>
		public string DefaultSortExpression
		{
			get { return (string)(ViewState["DefaultSortExpression"] ?? String.Empty); }
			set { ViewState["DefaultSortExpression"] = value; }
		}

		/// <summary>
		/// Zaji��uje pr�ci se senzamem polo�ek, podle kter�ch se �ad�.
		/// </summary>
		public new Sorting Sorting
		{
			get
			{
				return sorting;
			}
		}
		private Sorting sorting = new Sorting();
		#endregion

		#region	OnSorting
		/// <summary>
		/// P�i po�adavku na �azen� si zapamatujeme, jak cht�l u�ivatel �adit a nastav�me RequiresDataBinding na true.
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnSorting(GridViewSortEventArgs e)
		{
			sorting.AddSortExpression(e.SortExpression);
			base.RequiresDataBinding = true;
		}
		#endregion

		#region OnSorted
		/// <summary>
		/// Po set��d�n� podle sloupce zajist� u v�cestr�nkov�ch grid� n�vrat na prvn� str�nku
		/// </summary>
		/// <param name="e">argumenty ud�losti</param>
		protected override void OnSorted(EventArgs e)
		{
			base.OnSorted(e);
			PageIndex = 0;
		}
		#endregion

		#region Na��t�n� a ukl�d�n� ViewState
		/// <summary>
		/// Zajist� ulo�en� ViewState. Je p�id�no ulo�en� property Sorting.
		/// </summary>
		protected override object SaveViewState()
		{
			Pair viewStateData = new Pair();
			viewStateData.First = base.SaveViewState();
			viewStateData.Second = sorting;
			return viewStateData;
		}

		/// <summary>
		/// Zajist� na�ten� ViewState. Je p�id�no na�ten� property Sorting.
		/// </summary>
		protected override void LoadViewState(object savedState)
		{
			Pair viewStateData = (Pair)savedState;
			base.LoadViewState(viewStateData.First);
			if (viewStateData.Second != null)
				sorting = (Sorting)viewStateData.Second;
		}
		#endregion

		#region PerformDataBinding
		/// <summary>
		/// Provede databinding dat.
		/// Pokud data nejsou null a AutoSort je true, automaticky se�ad� data pomoc� GenericPropertyCompareru.
		/// Pokud u�ivatel dosud nenastavil ��dn� �azen�, pou�ije se �azen� dle DefaultSortExpression.
		/// </summary>
		/// <param name="data"></param>
		protected override void PerformDataBinding(System.Collections.IEnumerable data)
		{
			if ((data != null) && AutoSort)
			{
				if ((Sorting.SortItems.Count == 0) && !String.IsNullOrEmpty(DefaultSortExpression))
				{
					// sorting je nutn� zm�nit na z�klad� DefaultSortExpression,
					// kdybychom jej nezm�nili, tak prvn� kliknut� shodn� s DefaultSortExpression nic neud�l�
					sorting.AddSortExpression(DefaultSortExpression);
				}

				IEnumerable sortedData = SortHelper.PropertySort(data, Sorting.SortItems);
				base.PerformDataBinding(sortedData);
			}
			else
			{
				base.PerformDataBinding(data);
			}
		}
		#endregion
	}
}

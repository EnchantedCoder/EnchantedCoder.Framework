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
		#region PerformDataBinding
		/// <summary>
		/// Provede databinding dat.
		/// Pokud data nejsou null a AutoSort je true, automaticky se�ad� data pomoc� GenericPropertyCompareru.
		/// Pokud u�ivatel dosud nenastavil ��dn� �azen�, pou�ije se �azen� dle DefaultSortExpression.
		/// </summary>
		/// <param name="data"></param>
		protected override void PerformDataBinding(System.Collections.IEnumerable data)
		{
		}
		#endregion
	}
}

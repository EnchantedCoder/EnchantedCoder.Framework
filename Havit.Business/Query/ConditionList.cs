using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Havit.Business.Query
{
	/// <summary>
	/// Seznam podm�nek, kter� nem��e obsahovat pr�zdnou podm�nku.
	/// </summary>
	public class ConditionList: Collection<Condition>
	{
		/// <summary>
		/// P�edefinov�n� metody pro vkl�d�n� podm�nek. Nen� mo�n� vlo�it null (hodnota null je ignorov�na a nen� p�id�na do kolekce.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		protected override void InsertItem(int index, Condition item)
		{
			if (item != null)
			{
				base.InsertItem(index, item);
			}
		}		
	}
}

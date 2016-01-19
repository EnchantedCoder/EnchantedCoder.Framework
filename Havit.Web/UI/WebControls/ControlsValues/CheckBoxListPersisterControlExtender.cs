﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Havit.Web.UI.WebControls.ControlsValues;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Havit.Web.UI.WebControls.ControlsValues
{
	/// <summary>
	/// IPersisterControlExtender pro CheckBoxList.
	/// </summary>
	internal class CheckBoxListPersisterControlExtender : IPersisterControlExtender
	{
		#region GetValue
		public object GetValue(Control control)
		{
			return ((CheckBoxList)control).Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Value).ToArray();
		} 
		#endregion

		#region GetValueType
		public Type GetValueType()
		{
			return typeof(String[]);
		} 
		#endregion

		#region SetValue
		public void SetValue(Control control, object value)
		{
			CheckBoxList checkBoxList = (CheckBoxList)control;
			string[] values = (string[])value;

			checkBoxList.ClearSelection();
			foreach (string item in values)
			{
				ListItem listItem = checkBoxList.Items.FindByValue(item);
				if (listItem != null)
				{
					listItem.Selected = true;
				}
			}
		} 
		#endregion

		#region GetPriority
		public int? GetPriority(System.Web.UI.Control control)
		{
			if (control is CheckBoxList)
			{
				return 1;
			}
			return null;
		}
		#endregion

		#region PersistsChildren
		/// <summary>
		/// Pokud je true, ControlsValuesPersister se pokusí uložit i hodnoty child controlů.
		/// Implicitně vrací false.
		/// </summary>
		public bool PersistsChildren(Control control)
		{
			return false;
		}
		#endregion
	}
}

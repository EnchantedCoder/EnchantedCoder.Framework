using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Havit.Business;

namespace Havit.Enterprise.Web.UI.WebControls
{
	public class EnterpriseCheckBoxList: CheckBoxList
	{
		#region ItemObjectInfo
		/// <summary>
		/// Ud�v� metodu, kterou se z�sk� objekt na z�klad� ID.
		/// Hodnota vlastnosti nep�e��v� postback.
		/// </summary>
		public ObjectInfo ItemObjectInfo
		{
			get
			{
				return itemObjectInfo;
			}
			set
			{
				itemObjectInfo = value;
			}
		}
		private ObjectInfo itemObjectInfo;
		#endregion

		#region AutoSort
		/// <summary>
		/// Ud�v�, zda je zapnuto automatick� �azen� polo�ek p�i databindingu. V�choz� hodnota je true.
		/// </summary>
		public bool AutoSort
		{
			get { return (bool)(ViewState["AutoSort"] ?? true); }
			set { ViewState["AutoSort"] = value; }
		}
		#endregion

		#region AutoDataBind
		/// <summary>
		/// Ud�v�, zda je zapnuto automatick� nabindov�n� polo�ek p�i prvn�m na�ten� str�nky. V�choz� hodnota je false.
		/// </summary>
		public bool AutoDataBind
		{
			get { return (bool)(ViewState["AutoDataBind"] ?? false); }
			set { ViewState["AutoDataBind"] = value; }
		}
		#endregion

		#region DataSortField
		/// <summary>
		/// Ur�uje, podle jak� property jsou �azena. Pokud nen� ��dn� hodnota nastavena pou�ije se hodnota vlastnosti DataTextField.
		/// </summary>
		public string DataSortField
		{
			get { return (string)(ViewState["DataSortField"] ?? DataTextField); }
			set { ViewState["DataSortField"] = value; }
		}
		#endregion		

		#region SelectedId
		/// <summary>
		/// Vrac� ID vybran� polo�ky. Nen�-li ��dn� polo�ka vybran�, vrac� null.
		/// </summary>
		public int? SelectedId
		{
			get
			{
				return (SelectedValue == String.Empty) ? (int?)null : int.Parse(SelectedValue);
			}
		}
		#endregion

		#region SelectedObjects
		/// <summary>
		/// Vrac� objekt na z�klad� vybran� polo�ky v DropDownListu. Objekt se z�sk�v� metodou ve vlastnosti ItemsObjectInfo.
		/// Nen�-li ��dn� polo�ka vybr�na, vrac� null.
		/// </summary>
		public BusinessObjectBase[] SelectedObjects
		{
			get
			{
				if (itemObjectInfo == null)
				{
					throw new InvalidOperationException("Nen� nastavena vlastnost ItemObjectInfo.");
				}

				return (SelectedId == null) ? null : itemObjectInfo.GetObjectMethod(SelectedId.Value);
			}
			set
			{
				if (isDataBinding)
				{
					// pokud jsme v databindingu, odlo��me nastaven� hodnoty, proto�e je�t� nemus�me m�t DataSource ani data v Items.
					delayedSetSelectedObjectSet = true;
					delayedSetSelectedObject = value;
					return;
				}

				if (value == null)
				{
					EnsureAutoDataBind(); // jinak n�sledn� databinding zlikviduje vybranou hodnotu
					// pokud nastavujeme null, zajistime, aby existoval prazdny radek a vybereme jej
					EnsureEmptyItem();
					SelectedIndex = 0;
				}
				else
				{
					if (value.IsNew)
					{
						throw new ArgumentException("Nelze vybrat neulo�en� objekt.");
					}

					EnsureAutoDataBind();

					// pokud nastavujeme objekt
					ListItem listItem = Items.FindByValue(value.ID.ToString());
					if (listItem != null)
					{
						// nastavovany objekt je v seznamu
						SelectedValue = listItem.Value;
					}
					else
					{
						ListItem newListItem = new ListItem();
						newListItem.Text = DataBinder.Eval(value, DataTextField).ToString();
						newListItem.Value = DataBinder.Eval(value, DataValueField).ToString();
						Items.Add(newListItem);
						SelectedIndex = Items.Count - 1;
					}
				}
			}
		}
		#endregion

		#region DataBindPerformed
		/// <summary>
		/// Indikuje, zda ji� do�lo k nav�z�n� dat.
		/// </summary>
		private bool DataBindPerformed
		{
			get { return (bool)(ViewState["DataBindPerformed"] ?? false); }
			set { ViewState["DataBindPerformed"] = value; }
		}
		#endregion

		#region Constructor
		public EnterpriseCheckBoxList()
		{
			DataValueField = "ID";
		}
		#endregion

		#region EnsureAutoDataBind
		/// <summary>
		/// Zajist� nabindov�n� dat pro re�it AutoDataBind.
		/// </summary>
		protected void EnsureAutoDataBind()
		{
			if (AutoDataBind && !DataBindPerformed)
			{
				DataBindAll();
			}
		}
		#endregion

	}
}

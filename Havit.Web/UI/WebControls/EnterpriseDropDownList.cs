using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Havit.Business;
using Havit.Collections;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// EnterpriseDropDownList zaji��uje pohodln�j�� pr�ci s DropDownListem, jeho� prvky maj� vazbu ne business objekty.	
	/// </summary>
	public class EnterpriseDropDownList : DropDownList
	{
		#region Constructor
		/// <summary>
		/// Inicialuze DataValueField na "ID".
		/// </summary>
		public EnterpriseDropDownList()
		{
			DataValueField = "ID";
		}
		#endregion

		#region Properties
		/// <summary>
		/// ReferenceFieldPropertyInfo property, jej� hodnota se t�mto DropDownListem vyb�r�.
		/// Nastaven� t�to hodnoty rovn� p�ep�e hodnoty vlastnost� ItemsObjectInfo a Nullable.
		/// Hodnota t�to property nep�e��v� postback.
		/// </summary>
		public ReferenceFieldPropertyInfo PropertyInfo
		{
			get
			{
				return propertyInfo;
			}
			set
			{
				propertyInfo = value;
				itemsObjectInfo = propertyInfo.TargetObjectInfo;
				Nullable = propertyInfo.Nullable;
			}
		}
		private ReferenceFieldPropertyInfo propertyInfo;

		/// <summary>
		/// Ud�v� metodu, kterou se z�sk� objekt na z�klad� ID.
		/// Hodnota vlastnosti je automaticky nastavena nastaven�m vlastnosti PropertyInfo.
		/// Hodnota vlastnosti nep�e��v� postback.
		/// </summary>
		public ObjectInfo ItemsObjectInfo
		{
			get
			{
				return itemsObjectInfo;
			}
			set
			{
				itemsObjectInfo = value;
			}
		}
		private ObjectInfo itemsObjectInfo;

		/// <summary>
		/// Ud�v�, zda m� b�t na v�b�r pr�zdn� hodnota. V�choz� hodnota je true.
		/// </summary>
		public bool Nullable
		{
			get { return (bool)(ViewState["Nullable"] ?? true); }
			set { ViewState["Nullable"] = value; }
		}

		/// <summary>
		/// Ud�v�, zda je zapnuto automatick� �azen� polo�ek p�i databindingu. V�choz� hodnota je true.
		/// </summary>
		public bool AutoSort
		{
			get { return (bool)(ViewState["AutoSort"] ?? true); }
			set { ViewState["AutoSort"] = value; }
		}

		/// <summary>
		/// Ud�v�, zda je zapnuto automatick� nabindov�n� polo�ek p�i prvn�m na�ten� str�nky. V�choz� hodnota je false.
		/// </summary>
		public bool AutoDataBind
		{
			get { return (bool)(ViewState["AutoDataBind"] ?? false); }
			set { ViewState["AutoDataBind"] = value; }
		}

		/// <summary>
		/// Ur�uje, podle jak� property jsou �azena. Pokud nen� ��dn� hodnota nastavena pou�ije se hodnota vlastnosti DataTextField.
		/// </summary>
		public string DataSortField
		{
			get { return (string)(ViewState["DataSortField"] ?? DataTextField); }
			set { ViewState["DataSortField"] = value; }
		}
		
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

		/// <summary>
		/// Vrac� objekt na z�klad� vybran� polo�ky v DropDownListu. Objekt se z�sk�v� metodou ve vlastnosti ItemsObjectInfo.
		/// Nen�-li ��dn� polo�ka vybr�na, vrac� null.
		/// </summary>
		public BusinessObjectBase SelectedObject
		{
			get
			{
				if (itemsObjectInfo == null)
					throw new InvalidOperationException("Nen� nastavena vlastnost ObjectInfo.");

				return (SelectedId == null) ? null : itemsObjectInfo.GetObjectMethod(SelectedId.Value);
			}
			set
			{
				if (value == null)
				{
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

					// pokud nastavujeme objekt
					ListItem listItem = Items.FindByValue(value.ID.ToString());
					if (listItem != null)
					{
						// nastavovany objekt je v seznamu
						SelectedValue = listItem.Value;
					}
					else
					{
						// nastavovany objekt neni v seznamu, pridame jej
						bool oldAppendDataBoundItems = AppendDataBoundItems;
						AppendDataBoundItems = true;
						DataSource = new object[] { value };
						DataBind();
						AppendDataBoundItems = oldAppendDataBoundItems;
						SelectedIndex = Items.Count - 1;
					}
				}
			}
		}
		#endregion

		#region DataBinding

		/// <summary>
		/// Pokud jde o prvn� na�ten� str�nky a nen� nastaveno AutoDataBind, zavol� DataBindAll.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack && AutoDataBind)
			{
				DataBindAll();
			}
		}

		/// <summary>
		/// Nav�e na DropDownList v�echny (nasmazan�) business objekty ur�it�ho typu
		/// (zavol� metodu GetAll(), nastav� v�sledek je jako DataSource a zavol� DataBind).
		/// </summary>
		protected void DataBindAll()
		{
			if (itemsObjectInfo == null)
				throw new InvalidOperationException("Nen� nastavena vlastnost ItemsObjectInfo.");

			DataSource = itemsObjectInfo.GetAllMethod();
			DataBind();
		}

		/// <summary>
		/// Zajist�, aby byl po databindingu dopln�n ��dek pro v�b�r pr�zdn� hodnoty.
		/// </summary>
		/// <param name="dataSource"></param>
		protected override void PerformDataBinding(System.Collections.IEnumerable dataSource)
		{
			if ((dataSource != null) && AutoSort)
			{
				IEnumerable sortedData = SortHelper.PropertySort(dataSource, DataSortField);
				base.PerformDataBinding(sortedData);
			}
			else
			{
				base.PerformDataBinding(dataSource);
			}
			if (Nullable)
			{
				EnsureEmptyItem();
			}
		}

		/// <summary>
		/// P�id� na za��tek seznamu ��dek pro v�b�r pr�zdn� hodnoty, pokud tam ji� nen�.
		/// </summary>
		protected void EnsureEmptyItem()
		{
			if ((Items.Count == 0) || (Items[0].Value != String.Empty))
				Items.Insert(0, new ListItem("---", String.Empty));
		}
		#endregion
	}
}

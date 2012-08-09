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
		public ReferenceFieldPropertyInfo ItemPropertyInfo
		{
			get
			{
				return itemPropertyInfo;
			}
			set
			{
				if ((itemObjectInfo != null) && (itemObjectInfo != value.TargetObjectInfo))
				{
					throw new ArgumentException("Nekonzistence ItemPropertyInfo.TargetObjectInfo a ItemObjectInfo");
				}
				itemPropertyInfo = value;
				itemObjectInfo = itemPropertyInfo.TargetObjectInfo;
				Nullable = itemPropertyInfo.Nullable;
			}
		}
		private ReferenceFieldPropertyInfo itemPropertyInfo;

		/// <summary>
		/// Ud�v� metodu, kterou se z�sk� objekt na z�klad� ID.
		/// Hodnota vlastnosti je automaticky nastavena nastaven�m vlastnosti PropertyInfo.
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
				if ((itemPropertyInfo != null) && (value != itemPropertyInfo.TargetObjectInfo))
				{
					throw new ArgumentException("Nekonzistence ItemPropertyInfo.TargetObjectInfo a ItemObjectInfo");
				}
				itemObjectInfo = value;
			}
		}
		private ObjectInfo itemObjectInfo;

		/// <summary>
		/// Ud�v�, zda m� b�t na v�b�r pr�zdn� hodnota. V�choz� hodnota je true.
		/// </summary>
		public bool Nullable
		{
			get { return (bool)(ViewState["Nullable"] ?? true); }
			set { ViewState["Nullable"] = value; }
		}

		/// <summary>
		/// Ud�v� text pr�zdn� hodnoty. V�choz� hodnota je "---".
		/// </summary>
		public string NullableText
		{
			get { return (string)(ViewState["NullableText"] ?? "---"); }
			set { ViewState["NullableText"] = value; }
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
				if (itemObjectInfo == null)
					throw new InvalidOperationException("Nen� nastavena vlastnost ItemObjectInfo.");

				return (SelectedId == null) ? null : itemObjectInfo.GetObjectMethod(SelectedId.Value);
			}
			set
			{
				if (isDataBinding)
				{
					// pokud jsme v databindingu, odlo��me nastaven� hodnoty, proto�e je�t� nemus�me m�t DataSource ani data v Items.
					delayedSetSelectedObject = value;
					return;
				}

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

		/// <summary>
		/// Prov�d� databinding a �e�� odlo�en� nastaven� SelectedObject.
		/// </summary>
		public override void DataBind()
		{
			isDataBinding = true;
			base.DataBind();
			isDataBinding = false;

			if (delayedSetSelectedObject != null)
			{
				SelectedObject = delayedSetSelectedObject;
				delayedSetSelectedObject = null;
			}
		}

		#endregion

		#region Private properties
		/// <summary>
		/// Indikuje, zda ji� do�lo k nav�z�n� dat.
		/// </summary>
		private bool DataBindPerformed
		{
			get { return (bool)(ViewState["DataBindPerformed"] ?? false);  }
			set { ViewState["DataBindPerformed"] = value; }
		}


		/// <summary>
		/// Indikuje pr�v� porob�haj�c� databinding.
		/// </summary>
		bool isDataBinding = false;

		/// <summary>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data.
		/// </summary>
		BusinessObjectBase delayedSetSelectedObject = null;

		#endregion

		#region DataBinding

		/// <summary>
		/// Pokud jde o prvn� na�ten� str�nky a nen� nastaveno AutoDataBind, zavol� DataBindAll.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnsureAutoDataBind();
		}

		/// <summary>
		/// Nav�e na DropDownList v�echny (nasmazan�) business objekty ur�it�ho typu
		/// (zavol� metodu GetAll(), nastav� v�sledek je jako DataSource a zavol� DataBind).
		/// </summary>
		protected void DataBindAll()
		{
			if (itemObjectInfo == null)
				throw new InvalidOperationException("Nen� nastavena vlastnost ItemObjectInfo.");

			PerformDataBinding(itemObjectInfo.GetAllMethod());
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
			DataBindPerformed = true;
		}

		/// <summary>
		/// P�id� na za��tek seznamu ��dek pro v�b�r pr�zdn� hodnoty, pokud tam ji� nen�.
		/// </summary>
		protected void EnsureEmptyItem()
		{
			if ((Items.Count == 0) || (Items[0].Value != String.Empty))
				Items.Insert(0, new ListItem(NullableText, String.Empty));
		}
		#endregion
	}
}

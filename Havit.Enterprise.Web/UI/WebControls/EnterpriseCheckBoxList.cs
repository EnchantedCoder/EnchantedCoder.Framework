using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Havit.Business;
using System.Web.UI;
using Havit.Collections;
using System.Collections;

namespace Havit.Web.UI.WebControls
{
	public class EnterpriseCheckBoxList: CheckBoxListExt
	{
		#region ItemPropertyInfo
		/// <summary>
		/// ReferenceFieldPropertyInfo property, jej� hodnota se t�mto DropDownListem vyb�r�.
		/// Nastaven� t�to hodnoty rovn� p�ep�e hodnoty vlastnost� ItemObjectInfo a Nullable.
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
				//				Nullable = itemPropertyInfo.Nullable;
			}
		}
		private ReferenceFieldPropertyInfo itemPropertyInfo;
		#endregion

		#region ItemObjectInfo
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

		#region SortDirection
		/// <summary>
		/// Ud�v� sm�r �azen� polo�ek.
		/// V�choz� je vzestupn� �azen� (Ascending).
		/// </summary>
		public Havit.Collections.SortDirection SortDirection
		{
			get { return (Havit.Collections.SortDirection)(ViewState["SortDirection"] ?? Havit.Collections.SortDirection.Ascending); }
			set { ViewState["SortDirection"] = value; }
		}
		#endregion

		#region SelectedIds
		/// <summary>
		/// Vrac� ID vybran� polo�ky. Nen�-li ��dn� polo�ka vybran�, vrac� null.
		/// </summary>
		public int[] SelectedIds
		{
			get
			{
				List<int> result = new List<int>();
				foreach (ListItem item in this.Items)
				{
					if (item.Selected)
					{
						result.Add(int.Parse(item.Value));
					}
				}
				return result.ToArray();
			}
		}
		#endregion

		#region SelectedObjects
		/// <summary>
		/// Vrac� v��et s vybran�mi objekty (na z�klad� za�krtnut� CheckBox�). Objekt se z�sk�v� metodou ve vlastnosti ItemObjectInfo.
		/// </summary>
		public IEnumerable SelectedObjects
		{
			get
			{
				if (itemObjectInfo == null)
				{
					throw new InvalidOperationException("Nen� nastavena vlastnost ItemObjectInfo.");
				}

				List<BusinessObjectBase> result = new List<BusinessObjectBase>();
				foreach (int id in SelectedIds)
				{
					result.Add(itemObjectInfo.GetObjectMethod(id));
				}
				return result;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}

				if (isDataBinding)
				{
					// pokud jsme v databindingu, odlo��me nastaven� hodnoty, proto�e je�t� nemus�me m�t DataSource ani data v Items.
					delayedSetSelectedObjectSet = true;
					delayedSetSelectedObjects = value;
					return;
				}

				EnsureAutoDataBind(); // jinak n�sledn� databinding zlikviduje vybranou hodnotu
				this.ClearSelection();

				foreach (object selectObject in value)
				{
					if (!(selectObject is BusinessObjectBase))
					{
						throw new ArgumentException("Data obsahuj� prvek, kter� nen� potomkem BusinessObjectBase.");
					}

					BusinessObjectBase businessObject = (BusinessObjectBase)selectObject;
					if (businessObject.IsNew)
					{
						throw new ArgumentException("Nelze vybrat neulo�en� objekt.");
					}

					// pokud nastavujeme objekt
					ListItem listItem = Items.FindByValue(businessObject.ID.ToString());
					if (listItem != null)
					{
						// nastavovany objekt je v seznamu
						listItem.Selected = true;
					}
					else
					{
						ListItem newListItem = new ListItem();
						newListItem.Text = DataBinder.Eval(businessObject, DataTextField).ToString();
						newListItem.Value = DataBinder.Eval(businessObject, DataValueField).ToString();
						newListItem.Selected = true;
						Items.Add(newListItem);
					}
				}
			}
		}
		#endregion

		#region EnsureAutoDataBind
		/// <summary>
		/// Zajist� nabindov�n� dat pro re�im AutoDataBind.
		/// </summary>
		protected void EnsureAutoDataBind()
		{
			if (AutoDataBind && !DataBindPerformed)
			{
				DataBindAll();
			}
		}
		#endregion

		#region Private properties
		/// <summary>
		/// Indikuje, zda ji� do�lo k nav�z�n� dat.
		/// </summary>
		private bool DataBindPerformed
		{
			get { return (bool)(ViewState["DataBindPerformed"] ?? false); }
			set { ViewState["DataBindPerformed"] = value; }
		}

		/// <summary>
		/// Indikuje pr�v� porob�haj�c� databinding.
		/// </summary>
		bool isDataBinding = false;

		/// <summary>
		/// Objekt, kter� m� b�t nastaven jako vybran�, ale jeho nastaven� bylo odlo�eno.
		/// </summary>
		/// <remarks>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data.
		/// </remarks>
		IEnumerable delayedSetSelectedObjects = null;

		/// <summary>
		/// Ud�v�, zda m�me nastaven objekt pro odlo�en� nastaven� vybran�ho objektu.
		/// </summary>
		/// <remarks>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data. 
		/// </remarks>
		bool delayedSetSelectedObjectSet = false;

		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci EnterpriseCheckBoxList.
		/// </summary>
		public EnterpriseCheckBoxList()
		{
			DataValueField = "ID";
		} 
		#endregion

		#region OnLoad
		/// <summary>
		/// Pokud jde o prvn� na�ten� str�nky a nen� nastaveno AutoDataBind, zavol� DataBindAll.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnsureAutoDataBind();
		}
		#endregion

		#region DataBind
		/// <summary>
		/// Prov�d� databinding a �e�� odlo�en� nastaven� SelectedObject.
		/// </summary>
		public override void DataBind()
		{
			// v p��pad� pou�it� z GridView (v EnterpriseGV bez AutoDataBind)
			// se vyvol� nejd��v DataBind a pot� teprve OnLoad.
			// mus�me proto zajistit napln�n� hodnot seznamu i zde
			EnsureAutoDataBind();

			isDataBinding = true;
			base.DataBind();
			isDataBinding = false;

			if (delayedSetSelectedObjectSet)
			{
				this.SelectedObjects = delayedSetSelectedObjects;
				delayedSetSelectedObjectSet = false;
				delayedSetSelectedObjects = null;
			}
		}
		#endregion

		#region DataBindAll
		/// <summary>
		/// Nav�e na DropDownList v�echny (nasmazan�) business objekty ur�it�ho typu
		/// (zavol� metodu GetAll(), nastav� v�sledek je jako DataSource a zavol� DataBind).
		/// </summary>
		protected void DataBindAll()
		{
			if (itemObjectInfo == null)
			{
				throw new InvalidOperationException("Nen� nastavena vlastnost ItemObjectInfo.");
			}

			PerformDataBinding(itemObjectInfo.GetAllMethod());
		}
		#endregion

		#region PerformDataBinding
		/// <summary>
		/// Zajist�, aby byl po databindingu dopln�n ��dek pro v�b�r pr�zdn� hodnoty.
		/// </summary>
		/// <param name="dataSource"></param>
		protected override void PerformDataBinding(System.Collections.IEnumerable dataSource)
		{
			if (String.IsNullOrEmpty(DataTextField))
			{
				throw new InvalidOperationException(String.Format("Nen� nastavena hodnota vlastnosti DataTextField controlu {0}.", ID));
			}

			if ((dataSource != null) && AutoSort)
			{
				if (String.IsNullOrEmpty(DataSortField))
				{
					throw new InvalidOperationException(String.Format("AutoSort je true, ale nen� nastavena hodnota vlastnosti DataSortField controlu {0}.", ID));
				}

				SortItemCollection sorting = new SortItemCollection();
				sorting.Add(new SortItem(this.DataSortField, this.SortDirection));
				IEnumerable sortedData = SortHelper.PropertySort(dataSource, sorting);

				base.PerformDataBinding(sortedData);
			}
			else
			{
				base.PerformDataBinding(dataSource);
			}

			DataBindPerformed = true;

		}
		#endregion

		#region SelectObjectsIfPresent
		/// <summary>
		/// Vybere objekt dle ID, pokud je objekt s t�mto ID mezi daty.
		/// Pokud nen�, neprovede nic. V�sledn� kolekce objekt� nastav� v�b�r (SelectedObjects).
		/// Metoda je ur�ena pro vnit�n� implementaci ukl�d�n� hodnot.
		/// </summary>
		/// <param name="objectIDs"></param>
		public void SelectObjectsIfPresent(int[] objectIDs)
		{
			List<object> objectsList = new List<object>();

			EnsureAutoDataBind();

			foreach (int objectID in objectIDs)
			{
				// pokud se objectID nach�z� mezi prvkama ERBL, z�sk�me objekt a p�id�me ho do kolekce
				if (Items.FindByValue(objectID.ToString()) != null)
				{
					Object obj = ItemObjectInfo.GetObjectMethod(objectID);
					objectsList.Add(obj);
				}
			}

			// Nastaven� vybran�ch polo�ek
			SelectedObjects = objectsList.ToArray();
		} 
		#endregion
	}
}

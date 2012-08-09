using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Havit.Business;
using Havit.Collections;
using Havit.Web.UI.WebControls.ControlsValues;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// EnterpriseDropDownList zaji��uje pohodln�j�� pr�ci s DropDownListem, jeho� prvky p�edstavuj� business objekty.	
	/// </summary>
	public class EnterpriseDropDownList : DropDownListExt
	{
		#region Constructors (static)
		static EnterpriseDropDownList()
		{
			Havit.Web.UI.WebControls.ControlsValues.PersisterControlExtenderRepository.Default.Add(new EnterpriseDropDownListPersisterControlExtender());
		} 
		#endregion
		
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

		#region Nullable
		/// <summary>
		/// Ud�v�, zda m� b�t na v�b�r pr�zdn� hodnota. V�choz� hodnota je true.
		/// </summary>
		public bool Nullable
		{
			get { return (bool)(ViewState["Nullable"] ?? true); }
			set { ViewState["Nullable"] = value; }
		}		
		#endregion

		#region NullableText
		/// <summary>
		/// Ud�v� text pr�zdn� hodnoty. V�choz� hodnota je "---".
		/// </summary>
		public string NullableText
		{
			get { return (string)(ViewState["NullableText"] ?? "---"); }
			set { ViewState["NullableText"] = value; }
		}		
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

		#region SelectedId
		/// <summary>
		/// Vrac� ID vybran� polo�ky. Nen�-li ��dn� polo�ka vybran�, vrac� null.
		/// </summary>
		public int? SelectedId
		{
			get
			{
				return (String.IsNullOrEmpty(SelectedValue) ? (int?)null : int.Parse(SelectedValue));
			}
		}		
		#endregion

		#region SelectedObject
		/// <summary>
		/// Vrac� objekt na z�klad� vybran� polo�ky v DropDownListu. Objekt se z�sk�v� metodou ve vlastnosti ItemObjectInfo.
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
					ClearSelection(); // pot�ebujeme potla�it chov�n� v p�edkovi - cachedSelectedIndex a cachedSelectedValue (U� tam m��e b�t nastavena hodnota, ale my chceme jinou, jen�e d�ky delayedXXX ji nastav�me a� za chvilku. Tak�e by n�m to bez tohoto ��dku mohlo padat.)
					delayedSetSelectedObjectSet = true;
					delayedSetSelectedObject = value;
					return;
				}

				if (value == null)
				{
					EnsureAutoDataBind(); // jinak n�sledn� databinding zlikviduje vybranou hodnotu
					// pokud nastavujeme null, zajistime, aby existoval prazdny radek a vybereme jej
					EnsureEmptyItem();
					SelectedValue = "";
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
						SelectedValue = newListItem.Value;
					}
				}
			}
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
		BusinessObjectBase delayedSetSelectedObject = null;

		/// <summary>
		/// Ud�v�, zda m�me nastaven objekt pro odlo�en� nastaven� vybran�ho objektu.
		/// </summary>
		/// <remarks>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data. 
		/// </remarks>
		bool delayedSetSelectedObjectSet = false;

		#region IsNullable
		/// <summary>
		/// Ud�v�, zda je EDDL nullable, viz k�d...
		/// </summary>
		private bool IsNullable
		{
			get
			{
				if (itemPropertyInfo != null)
				{
					return itemPropertyInfo.Nullable;
				}
				else
				{
					return Nullable;
				}
			}
		}
		#endregion

		#endregion

		#region ---------------------------------------------------------------------------------------------
		#endregion

		#region Constructor
		/// <summary>
		/// Inicialuze DataValueField na "ID".
		/// </summary>
		public EnterpriseDropDownList()
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
				SelectedObject = delayedSetSelectedObject;
				delayedSetSelectedObjectSet = false;
				delayedSetSelectedObject = null;
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

			CheckNullableConsistency();
			if (IsNullable)
			{
				EnsureEmptyItem();
				// SelectedIndex = 0;
			}
			DataBindPerformed = true;

		}		
		#endregion

		#region CheckNullableConsistency
		/// <summary>
		/// Ov��� konzistentn� zad�n� ItemPropertyInfo.Nullable a Nullable.
		/// 
		/// </summary>
		private void CheckNullableConsistency()
		{
			bool? nullable = (bool?)ViewState["Nullable"];
			if ((nullable != null) && (itemPropertyInfo != null))
			{
				if (itemPropertyInfo.Nullable != nullable)
				{
					throw new ApplicationException("Je-li nastavena hodnota ItemPropertyInfo a Nullable, mus� b�t ItemPropertyInfo.Nullable a Nullable shodn�. Nyn� se li��.");
				}
			}
		}

		#endregion

		#region EnsureEmptyItem
		/// <summary>
		/// P�id� na za��tek seznamu ��dek pro v�b�r pr�zdn� hodnoty, pokud tam ji� nen�.
		/// </summary>
		public void EnsureEmptyItem()
		{
			if ((Items.Count == 0) || (Items[0].Value != String.Empty))
			{
				Items.Insert(0, new ListItem(NullableText, String.Empty));
			}
		}
		#endregion

        /// <summary>
        /// Vybere objekt dle ID, pokud je objekt s t�mto ID mezi daty.
        /// Pokud nen�, neprovede nic.
        /// Vrac� true/false indikuj�c�, zda se poda�ilo objekt vybrat.
        /// Metoda je ur�ena pro vnit�n� implementaci ukl�d�n� hondot filtr�.
        /// </summary>
#warning Po p�esunu ukl�d�n� hodnot filtr� z DSV do frameworku ud�lat metodu intern�.
        public bool SelectObjectIfPresent(int? objectID)
        {

            if ((objectID == null) && Nullable)
            {
                EnsureAutoDataBind();
                EnsureEmptyItem();
                SelectedValue = "";
                return true;
            }

            if (objectID != null)
            {
                EnsureAutoDataBind();
                // pokud nastavujeme objekt
                ListItem listItem = Items.FindByValue(objectID.Value.ToString());
                if (listItem != null)
                {
                    // nastavovany objekt je v seznamu
                    SelectedValue = listItem.Value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }   
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// DropDownList pro pr�ci s v��tov�m datov�m typem enum
	/// </summary>
	public class EnumDropDownList : DropDownListExt
	{
		#region EnumType
		/// <summary>
		/// Typ enum, kter� obsluhujeme.
		/// </summary>
		public Type EnumType
		{
			get
			{
				return _enumType;
			}
			set
			{
				if (!value.IsEnum)
				{
					throw new ArgumentException("Parametr mus� b�t v��tov�m typem.");
				}
				_enumType = value;

			}
		}
		private Type _enumType;
		#endregion

		#region SelectedEnumValue
		/// <summary>
		/// Hodnota typu enum, kter� je nastavena DropDownListu
		/// </summary>
		public object SelectedEnumValue
		{
			set
			{
				if (isDataBinding)
				{
					// pokud jsme v databindingu, odlo��me nastaven� hodnoty, proto�e je�t� nemus�me m�t DataSource ani data v Items.
					delayedSetSelectedEnumValueNeeded = true;
					delayedSetSelectedEnumValue = value;
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
					EnsureAutoDataBind();
					SelectedValue = value.ToString();
				}
			}
			get
			{
				if (String.IsNullOrEmpty(this.SelectedValue))
				{
					return null;
				}
				else
				{
					if (this.EnumType == null)
					{
						throw new InvalidOperationException("Nen� nastavena vlastnost EnumType.");
					}
					return Enum.Parse(this.EnumType, this.SelectedValue);
				}
			}
		}
		#endregion

		#region Nullable, NullableText
		/// <summary>
		/// Ud�v�, zda m� b�t na v�b�r pr�zdn� hodnota. V�choz� hodnota je true.
		/// </summary>
		public bool Nullable
		{
			get
			{
				return (bool)(ViewState["Nullable"] ?? true);
			}
			set
			{
				ViewState["Nullable"] = value;
			}
		}

		/// <summary>
		/// Ud�v� text pr�zdn� hodnoty. V�choz� hodnota je "---".
		/// </summary>
		public string NullableText
		{
			get
			{
				return (string)(ViewState["NullableText"] ?? "---");
			}
			set
			{
				ViewState["NullableText"] = value;
			}
		}
		#endregion

		#region OnLoad
		/// <summary>
		/// Handles the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnsureAutoDataBind();
		}
		#endregion

		#region EnsureAutoDataBind (protected)
		/// <summary>
		/// Zajist� nabindov�n� dat.
		/// </summary>
		private void EnsureAutoDataBind()
		{
			if (!this.DataBindPerformed)
			{
				DataBindAll();
			}
		}
		#endregion

		#region DataBind
		/// <summary>
		/// Prov�d� databinding a �e�� odlo�en� nastaven� SelectedObject.
		/// </summary>
		public override void DataBind()
		{
			isDataBinding = true;
			base.DataBind();
			isDataBinding = false;

			if (delayedSetSelectedEnumValueNeeded)
			{
				SelectedEnumValue = delayedSetSelectedEnumValue;
				delayedSetSelectedEnumValueNeeded = false;
				delayedSetSelectedEnumValue = null;
			}
		}
		#endregion

		#region Private properties
		/// <summary>
		/// Indikuje, zda ji� do�lo k nav�z�n� dat.
		/// </summary>
		private bool DataBindPerformed
		{
			get
			{
				return (bool)(ViewState["DataBindPerformed"] ?? false);
			}
			set
			{
				ViewState["DataBindPerformed"] = value;
			}
		}

		/// <summary>
		/// Indikuje pr�v� porob�haj�c� databinding.
		/// </summary>
		private bool isDataBinding = false;

		/// <summary>
		/// Objekt, kter� m� b�t nastaven jako vybran�, ale jeho nastaven� bylo odlo�eno.
		/// </summary>
		/// <remarks>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data.
		/// </remarks>
		private object delayedSetSelectedEnumValue = null;

		/// <summary>
		/// Ud�v�, zda m�me nastaven objekt pro odlo�en� nastaven� vybran�ho objektu.
		/// </summary>
		/// <remarks>
		/// Pokud nastavujeme SelectedObject b�hem DataBindingu (ve str�nce pomoc� &lt;%# ... %&gt;),
		/// odlo�� se nastaven� hodnoty a� na konec DataBindingu. To proto�e v okam�iku nastavov�n� SelectedObject 
		/// nemus� b�t v Items je�t� data. 
		/// </remarks>
		private bool delayedSetSelectedEnumValueNeeded = false;
		#endregion

		#region DataBindAll (private)
		/// <summary>
		/// Nav�e na DropDownList v�echny hodnoty enumu.
		/// </summary>
		private void DataBindAll()
		{
			if (this.EnumType == null)
			{
				throw new InvalidOperationException("Nen� nastavena vlastnost EnumType");
			}
			PerformDataBinding(Enum.GetValues(this.EnumType));
		}
		#endregion

		#region PerformDataBinding (override)
		/// <summary>
		/// Zajist�, aby byl po databindingu dopln�n ��dek pro v�b�r pr�zdn� hodnoty.
		/// </summary>
		/// <param name="dataSource"></param>
		protected override void PerformDataBinding(System.Collections.IEnumerable dataSource)
		{
			base.PerformDataBinding(dataSource);

			if (this.Nullable)
			{
				EnsureEmptyItem();
				SelectedIndex = 0;
			}
			DataBindPerformed = true;
		}
		#endregion

		#region CreateItem (override)
		/// <summary>
		/// Vytvo�� ListItem, sou��st PerformDataBindingu.
		/// </summary>
		/// <param name="dataItem">The data item.</param>
		/// <returns></returns>
		protected override ListItem CreateItem(object dataItem)
		{
			Enum enumDataItem = (Enum)dataItem;

			ListItem item = new ListItem();
			item.Value = enumDataItem.ToString("d");

			if (!String.IsNullOrEmpty(DataTextFormatString))
			{
				item.Text = HttpUtilityExt.GetResourceString(String.Format(DataTextFormatString, dataItem));
			}
			else
			{
				item.Text = enumDataItem.ToString();
			}

			return item;
		}
		#endregion

		#region EnsureEmptyItem (private)
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
	}
}

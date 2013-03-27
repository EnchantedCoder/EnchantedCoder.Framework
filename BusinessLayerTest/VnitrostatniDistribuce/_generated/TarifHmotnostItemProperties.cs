//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Havit.Business;
using Havit.Business.Query;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlServer;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest.VnitrostatniDistribuce
{
	/// <summary>
	/// Objektová reprezentace metadat vlastností typu TarifHmotnostItem.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public class TarifHmotnostItemProperties
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public TarifHmotnostItemProperties()
		{
			_id = new FieldPropertyInfo();
			_cenaAmount = new FieldPropertyInfo();
			_cenaCurrency = new ReferenceFieldPropertyInfo();
			_all = new PropertyInfoCollection(_id, _cenaAmount, _cenaCurrency);
		}
		
		/// <summary>
		/// Inicializuje hodnoty metadat.
		/// </summary>
		public void Initialize(ObjectInfo objectInfo)
		{
			_id.Initialize(objectInfo, "ID", "TarifHmotnostItemID", true, SqlDbType.Int, false, 4);
			_cenaAmount.Initialize(objectInfo, "CenaAmount", "CenaAmount", false, SqlDbType.Money, false, 8);
			_cenaCurrency.Initialize(objectInfo, "CenaCurrency", "CenaCurrencyID", false, SqlDbType.Int, false, 4, typeof(Havit.BusinessLayerTest.Currency), Havit.BusinessLayerTest.Currency.ObjectInfo);
		}
		
		/// <summary>
		/// Identifikátor objektu.
		/// </summary>
		public FieldPropertyInfo ID
		{
			get
			{
				return _id;
			}
		}
		private FieldPropertyInfo _id;
		
		/// <summary>
		/// Cena.
		/// </summary>
		public FieldPropertyInfo CenaAmount
		{
			get
			{
				return _cenaAmount;
			}
		}
		private FieldPropertyInfo _cenaAmount;
		
		/// <summary>
		/// Cena (měna).
		/// </summary>
		public ReferenceFieldPropertyInfo CenaCurrency
		{
			get
			{
				return _cenaCurrency;
			}
		}
		private ReferenceFieldPropertyInfo _cenaCurrency;
		
		/// <summary>
		/// Všechny sloupečky typu TarifHmotnostItem.
		/// </summary>
		public PropertyInfoCollection All
		{
			get
			{
				return _all;
			}
		}
		private PropertyInfoCollection _all;
		
	}
}

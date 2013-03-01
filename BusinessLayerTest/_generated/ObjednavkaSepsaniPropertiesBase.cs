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

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Objektová reprezentace metadat vlastností typu ObjednavkaSepsani.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public class ObjednavkaSepsaniPropertiesBase
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public ObjednavkaSepsaniPropertiesBase()
		{
			_id = new FieldPropertyInfo();
			_stornoKomunikace = new ReferenceFieldPropertyInfo();
			_all = new PropertyInfoCollection(_id, _stornoKomunikace);
		}
		
		/// <summary>
		/// Inicializuje hodnoty metadat.
		/// </summary>
		public void Initialize(ObjectInfo objectInfo)
		{
			_id.Initialize(objectInfo, "ID", "ObjednavkaSepsaniID", true, SqlDbType.Int, false, 4);
			_stornoKomunikace.Initialize(objectInfo, "StornoKomunikace", "StornoKomunikaceID", false, SqlDbType.Int, true, 4, typeof(Havit.BusinessLayerTest.Komunikace), Havit.BusinessLayerTest.Komunikace.ObjectInfo);
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
		/// Odkaz na komunikaci, která stornuje tuto objednávku.
		/// </summary>
		public ReferenceFieldPropertyInfo StornoKomunikace
		{
			get
			{
				return _stornoKomunikace;
			}
		}
		private ReferenceFieldPropertyInfo _stornoKomunikace;
		
		/// <summary>
		/// Všechny sloupečky typu ObjednavkaSepsani.
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

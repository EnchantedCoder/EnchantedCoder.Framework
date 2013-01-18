﻿//------------------------------------------------------------------------------
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
	/// Objektová reprezentace metadat vlastností typu Role.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public class RolePropertiesBase
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public RolePropertiesBase()
		{
			_id = new FieldPropertyInfo();
			_symbol = new FieldPropertyInfo();
			_all = new PropertyInfoCollection(_id, _symbol);
		}
		
		/// <summary>
		/// Inicializuje hodnoty metadat.
		/// </summary>
		public void Initialize(ObjectInfo objectInfo)
		{
			_id.Initialize(objectInfo, "ID", "RoleID", true, SqlDbType.Int, false, 4);
			_symbol.Initialize(objectInfo, "Symbol", "Symbol", false, SqlDbType.VarChar, true, 50);
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
		/// Symbol role (název pro ASP.NET autrhorization)
		/// </summary>
		public FieldPropertyInfo Symbol
		{
			get
			{
				return _symbol;
			}
		}
		private FieldPropertyInfo _symbol;
		
		/// <summary>
		/// Všechny sloupečky typu Role.
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

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
using System.Xml;
using Havit.Business;
using Havit.Business.Query;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlServer;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Objektová reprezentace metadat vlastností typu SoftDeleteWithDateTimeOffset.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public class SoftDeleteWithDateTimeOffsetProperties
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public SoftDeleteWithDateTimeOffsetProperties()
		{
			_id = new FieldPropertyInfo();
			_deleted = new FieldPropertyInfo();
			_all = new PropertyInfoCollection(_id, _deleted);
		}
		
		/// <summary>
		/// Inicializuje hodnoty metadat.
		/// </summary>
		public void Initialize(ObjectInfo objectInfo)
		{
			_id.Initialize(objectInfo, "ID", "SoftDeleteWithDateTimeOffsetID", true, SqlDbType.Int, false, 4);
			_deleted.Initialize(objectInfo, "Deleted", "Deleted", false, SqlDbType.DateTimeOffset, true, 10);
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
		/// Čas smazání objektu.
		/// </summary>
		public FieldPropertyInfo Deleted
		{
			get
			{
				return _deleted;
			}
		}
		private FieldPropertyInfo _deleted;
		
		/// <summary>
		/// Všechny sloupečky typu SoftDeleteWithDateTimeOffset.
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

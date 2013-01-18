﻿using System;
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
	/// Kolekce business objektů typu Havit.BusinessLayerTest.ObjednavkaSepsani.
	/// </summary>
	public partial class ObjednavkaSepsaniCollection
	{
		//------------------------------------------------------------------------------
		// <auto-generated>
		//     This code was generated by a tool.
		//     Changes to this file will be lost if the code is regenerated.
		// </auto-generated>
		//------------------------------------------------------------------------------
		
		#region Constructors
		/// <summary>
		/// Vytvoří novou instanci kolekce.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public ObjednavkaSepsaniCollection() : base()
		{
		}
		
		/// <summary>
		/// Vytvoří novou instanci kolekce a zkopíruje do ní prvky z předané kolekce.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public ObjednavkaSepsaniCollection(IEnumerable<ObjednavkaSepsani> collection) : base(collection)
		{
		}
		#endregion
		
	}
}

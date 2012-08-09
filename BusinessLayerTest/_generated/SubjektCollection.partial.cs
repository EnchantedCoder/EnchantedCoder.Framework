using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;
using Havit.Business;
using Havit.Business.Query;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Kolekce business objektů typu Havit.BusinessLayerTest.Subjekt.
	/// </summary>
	public partial class SubjektCollection
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
		public SubjektCollection() : base()
		{
		}
		
		/// <summary>
		/// Vytvoří novou instanci kolekce a zkopíruje do ní prvky z předané kolekce.
		/// </summary>
		public SubjektCollection(IEnumerable<Subjekt> collection) : base(collection)
		{
		}
		#endregion
		
	}
}

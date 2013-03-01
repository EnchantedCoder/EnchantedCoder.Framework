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
	/// Rozšiřující metody.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public static partial class ExtensionMethods
	{
		#region IEnumerable<Havit.BusinessLayerTest.KomunikaceCollection>.ToCollection
		/// <summary>
		/// Vytvoří Havit.BusinessLayerTest.KomunikaceCollection z IEnumerable&lt;Havit.BusinessLayerTest.Komunikace&gt;.
		/// </summary>
		public static Havit.BusinessLayerTest.KomunikaceCollection ToCollection(this IEnumerable<Havit.BusinessLayerTest.Komunikace> objects)
		{
			return new Havit.BusinessLayerTest.KomunikaceCollection(objects);
		}
		#endregion
		
		#region IEnumerable<Havit.BusinessLayerTest.ObjednavkaSepsaniCollection>.ToCollection
		/// <summary>
		/// Vytvoří Havit.BusinessLayerTest.ObjednavkaSepsaniCollection z IEnumerable&lt;Havit.BusinessLayerTest.ObjednavkaSepsani&gt;.
		/// </summary>
		public static Havit.BusinessLayerTest.ObjednavkaSepsaniCollection ToCollection(this IEnumerable<Havit.BusinessLayerTest.ObjednavkaSepsani> objects)
		{
			return new Havit.BusinessLayerTest.ObjednavkaSepsaniCollection(objects);
		}
		#endregion
		
		#region IEnumerable<Havit.BusinessLayerTest.RoleCollection>.ToCollection
		/// <summary>
		/// Vytvoří Havit.BusinessLayerTest.RoleCollection z IEnumerable&lt;Havit.BusinessLayerTest.Role&gt;.
		/// </summary>
		public static Havit.BusinessLayerTest.RoleCollection ToCollection(this IEnumerable<Havit.BusinessLayerTest.Role> objects)
		{
			return new Havit.BusinessLayerTest.RoleCollection(objects);
		}
		#endregion
		
		#region IEnumerable<Havit.BusinessLayerTest.SubjektCollection>.ToCollection
		/// <summary>
		/// Vytvoří Havit.BusinessLayerTest.SubjektCollection z IEnumerable&lt;Havit.BusinessLayerTest.Subjekt&gt;.
		/// </summary>
		public static Havit.BusinessLayerTest.SubjektCollection ToCollection(this IEnumerable<Havit.BusinessLayerTest.Subjekt> objects)
		{
			return new Havit.BusinessLayerTest.SubjektCollection(objects);
		}
		#endregion
		
		#region IEnumerable<Havit.BusinessLayerTest.UzivatelCollection>.ToCollection
		/// <summary>
		/// Vytvoří Havit.BusinessLayerTest.UzivatelCollection z IEnumerable&lt;Havit.BusinessLayerTest.Uzivatel&gt;.
		/// </summary>
		public static Havit.BusinessLayerTest.UzivatelCollection ToCollection(this IEnumerable<Havit.BusinessLayerTest.Uzivatel> objects)
		{
			return new Havit.BusinessLayerTest.UzivatelCollection(objects);
		}
		#endregion
		
	}
}

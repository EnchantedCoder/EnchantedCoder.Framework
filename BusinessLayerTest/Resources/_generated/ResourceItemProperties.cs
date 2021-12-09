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

namespace Havit.BusinessLayerTest.Resources
{
	/// <summary>
	/// Objektová reprezentace metadat vlastností typu ResourceItem.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public class ResourceItemProperties
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public ResourceItemProperties()
		{
			_id = new FieldPropertyInfo();
			_resourceClass = new ReferenceFieldPropertyInfo();
			_resourceKey = new FieldPropertyInfo();
			_description = new FieldPropertyInfo();
			_localizations = new CollectionPropertyInfo();
			_all = new PropertyInfoCollection(_id, _resourceClass, _resourceKey, _description, _localizations);
		}
		
		/// <summary>
		/// Inicializuje hodnoty metadat.
		/// </summary>
		public void Initialize(ObjectInfo objectInfo)
		{
			_id.Initialize(objectInfo, "ID", "ResourceItemID", true, SqlDbType.Int, false, 4);
			_resourceClass.Initialize(objectInfo, "ResourceClass", "ResourceClassID", false, SqlDbType.Int, false, 4, typeof(Havit.BusinessLayerTest.Resources.ResourceClass), Havit.BusinessLayerTest.Resources.ResourceClass.ObjectInfo);
			_resourceKey.Initialize(objectInfo, "ResourceKey", "ResourceKey", false, SqlDbType.VarChar, false, 100);
			_description.Initialize(objectInfo, "Description", "Description", false, SqlDbType.NVarChar, true, 200);
			_localizations.Initialize(objectInfo, "Localizations", typeof(Havit.BusinessLayerTest.Resources.ResourceItemLocalization), "(SELECT CAST([_items].[ResourceItemLocalizationID] AS NVARCHAR(11)) + '|' FROM [dbo].[ResourceItemLocalization] AS [_items] WHERE ([_items].[ResourceItemID] = [dbo].[ResourceItem].[ResourceItemID]) FOR XML PATH('')) AS [Localizations]");
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
		/// Třída resources.
		/// </summary>
		public ReferenceFieldPropertyInfo ResourceClass
		{
			get
			{
				return _resourceClass;
			}
		}
		private ReferenceFieldPropertyInfo _resourceClass;
		
		/// <summary>
		/// Klíč položky v rámci ResourceClass
		/// </summary>
		public FieldPropertyInfo ResourceKey
		{
			get
			{
				return _resourceKey;
			}
		}
		private FieldPropertyInfo _resourceKey;
		
		/// <summary>
		/// Popis pro administraci.
		/// </summary>
		public FieldPropertyInfo Description
		{
			get
			{
				return _description;
			}
		}
		private FieldPropertyInfo _description;
		
		/// <summary>
		/// Lokalizované hodnoty.
		/// </summary>
		public CollectionPropertyInfo Localizations
		{
			get
			{
				return _localizations;
			}
		}
		private CollectionPropertyInfo _localizations;
		
		/// <summary>
		/// Všechny sloupečky typu ResourceItem.
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

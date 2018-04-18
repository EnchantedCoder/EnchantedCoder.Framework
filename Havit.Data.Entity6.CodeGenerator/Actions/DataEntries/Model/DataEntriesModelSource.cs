﻿using System;
using System.Collections.Generic;
using System.Linq;
using Havit.Data.Entity.CodeGenerator.Entity;
using Havit.Data.Entity.CodeGenerator.Services;
using Havit.Data.Entity.Mapping.Internal;

namespace Havit.Data.Entity.CodeGenerator.Actions.DataEntries.Model
{
	public class DataEntriesModelSource : IModelSource<DataEntriesModel>
	{
		private readonly DbContext dbContext;
		private readonly Project modelProject;
		private readonly Project dataLayerProject;
		private readonly CammelCaseNamingStrategy cammelCaseNamingStrategy;

		public DataEntriesModelSource(DbContext dbContext, Project modelProject, Project dataLayerProject, CammelCaseNamingStrategy cammelCaseNamingStrategy)
		{
			this.dbContext = dbContext;
			this.modelProject = modelProject;
			this.dataLayerProject = dataLayerProject;
			this.cammelCaseNamingStrategy = cammelCaseNamingStrategy;
		}

		public IEnumerable<DataEntriesModel> GetModels()
		{
			return (from registeredEntity in dbContext.Db()
				let entriesEnumType = GetEntriesEnum(registeredEntity.Type)
				where (entriesEnumType != null)
				select new DataEntriesModel
				{
					UseDataEntrySymbolStorage = registeredEntity.HasDatabaseGeneratedIdentity,
					NamespaceName = GetNamespaceName(registeredEntity.NamespaceName),
					InterfaceName = "I" + registeredEntity.ClassName + "Entries",
					DbClassName = registeredEntity.ClassName + "Entries",
					ModelClassFullName = registeredEntity.FullName,
					ModelEntriesEnumerationFullName = registeredEntity.FullName + ".Entry",
					Entries = System.Enum.GetNames(entriesEnumType).OrderBy(item => item).Select(item => new DataEntriesModel.Entry
					{
						PropertyName = item,
						FieldName = cammelCaseNamingStrategy.GetCammelCase(item)
					}).ToList()
				}).ToList();
		}

		private Type GetEntriesEnum(Type type)
		{
			Type entriesType = type.GetNestedType("Entry"); // TODO: Duplikovaný kód
			if ((entriesType != null) && (entriesType.IsEnum))
			{
				return entriesType;
			}
			return null;
		}

		private string GetNamespaceName(string namespaceName)
		{
			string modelProjectNamespace = modelProject.GetProjectRootNamespace();
			if (namespaceName.StartsWith(modelProjectNamespace))
			{
				return dataLayerProject.GetProjectRootNamespace() + ".DataEntries" + namespaceName.Substring(modelProjectNamespace.Length);
			}
			else
			{
				return namespaceName + ".DataSources";
			}
		}
	}
}

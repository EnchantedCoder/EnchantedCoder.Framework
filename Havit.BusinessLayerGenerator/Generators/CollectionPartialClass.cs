﻿using Havit.Business.BusinessLayerGenerator.Csproj;
using Havit.Business.BusinessLayerGenerator.Helpers;
using Havit.Business.BusinessLayerGenerator.TfsClient;
using Havit.Business.BusinessLayerGenerator.Writers;
using Microsoft.SqlServer.Management.Smo;

namespace Havit.Business.BusinessLayerGenerator.Generators
{
	public static class CollectionPartialClass
	{
		#region Generate
		public static void Generate(Table table, CsprojFile csprojFile, SourceControlClient sourceControlClient)
		{
			string fileName = FileHelper.GetFilename(table, "Collection.partial.cs", FileHelper.GeneratedFolder);

			if (csprojFile != null)
			{
				csprojFile.Ensures(fileName);
			}

			CodeWriter writer = new CodeWriter(FileHelper.ResolvePath(fileName), sourceControlClient);
			Autogenerated.WriteAutogeneratedNoCodeHere(writer);
			BusinessObjectUsings.WriteUsings(writer);
			CollectionBaseClass.WriteNamespaceBegin(writer, table);
			CollectionClass.WriteClassBegin(writer, table, true);

			CollectionBaseClass.WriteConstructors(writer, table, false);

			Autogenerated.WriteAutogeneratedNoCodeHere(writer);

			CollectionBaseClass.WriteClassEnd(writer);
			CollectionBaseClass.WriteNamespaceEnd(writer);
			
			writer.Save();
		}
		#endregion
	}
}

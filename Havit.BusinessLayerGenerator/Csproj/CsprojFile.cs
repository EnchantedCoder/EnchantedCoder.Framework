﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Havit.Business.BusinessLayerGenerator.Csproj
{
	public class CsprojFile
	{
		#region MSBuildNamespace
		private readonly XNamespace MSBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
		#endregion

		#region Private fields 
		private readonly string generatorIdentifier;
		private bool contentChanged = false;
		private readonly XDocument content;
		private readonly List<string> ensuredFilenames = new List<string>();
		#endregion

		#region Filename
		public string Filename
		{
			get;
			private set;
		}

		public string Path => System.IO.Path.GetDirectoryName(Filename);
		#endregion

		#region Constructors
		public CsprojFile(string filename, string generatorIdentifier)
		{
			this.generatorIdentifier = generatorIdentifier;
			this.Filename = filename;
			content = XDocument.Load(filename, LoadOptions.PreserveWhitespace);
			content.Changed += Content_Changed;
		}

		private CsprojFile()
		{
		}
		#endregion

		#region Content_Changed
		private void Content_Changed(object sender, XObjectChangeEventArgs e)
		{
			contentChanged = true;
		}
		#endregion
		
		#region Ensures
		public virtual void Ensures(string filename)
		{
			ensuredFilenames.Add(filename);

			XElement itemElement = content.Root.Elements(MSBuildNamespace + "ItemGroup").Elements(MSBuildNamespace + "Compile").Where(element => element.Attributes("Include").Any(attribute => String.Equals(filename, (string)attribute, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault();

			// abychom do kódu dostali metadata HavitBusinessLayerGenerator, tak položku, která jej nemá, odstraníme (a následně přidáme spolu s ní)
			if (itemElement != null)
			{
				if (itemElement.Elements(MSBuildNamespace + generatorIdentifier).Count() == 0)
				{
					this.RemoveWithNextWhitespace(itemElement);
					itemElement = null;
				}
			}

			if (itemElement == null)
			{
				// najdeme první element ItemGroup obsahující sub-element Compile.
				XElement itemgroup = content.Root.Elements(MSBuildNamespace + "ItemGroup").Where(element => element.Elements(MSBuildNamespace + "Compile").Count() > 0).FirstOrDefault();

				if (itemgroup != null)				
				{
					// za poslední element dáme konec řádku, odsazení a element
					itemgroup.Elements().Last().AddAfterSelf(
						new XText("\n"),
						new XText("    "),
						new XElement(MSBuildNamespace + "Compile", 
							new XAttribute("Include", filename),
							new XText("\n"),
							new XText("      "),
							new XElement(MSBuildNamespace + "SubType", "Code"),
							new XText("\n"),
							new XText("      "),
							new XElement(MSBuildNamespace + generatorIdentifier, "1"),
							new XText("\n"),
							new XText("    ")));
				}
			}
		}
		#endregion

		#region Remove
		public virtual void Remove(string filename)
		{
			// Odstraníme všechny ItemGroupy, které obsahují jakýkoli element Compile s atributem Include rovným názvu souboru.
			// protože odstranění za sebou nechá whitespaces (díky LoadOptions.PreserveWhitespaces), odstraníme element vč. následucího whitespace
			content.Root.Elements(MSBuildNamespace + "ItemGroup").Elements(MSBuildNamespace + "Compile").Where(element => element.Attributes("Include").Any(attribute => attribute.Value == filename)).ToList().ForEach(element => this.RemoveWithNextWhitespace(element));
		}
		#endregion

		#region RemoveUnusedGeneratedFiles
		public void RemoveUnusedGeneratedFiles()
		{
			// najdeme všechny elementy Compile v ItemGroup, které mají sub-element HavitBusinessLayerGenerator, ale nejsou v seznamu generovaných souborů
			content.Root
				.Elements(MSBuildNamespace + "ItemGroup")
				.Elements(MSBuildNamespace + "Compile")
				.Where(element =>
					(element.Elements(generatorIdentifier).Count() > 0)
					&& !element.Attributes("Include").Any(attribute => ensuredFilenames.Contains(attribute.Value)))
				.ToList()
				.ForEach(element => this.RemoveWithNextWhitespace(element));
		}
		#endregion

		#region SaveChanges
		public virtual void SaveChanges()
		{
			if (contentChanged)
			{
				content.Save(Filename);
			}
		}
		#endregion

		#region RemoveWithNextWhitespace
		private void RemoveWithNextWhitespace(XElement element)
		{
			IEnumerable<XNode> textNodes
				= element.NodesAfterSelf().TakeWhile(node => node is XText);
			if (element.ElementsAfterSelf().Any())
			{
				// Easy case, remove following text nodes.
				textNodes.ToList().ForEach(node => node.Remove());
			}
			else
			{
				// Remove trailing whitespace.
				textNodes.Cast<XText>().TakeWhile(text => !text.Value.Contains("\n"))
						 .ToList().ForEach(text => text.Remove());
				// Fetch text node containing newline, if any.
				XText newLineTextNode
					= element.NodesAfterSelf().OfType<XText>().FirstOrDefault();
				if (newLineTextNode != null)
				{
					string value = newLineTextNode.Value;
					if (value.Length > 1)
					{
						// Composite text node, trim until newline (inclusive).
						newLineTextNode.AddAfterSelf(
							new XText(value.Substring(value.IndexOf('\n') + 1)));
					}
					// Remove original node.
					newLineTextNode.Remove();
				}
			}
			element.Remove();
		}
		#endregion

		#region GetByFolder
		public static CsprojFile GetByFolder(string folder, string generatorIdentifier = "HavitBusinessLayerGenerator")
		{
			var files = System.IO.Directory.GetFiles(folder, "*.csproj");
			if (files.Length == 1)
			{
				return new CsprojFile(files[0], generatorIdentifier);
			}
			return null;
		}
		#endregion

	}
}

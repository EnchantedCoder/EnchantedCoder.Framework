using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Havit.Xml.Rss
{
	/// <summary>
	/// Hlavn� t��da reprezentuj�c� cel� RSS dokument.
	/// RSS verze 2.0
	/// Specifikace na http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	[XmlRoot("rss")]
	public class RssFeed
	{
		#region Fields

		private string version = "";
		private RssChannelCollection channels;

		/// <summary>
		/// Atribut "version", nastaven na implementovanou verzi.
		/// </summary>
		[XmlAttribute("version")]
		public string Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
			}
		}

		/// <summary>
		/// Kolekce channel� ve feedu.
		/// </summary>
		[XmlElement("channel")]
		public RssChannelCollection Channels
		{
			get
			{
				return channels;
			}
		}

		#endregion

		#region Constructor
		
		/// <summary>
		/// Vytvo�� novou instanci RSS feedu.
		/// </summary>
		public RssFeed()
		{
			this.channels = new RssChannelCollection();
			this.version = "2.0";
			
		}
		#endregion

		#region Seralization to XML

		/// <summary>
		/// Vr�t� RSS feed jako XmlDocument
		/// </summary>
		/// <returns>RSS feed jako XmlDocument</returns>
		public XmlDocument SerializeToXmlDocument()
		{
			StringWriter writer = new StringWriter(CultureInfo.CurrentCulture);
			XmlSerializer serializer = new XmlSerializer(typeof (RssFeed));
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", null);
			serializer.Serialize(writer, this, ns);

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(new StringReader(writer.ToString()));
			writer.Close();

			// Vymaz�n� pr�zdn�ch element�
			foreach(XmlNode node in xmlDocument.SelectNodes("//*[.='']"))
			{
				node.ParentNode.RemoveChild(node);
			}

			return xmlDocument;
		}

		/// <summary>
		/// Vr�t� RSS feed jako String s XML dokumentem
		/// </summary>
		/// <returns>RSS feed jako String</returns>
		public override string ToString()
		{
			XmlDocument xmlDocument = this.SerializeToXmlDocument();
			StringWriter writer = new StringWriter(CultureInfo.CurrentCulture);
			xmlDocument.Save(writer);

			string result = writer.ToString();
			writer.Close();

			return result;
		}

		#endregion
	}
}

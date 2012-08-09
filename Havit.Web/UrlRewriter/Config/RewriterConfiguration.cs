﻿using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Xml.Serialization;

namespace Havit.Web.UrlRewriter.Config
{
	/// <summary>
	/// Specifies the configuration settings in the Web.config for the RewriterRule.
	/// </summary>
	/// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
	/// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
	/// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
	/// constant names and values, which could then be referenced in rewrite rules.
	/// <p />
	/// The structure in the Web.config file is as follows:
	/// <code>
	/// &lt;configuration&gt;
	/// 	&lt;configSections&gt;
	/// 		&lt;section name="UrlRewriterConfig" 
	/// 		            type="Havit.Web.UrlRewriter.Config.RewriterConfigSerializerSectionHandler, Havit.Web" /&gt;
	///		&lt;/configSections&gt;
	///		
	///		&lt;UrlRewriterConfig&gt;
	///			&lt;Rules&gt;
	///				&lt;RewriterRule&gt;
	///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
	///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
	///				&lt;/RewriterRule&gt;
	///				&lt;RewriterRule&gt;
	///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
	///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
	///				&lt;/RewriterRule&gt;
	///				...
	///				&lt;RewriterRule&gt;
	///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
	///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
	///				&lt;/RewriterRule&gt;
	///			&lt;/Rules&gt;
	///		&lt;/UrlRewriterConfig&gt;
	///		
	///		&lt;system.web&gt;
	///			...
	///		&lt;/system.web&gt;
	///	&lt;/configuration&gt;
	/// </code>
	/// </remarks>
	[Serializable()]
	[XmlRoot("UrlRewriterConfig")]
	public class RewriterConfiguration
	{
		// private member variables
		private RewriterRuleCollection rules;			// an instance of the RewriterRuleCollection class...

		/// <summary>
		/// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
		/// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
		/// a <b>RewriterConfiguration</b> instance.
		/// </summary>
		/// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
		public static RewriterConfiguration GetConfig()
		{
			const string sectionName = "UrlRewriterConfig";

			RewriterConfiguration result = (RewriterConfiguration)HttpContext.Current.Cache[sectionName];

			if (result == null)
			{
				result = (RewriterConfiguration)ConfigurationManager.GetSection(sectionName);
				HttpContext.Current.Cache[sectionName] = result;
			}

			return result;
		}

		#region Public Properties
		/// <summary>
		/// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
		/// </summary>
		public RewriterRuleCollection Rules
		{
			get
			{
				return rules;
			}
			set
			{
				rules = value;
			}
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EnchantedCoder.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantedCoder.Extensions.DependencyInjection
{
	/// <summary>
	/// Service to register with lifetime and service type.
	/// </summary>
	[DebuggerDisplay("Type={Type}, ServiceAttribute = {ServiceAttribute}")]
	public class TypeServiceAttributeInfo
	{
		/// <summary>
		/// Service to register.
		/// </summary>
		public Type Type { get; set; }


		/// <summary>
		/// ServiceAttribute with service lifetime and service types.
		/// </summary>
		public ServiceAttributeBase ServiceAttribute { get; set; }
	}
}

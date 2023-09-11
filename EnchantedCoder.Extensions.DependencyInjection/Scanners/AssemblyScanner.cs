using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnchantedCoder.Extensions.DependencyInjection.Abstractions;

namespace EnchantedCoder.Extensions.DependencyInjection.Scanners
{
	/// <summary>
	/// Scans assembly for types to register.
	/// </summary>
	public static class AssemblyScanner
	{
		/// <summary>
		/// Scans assembly for classes with ServiceAttribute and the given profile.
		/// </summary>
		public static TypeServiceAttributeInfo[] GetTypesWithServiceAttribute(Assembly assembly, string profile)
		{
			return (from type in assembly.GetTypes()
					from serviceAttribute in type.GetCustomAttributes(typeof(ServiceAttributeBase), false).Cast<ServiceAttributeBase>()
					where (serviceAttribute.Profile == profile)
					select new TypeServiceAttributeInfo { Type = type, ServiceAttribute = serviceAttribute })
					.ToArray();
		}
	}
}

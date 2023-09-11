﻿using System;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Formatters;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Processors;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Services;
using EnchantedCoder.AspNetCore.Mvc.ErrorToJson.Configuration;
using EnchantedCoder.AspNetCore.Mvc.ErrorToJson.Services;
using EnchantedCoder.AspNetCore.Mvc.ExceptionMonitoring;
using Microsoft.Extensions.Configuration;

// Správný namespace je Microsoft.Extensions.DependencyInjection!

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension metody pro registraci ErrorToJson.
	/// </summary>
	public static class ErrorToJsonApplicationServiceCollectionExtensions
	{
		/// <summary>
		/// Zaregistruje služby pro ErrorToJson.
		/// </summary>
		public static void AddErrorToJson(this IServiceCollection services, Action<ErrorToJsonSetup> setupAction)
		{
			ErrorToJsonSetup setup = new ErrorToJsonSetup();
			setupAction.Invoke(setup);
			var configuration = setup.GetConfiguration();

			services.AddSingleton<IErrorToJsonService>(new ErrorToJsonService(configuration));
		}
	}
}

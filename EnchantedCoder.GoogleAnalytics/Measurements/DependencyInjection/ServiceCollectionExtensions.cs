using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.GoogleAnalytics.Measurements;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantedCoder.GoogleAnalytics.Measurements.DependencyInjection
{
	/// <summary>
	/// Static extension class for <see cref="IServiceCollection"/> extensions
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers <see cref="IGoogleAnalyticsMeasurementApiClient"/> and dependent services with transient lifestyle into <paramref name="services"/>
		/// </summary>
		/// <param name="services">Service container</param>
		public static void AddGoogleAnalyticMeasurementApiClient(this IServiceCollection services)
		{
			services
				.AddTransient<IHttpRequestSender, HttpClientRequestSender>()
				.AddTransient<IGoogleAnalyticsMeasurementApiClient, GoogleAnalyticsMeasurementApiClient>();
		}
	}
}

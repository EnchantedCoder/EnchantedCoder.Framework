using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.GoogleAnalytics.Measurements;
using EnchantedCoder.GoogleAnalytics.Measurements.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.GoogleAnalytics.Tests.Measurements
{
	[TestClass]
	public class ServiceCollectionTests
	{
		[TestMethod]
		public void ServiceCollection_CanResolve_IGoogleAnalyticsMeasurementApiClient()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddSingleton<IGoogleAnalyticsMeasurementApiConfiguration>(new FakeGoogleAnalyticsMeasurementApiConfiguration());
			services.AddGoogleAnalyticMeasurementApiClient();
			var provider = services.BuildServiceProvider();
			var client = provider.GetRequiredService<IGoogleAnalyticsMeasurementApiClient>();

			Assert.IsNotNull(client);
		}
	}
}

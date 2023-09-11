﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Processors;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EnchantedCoder.AspNetCore.Tests.ExceptionMonitoring.ExceptionMonitoringApplicationBuilderExtensions
{
	[TestClass]
	public class ExceptionMonitoringServiceCollectionExtensionsTests
	{
		[TestMethod]
		public void ExceptionMonitoringServiceCollectionExtensions_AddExceptionMonitoring_RegistersServices()
		{
			// Arrange
			IServiceCollection services = new ServiceCollection();
			IConfigurationRoot configuration = new ConfigurationBuilder().Build();
			Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

			// Act
			services.AddExceptionMonitoring(configuration);
			services.AddSingleton<IHttpContextAccessor>(httpContextAccessorMock.Object);
			services.AddLogging();
			var serviceProvider = services.BuildServiceProvider();

			// Assert
			Assert.IsNotNull(serviceProvider.GetService<IExceptionMonitoringService>());
			Assert.IsInstanceOfType(serviceProvider.GetService<IExceptionMonitoringProcessor>(), typeof(BufferingSmtpExceptionMonitoringProcessor));
		}
	}
}

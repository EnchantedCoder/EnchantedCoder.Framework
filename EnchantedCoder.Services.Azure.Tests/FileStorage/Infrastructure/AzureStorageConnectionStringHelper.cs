using System;
using Azure.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;

namespace EnchantedCoder.Services.Azure.Tests.FileStorage.Infrastructure
{
	public static class AzureStorageConnectionStringHelper
	{
		public static string GetConnectionString() => AzureBlobStorageConnectionStringLazy.Value;

		private static Lazy<string> AzureBlobStorageConnectionStringLazy = new Lazy<string>(() =>
		{
			var config = new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.Build();
			string connectionString = config.GetConnectionString("AzureStorage");

			if (connectionString is null)
			{
				config = new ConfigurationBuilder()
					.AddAzureKeyVault(new Uri("https://EnchantedCoderFrameworkConfigKV.vault.azure.net"), new DefaultAzureCredential())
					.Build();
				connectionString = config.GetConnectionString("AzureStorage");
			}

			if (connectionString is null)
			{
				throw new InvalidOperationException("Couldn't find Azure storage connection string in the configuration.");
			}

			return connectionString;
		});
	}
}

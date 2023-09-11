using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.Patterns.DataSeeds;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure.Factories
{
	internal class DataSeedPersisterFactory : IDataSeedPersisterFactory
	{
		private readonly IServiceProvider serviceProvider;

		public DataSeedPersisterFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IDataSeedPersister CreateService()
		{
			return serviceProvider.GetRequiredService<IDataSeedPersister>();
		}

		public void ReleaseService(IDataSeedPersister service)
		{
			// NOOP
		}
	}
}

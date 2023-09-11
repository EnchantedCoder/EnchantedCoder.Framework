using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.UnitOfWorks.BeforeCommitProcessors;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure.Factories
{
	internal class BeforeCommitProcessorsFactory : IBeforeCommitProcessorsFactory
	{
		private readonly IServiceProvider serviceProvider;

		public BeforeCommitProcessorsFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IEnumerable<IBeforeCommitProcessor<TEntity>> Create<TEntity>()
			where TEntity : class
		{
			return serviceProvider.GetRequiredService<IEnumerable<IBeforeCommitProcessor<TEntity>>>();
		}
	}
}

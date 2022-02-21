﻿using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure.Factories;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks.BeforeCommitProcessors;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks.EntityValidation;
using Havit.Data.Patterns.DataSeeds;
using Havit.Data.Patterns.DataSources;
using Havit.Data.Patterns.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure
{
	/// <summary>
	/// Třída pro registraci služeb do dependency injection containeru, kterým je ServiceCollection.
	/// </summary>
	internal class ServiceCollectionServiceInstaller : ServiceInstallerBase
	{
		private readonly IServiceCollection services;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public ServiceCollectionServiceInstaller(IServiceCollection services)
		{
			this.services = services;
		}

		public override void TryAddFactory(Type factoryType)
		{
			if (factoryType == typeof(IDataSeedPersisterFactory))
			{
				services.TryAddTransient<IDataSeedPersisterFactory, DataSeedPersisterFactory>();
			}
			else if (factoryType == typeof(IBeforeCommitProcessorsFactory))
			{
				services.TryAddTransient<IBeforeCommitProcessorsFactory, BeforeCommitProcessorsFactory>();
			}
			else if (factoryType == typeof(IEntityValidatorsFactory))
			{
				services.TryAddTransient<IEntityValidatorsFactory, EntityValidatorsFactory>();
			}
			else
			{
				throw new ArgumentException($"Factory type {factoryType} is not supported.", nameof(factoryType));
			}
		}

		/// <inheritdoc/>
		public override void AddService(Type serviceType, Type implementationType, ServiceLifetime lifetime)
		{
			services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
		}

		/// <inheritdoc/>
		public override void AddServiceSingletonInstance(Type serviceType, object implementation)
		{
			services.AddSingleton(serviceType, implementation);
		}

		/// <inheritdoc/>
		protected override void AddMultipleServices(Type[] serviceTypes, Type implementationType, ServiceLifetime lifetime)
		{
			if (lifetime == ServiceLifetime.Transient)
			{
				foreach (var serviceType in serviceTypes)
				{
					services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
				}
				return;
			}

			// je Scoped nebo Singleton a zároveň máme více interfaces
			var firstServiceTypeToRegister = serviceTypes.First();

			// registrace prvního interface
			services.Add(new ServiceDescriptor(firstServiceTypeToRegister, implementationType, lifetime /* Scoped nebo Singleton */));

			// registrace druhého a dalšího interface
			foreach (var serviceType in serviceTypes.Skip(1) /* až od druhého */)
			{
				services.AddTransient(serviceType, sp => sp.GetRequiredService(firstServiceTypeToRegister));
			}
		}

		/// <inheritdoc/>
		public override void TryAddService(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
			services.TryAdd(new ServiceDescriptor(serviceType, implementationType, lifetime));
		}
    }
}

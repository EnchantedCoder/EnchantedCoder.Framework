﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnchantedCoder.Hangfire.Extensions.RecurringJobs;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace EnchantedCoder.Hangfire.Extensions.BackgroundJobs;

/// <summary>
/// Methods to help with background jobs.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Deletes all enqueued jobs in a queue.
	/// </summary>
	public static void AddHangfireEnqueuedJobsCleanupOnApplicationStartup(this IServiceCollection services, string queue = "default")
	{
		AddHangfireEnqueuedJobsCleanupOnApplicationStartup(services, new string[] { queue });
	}

	/// <summary>
	/// Deletes all enqueued jobs in a queues.
	/// </summary>
	public static void AddHangfireEnqueuedJobsCleanupOnApplicationStartup(this IServiceCollection services, string[] queues)
	{
		services.TryAddSingleton<IBackgroundJobManager, BackgroundJobManager>();
		services.AddHostedService<EnqueuedJobsCleanupOnApplicationStartup>();
		services.PostConfigure<EnqueuedJobsCleanupOnApplicationStartupOptions>(options => options.Queues.AddRange(queues));
	}
}

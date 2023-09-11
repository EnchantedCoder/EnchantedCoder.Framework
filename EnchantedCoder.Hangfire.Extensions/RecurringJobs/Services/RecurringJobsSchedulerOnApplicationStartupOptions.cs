using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.Extensions.Hosting;

namespace EnchantedCoder.Hangfire.Extensions.RecurringJobs.Services;

/// <summary>
/// Options for RecurringJobsSchedulerOnApplicationStartup.
/// </summary>
internal class RecurringJobsSchedulerOnApplicationStartupOptions
{
	/// <summary>
	/// Recurring jobs to schedule.
	/// </summary>
	public List<IRecurringJob> RecurringJobs { get; } = new List<IRecurringJob>();
}

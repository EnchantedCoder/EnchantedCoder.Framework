﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnchantedCoder.Diagnostics.Contracts;
using EnchantedCoder.Hangfire.Extensions.RecurringJobs.Services;
using Hangfire;

namespace EnchantedCoder.Hangfire.Extensions.RecurringJobs;

/// <summary>
/// Recurring job to run another recurring jobs in sequence.
/// Why running recurring jobs and not a background jobs? Because running recurring jobs updates the state on the hangfire dashboard!
/// </summary>
public class SequenceRecurringJob : IRecurringJob
{
	/// <summary>
	/// Job identifier.
	/// </summary>
	public string JobId { get; }

	/// <summary>
	/// Cron expression.
	/// </summary>
	public string CronExpression { get; }

	/// <summary>
	/// RecurringJobs to run in sequence.
	/// </summary>
	public IRecurringJob[] RecurringJobsToRunInSequence { get; }

	/// <summary>
	/// Job continuation options (used to configure whether to continue when any job fails).
	/// </summary>
	public JobContinuationOptions JobContinuationOptions { get; }

	/// <summary>
	/// Time zone info.
	/// </summary>
	public TimeZoneInfo TimeZone { get; }

	/// <summary>
	/// Queue name.
	/// </summary>
	public string Queue { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	public SequenceRecurringJob(string jobId, string cronExpression, IRecurringJob[] recurringJobsToRunInSequence, JobContinuationOptions jobContinuationOptions = JobContinuationOptions.OnAnyFinishedState, TimeZoneInfo timeZone = null, string queue = "default")
	{
		Contract.Requires((recurringJobsToRunInSequence != null) && recurringJobsToRunInSequence.Any());

		this.JobId = jobId;
		this.CronExpression = cronExpression;
		this.JobContinuationOptions = jobContinuationOptions;
		this.TimeZone = timeZone;
		this.Queue = queue;
		this.RecurringJobsToRunInSequence = recurringJobsToRunInSequence;
	}

	/// <inheritdoc />
	public async Task RunAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
	{
		List<Exception> exceptions = new List<Exception>();

		foreach (var recurringJob in RecurringJobsToRunInSequence)
		{
			cancellationToken.ThrowIfCancellationRequested();

			try
			{
				await recurringJob.RunAsync(serviceProvider, cancellationToken);
			}
			catch (Exception exception) when (exception is not OperationCanceledException)
			{
				exceptions.Add(exception);
			}
		}

		if (exceptions.Any())
		{
			throw new AggregateException(exceptions);
		}
	}

	/// <inheritdoc />
	public void ScheduleAsRecurringJob(IRecurringJobManager recurringJobManager)
	{
		recurringJobManager.AddOrUpdate<ISequenceRecurringJobScheduler>(this.JobId, planner => planner.ProcessRecurryingJobsInQueue(JobId, RecurringJobsToRunInSequence.Select(item => item.JobId).ToArray(), this.JobContinuationOptions), CronExpression, TimeZone, Queue);
	}
}

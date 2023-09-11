﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;

namespace EnchantedCoder.Hangfire.Extensions.BackgroundJobs;

/// <inheritdoc />
public class BackgroundJobManager : IBackgroundJobManager
{
	private readonly IBackgroundJobClient backgroundJobClient;
	private readonly JobStorage jobStorage;

	/// <summary>
	/// Constructor.
	/// </summary>
	public BackgroundJobManager(IBackgroundJobClient backgroundJobClient, JobStorage jobStorage)
	{
		this.backgroundJobClient = backgroundJobClient;
		this.jobStorage = jobStorage;
	}

	/// <inheritdoc />
	public void DeleteEnqueuedJobs(string queue = "default")
	{
		// remove enqueued jobs
		List<string> toDelete = new List<string>();
		IMonitoringApi monitor = jobStorage.GetMonitoringApi();
		if (monitor.Queues().Any())
		{
			QueueWithTopEnqueuedJobsDto queueWithTopEnqueuedJobs = monitor.Queues().SingleOrDefault(q => q.Name == queue); // get the single queue by name

			if (queueWithTopEnqueuedJobs != null)
			{
				Enumerable.Range(0, (int)Math.Ceiling(queue.Length / 1000d)) // lets batch items by 1000
					.SelectMany(batchIndex => monitor.EnqueuedJobs(queueWithTopEnqueuedJobs.Name, 1000 * batchIndex, 1000)) // select jobs in a batch by batchIndex
					.Select(jobEntry => jobEntry.Key) // select JobId (key is a JobId)
					.ToList() // process all pages to memory
					.ForEach(jobId => backgroundJobClient.Delete(jobId)); // remove the job
			}
		}
	}
}
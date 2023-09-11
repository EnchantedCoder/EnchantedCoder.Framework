using System;
using System.Threading.Tasks;
using EnchantedCoder.AspNetCore.ExceptionMonitoring.Services;
using EnchantedCoder.Diagnostics.Contracts;

namespace EnchantedCoder.AspNetCore.ExceptionMonitoring.ExceptionHandlers
{
	internal class UnobservedTaskExceptionHandler
	{
		private static UnobservedTaskExceptionHandler ExceptionHandler { get; set; }

		private readonly IExceptionMonitoringService exceptionMonitoringService;

		public UnobservedTaskExceptionHandler(IExceptionMonitoringService exceptionMonitoringService)
		{
			this.exceptionMonitoringService = exceptionMonitoringService;
		}

		public static void RegisterHandler(IExceptionMonitoringService exceptionMonitoringService)
		{
			Contract.Requires<ArgumentNullException>(exceptionMonitoringService != null);

			if (ExceptionHandler != null)
			{
				throw new InvalidOperationException("Handler for unobserved task exception is already registered.");
			}

			var handler = new UnobservedTaskExceptionHandler(exceptionMonitoringService);
			ExceptionHandler = handler;

			TaskScheduler.UnobservedTaskException += handler.TaskScheduler_UnobservedTaskException;
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			exceptionMonitoringService.HandleException(e.Exception);
		}
	}
}
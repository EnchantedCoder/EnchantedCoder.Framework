using System;
using Havit.AspNetCore.Mvc.ExceptionMonitoring.Filters;
using Havit.AspNetCore.Mvc.ExceptionMonitoring.Processors;
using Microsoft.Extensions.Logging;

namespace Havit.AspNetCore.Mvc.ExceptionMonitoring.Services
{
	/// <summary>
	/// Exception monitoring.
	/// Zpracuje v�jimku p�ed�n�m exception monitoring processor�m.
	/// </summary>
    public class ExceptionMonitoringService : IExceptionMonitoringService
    {
        private readonly IExceptionMonitoringProcessor[] exceptionMonitoringProcessors;
        private readonly ILogger<ErrorMonitoringFilter> logger;

		/// <summary>
		/// Konstruktor.
		/// </summary>
        public ExceptionMonitoringService(IExceptionMonitoringProcessor[] exceptionMonitoringProcessors, ILogger<ErrorMonitoringFilter> logger)
        {
            this.exceptionMonitoringProcessors = exceptionMonitoringProcessors;
            this.logger = logger;
        }

		/// <summary>
		/// Zpracuje v�jimku p�ed�n�m exception monitoring processor�m.
		/// </summary>
        public void HandleException(Exception exception)
        {
            using (logger.BeginScope("Exception Monitoring"))
            {
                logger.LogDebug(0, "Processing exception of type {TYPE} with message '{MESSAGE}'.", exception.GetType().FullName, exception.Message);
                bool shouldMonitorException = ShouldHandleException(exception);
                logger.LogDebug("Exception ShouldBeMonitored = {SHOULDBEMONITORED}.", shouldMonitorException);

                if (ShouldHandleException(exception))
                {
                    if (exceptionMonitoringProcessors.Length == 0)
                    {
                        logger.LogWarning("No exception monitor registered.");
                    }

                    foreach (IExceptionMonitoringProcessor exceptionMonitor in exceptionMonitoringProcessors)
                    {
                        try
                        {
                            logger.LogDebug("Processing exception monitor {TYPE}.", exception.GetType().FullName);
                            exceptionMonitor.ProcessException(exception);
                        }
                        catch (Exception raisedException)
                        {
                            logger.LogError(0, raisedException, "Exception monitor {TYPE} failed with message {MESSAGE}", raisedException.GetType().FullName, raisedException.Message);
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Vrac� true, pokud se m� v�jimka zpracov�vat (p�ed�vat procesor�m).
		/// V�dy vrac� true, ale umo��uje potomk�m chov�n� p�edefinovat.
		/// </summary>
        protected virtual bool ShouldHandleException(Exception exception)
        {
            return true;
        }

    }
}

using System;

namespace Havit.AspNetCore.Mvc.ExceptionMonitoring.Services
{
	/// <summary>
	/// Exception Monitoring.
	/// </summary>
    public interface IExceptionMonitoringService
    {
		/// <summary>
		/// Zpracuje p�edanou v�jimku.
		/// </summary>
        void HandleException(Exception exception);
    }
}
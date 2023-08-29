using System;

namespace EnchantedCoder.AspNetCore.Mvc.ErrorToJson.Services
{
	/// <summary>
	/// Provides object result for exception.
	/// </summary>
	public interface IErrorToJsonService
	{
		/// <summary>
		/// Returns object data for exception. If no response specified, returns null.
		/// </summary>
		ResultData GetResultData(Exception exception);
	}
}
﻿using EnchantedCoder.AspNetCore.Mvc.ErrorToJson.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.AspNetCore.Mvc.ErrorToJson.Services
{
	/// <summary>
	/// Provides object result for exception.
	/// </summary>
	public class ErrorToJsonService : IErrorToJsonService
	{
		private ErrorToJsonConfiguration configuration;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ErrorToJsonService(ErrorToJsonConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <inheritdoc />
		public ResultData GetResultData(Exception exception)
		{
			var mappingItem = this.configuration.FindMapping(exception);
			if (mappingItem != null)
			{
				object result = mappingItem.ResultSelector(exception);
				bool markExceptionAsHandled = mappingItem.MarkExceptionAsHandledFunc(exception);

				return new ResultData
				{
					Data = result,
					StatusCode = mappingItem.StatusCodeSelector(exception),
					ExceptionHandled = markExceptionAsHandled
				};
			}
			return null;
		}
	}
}

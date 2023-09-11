using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Diagnostics;

namespace TracingTest
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			ExceptionTracer.Default.SubscribeToUnhandledExceptions();

			throw new ApplicationException("Test na EnchantedCoder.Diagnostics.ExceptionTracer a EnchantedCoder.Diagnostics.SmtpTraceListener.");
		}
	}
}

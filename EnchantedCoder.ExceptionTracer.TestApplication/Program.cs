using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnchantedCoder.Diagnostics;
using System.Diagnostics;
using System.Threading.Tasks;

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

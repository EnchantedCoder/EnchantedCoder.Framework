﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.TestHelpers.CastleWindsor
{
	public static class MisconfiguredComponentsHelper
	{
		public static void AssertMisconfiguredComponents(IWindsorContainer container)
		{
			var diagnostic = new PotentiallyMisconfiguredComponentsDiagnostic(container.Kernel);
			IHandler[] handlers = diagnostic.Inspect();
			if (handlers != null && handlers.Any())
			{
				var builder = new StringBuilder();
				builder.AppendFormat("Misconfigured components ({0})\r\n", handlers.Count());
				foreach (IHandler handler in handlers)
				{
					var info = (IExposeDependencyInfo)handler;
					var inspector = new DependencyInspector(builder);
					info.ObtainDependencyDetails(inspector);
				}
				Assert.Fail(builder.ToString());
			}
		}
	}
}

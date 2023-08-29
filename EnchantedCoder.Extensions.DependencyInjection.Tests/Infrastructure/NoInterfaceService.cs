﻿using EnchantedCoder.Extensions.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Extensions.DependencyInjection.Tests.Infrastructure
{
	[Service(Profile = nameof(NoInterfaceService))]
	public class NoInterfaceService
	{
	}
}

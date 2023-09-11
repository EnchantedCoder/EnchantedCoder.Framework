using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Extensions.DependencyInjection.Abstractions;

namespace EnchantedCoder.Extensions.DependencyInjection.CastleWindsor.Tests.Infrastructure
{
	[Service(Profile = nameof(NoInterfaceService))]
	public class NoInterfaceService
	{
	}
}

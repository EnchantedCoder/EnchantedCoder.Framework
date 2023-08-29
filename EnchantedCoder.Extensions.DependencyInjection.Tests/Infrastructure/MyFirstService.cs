using EnchantedCoder.Extensions.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Extensions.DependencyInjection.Tests.Infrastructure
{
	/// <summary>
	/// Implementuje dva interfaces - IFirstService a předka IService.
	/// </summary>
	[Service(Profile = nameof(MyFirstService))]
	internal class MyFirstService : IFirstService
	{
	}
}

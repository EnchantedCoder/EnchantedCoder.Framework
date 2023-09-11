using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Extensions.DependencyInjection.Abstractions;

namespace EnchantedCoder.Extensions.DependencyInjection.CastleWindsor.Tests.Infrastructure
{
	/// <summary>
	/// Implementuje jeden interface - IService.
	/// </summary>
	[Service(Profile = nameof(MyGenericService<object, object>))]
	public class MyGenericService<TA, TB> : IGenericService<TA, TB>
	{
	}
}

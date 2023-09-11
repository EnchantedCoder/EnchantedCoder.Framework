using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Scopes;

namespace EnchantedCoder.Tests.Scopes.Instrastructure
{
	internal class TestAsyncLocalScope : Scope<object>
	{
		private static readonly IScopeRepository<object> repository = new AsyncLocalScopeRepository<object>();

		public TestAsyncLocalScope(object instance) : base(instance, repository)
		{
		}

		public static object Current
		{
			get
			{
				return GetCurrent(repository);
			}
		}
	}
}

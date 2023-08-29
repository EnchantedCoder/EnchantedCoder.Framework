using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Tests
{
	internal class NoCacheModelCacheKeyFactory : IModelCacheKeyFactory
	{
		public object Create(Microsoft.EntityFrameworkCore.DbContext context, bool designTime) => context.GetHashCode();
	}
}
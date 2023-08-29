using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure
{
	public class FakeInt32DataSource : FakeDataSource<int>
	{
		public FakeInt32DataSource(params int[] data) : base(data)
		{
		}
	}
}

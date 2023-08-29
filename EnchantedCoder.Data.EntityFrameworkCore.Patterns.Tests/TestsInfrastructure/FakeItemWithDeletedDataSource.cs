using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure
{
	public class FakeItemWithDeletedDataSource : FakeDataSource<ItemWithDeleted>
	{
		public FakeItemWithDeletedDataSource(params ItemWithDeleted[] data)
			: base(data)
		{
		}
	}
}

using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure
{
	public class FakeLanguageDataSource : FakeDataSource<Language>
	{
		public FakeLanguageDataSource(params Language[] data) : base(data)
		{
		}
	}
}

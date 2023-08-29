using EnchantedCoder.EFCoreTests.Model;

namespace EnchantedCoder.EFCoreTests.DataLayer.Lookups
{
	public interface IUserLookupService
	{
		User GetUserByUsername(string username);
	}
}
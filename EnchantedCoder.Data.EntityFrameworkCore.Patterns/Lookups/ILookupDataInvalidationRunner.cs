using EnchantedCoder.Data.EntityFrameworkCore.Patterns.UnitOfWorks;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Lookups
{
	/// <summary>
	/// Zajistí  invalidaci lookup služeb všechny změněné objekty.
	/// </summary>
	public interface ILookupDataInvalidationRunner
	{
		/// <summary>
		/// Zajistí  invalidaci lookup služeb všechny změněné objekty.
		/// </summary>
		void Invalidate(Changes allKnownChanges);
	}
}
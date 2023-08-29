using System.Collections.Generic;
using System.Linq;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.Patterns.DataSources;
using EnchantedCoder.Diagnostics.Contracts;
using EnchantedCoder.Services.TimeServices;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources.Fakes
{
	/// <summary>
	/// Fake implementace <see cref="IDataSource{TSource}" /> pro použití v unit testech. Jako datový zdroj používá data předané v konstruktoru.
	/// </summary>
	public abstract class FakeDataSource<TEntity> : IDataSource<TEntity>
	{
		private readonly ISoftDeleteManager softDeleteManager;
		private readonly TEntity[] data;

		/// <summary>
		/// Data z datového zdroje jako IQueryable.
		/// </summary>
		public virtual IQueryable<TEntity> Data => new FakeAsyncEnumerable<TEntity>(data.AsQueryable().WhereNotDeleted(softDeleteManager).ToList());

		/// <summary>
		/// Data z datového zdroje jako IQueryable.
		/// </summary>
		public virtual IQueryable<TEntity> DataIncludingDeleted => new FakeAsyncEnumerable<TEntity>(data);

		/// <summary>
		/// Konstruktor.
		/// </summary>
		/// <param name="data">Data, která budou intancí vracena.</param>
		protected FakeDataSource(params TEntity[] data)
			: this(data.AsEnumerable(), null)
		{
		}

		/// <summary>
		/// Konstruktor.
		/// </summary>
		/// <param name="data">Data, která budou intancí vracena.</param>
		/// <param name="softDeleteManager">Pro podporu mazání příznakem.</param>
		protected FakeDataSource(IEnumerable<TEntity> data, ISoftDeleteManager softDeleteManager = null)
		{
			this.softDeleteManager = softDeleteManager ?? new SoftDeleteManager(new ServerTimeService());
			Contract.Requires(data != null);

			this.data = data.ToArray();
		}
	}
}

using System;
using System.Linq;
using System.Threading;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.Patterns.DataSources;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources
{
	/// <summary>
	/// Poskytuje datový zdroj objektů TEntity jako IQueryable.
	/// </summary>
	public abstract class DbDataSource<TEntity> : IDataSource<TEntity>
		where TEntity : class
	{
		private readonly ISoftDeleteManager softDeleteManager;
		private readonly Lazy<IDbSet<TEntity>> dbSetLazy;

		/// <summary>
		/// Vrací data z datového zdroje jako IQueryable.
		/// Pokud zdroj obsahuje záznamy smazané příznakem, jsou odfiltrovány (nejsou v datech).
		/// </summary>
		public virtual IQueryable<TEntity> Data => dbSetLazy.Value.AsQueryable().WhereNotDeleted(softDeleteManager);

		/// <summary>
		/// Vrací data z datového zdroje jako IQueryable.
		/// Pokud zdroj obsahuje záznamy smazané příznakem, jsou součástí dat.
		/// </summary>
		public virtual IQueryable<TEntity> DataIncludingDeleted => dbSetLazy.Value.AsQueryable();

		/// <summary>
		/// Konstruktor.
		/// </summary>
		protected DbDataSource(IDbContext dbContext, ISoftDeleteManager softDeleteManager)
		{
			this.dbSetLazy = new Lazy<IDbSet<TEntity>>(() => dbContext.Set<TEntity>(), LazyThreadSafetyMode.None);
			this.softDeleteManager = softDeleteManager;
		}
	}
}

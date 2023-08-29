using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources
{
	public class DbDataSourceFileNamingService : FileNamingServiceBase<DbDataSourceModel>
	{
		public DbDataSourceFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(DbDataSourceModel model)
		{
			return model.DbDataSourceClassName;
		}

		protected override string GetNamespaceName(DbDataSourceModel model)
		{
			return model.NamespaceName;
		}
	}
}

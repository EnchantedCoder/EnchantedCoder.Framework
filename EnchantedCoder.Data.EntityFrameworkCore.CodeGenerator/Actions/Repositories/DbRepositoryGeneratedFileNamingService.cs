using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories
{
	public class DbRepositoryGeneratedFileNamingService : FileNamingServiceBase<RepositoryModel>
	{
		public DbRepositoryGeneratedFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(RepositoryModel model)
		{
			return model.DbRepositoryName;
		}

		protected override string GetNamespaceName(RepositoryModel model)
		{
			return model.NamespaceName;
		}
	}
}

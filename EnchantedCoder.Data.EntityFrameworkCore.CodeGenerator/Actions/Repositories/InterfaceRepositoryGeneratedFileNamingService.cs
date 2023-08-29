using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories
{
	public class InterfaceRepositoryGeneratedFileNamingService : FileNamingServiceBase<RepositoryModel>
	{
		public InterfaceRepositoryGeneratedFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(RepositoryModel model)
		{
			return model.InterfaceRepositoryName;
		}

		protected override string GetNamespaceName(RepositoryModel model)
		{
			return model.NamespaceName;
		}
	}
}

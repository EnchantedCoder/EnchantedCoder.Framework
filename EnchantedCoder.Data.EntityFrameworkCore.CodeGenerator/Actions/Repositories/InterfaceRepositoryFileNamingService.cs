using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories
{
	public class InterfaceRepositoryFileNamingService : InterfaceRepositoryGeneratedFileNamingService
	{
		public InterfaceRepositoryFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override bool UseGeneratedFolder => false;
	}
}

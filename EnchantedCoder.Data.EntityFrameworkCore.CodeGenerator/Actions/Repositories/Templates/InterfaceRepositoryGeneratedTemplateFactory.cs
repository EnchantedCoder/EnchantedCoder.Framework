using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Templates
{
	public class InterfaceRepositoryGeneratedTemplateFactory : ITemplateFactory<RepositoryModel>
	{
		public ITemplate CreateTemplate(RepositoryModel model)
		{
			return new InterfaceRepositoryGeneratedTemplate(model);
		}
	}

}

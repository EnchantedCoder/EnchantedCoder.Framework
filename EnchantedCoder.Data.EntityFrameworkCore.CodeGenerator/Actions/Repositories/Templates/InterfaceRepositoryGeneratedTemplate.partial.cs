using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Templates
{
	public partial class InterfaceRepositoryGeneratedTemplate : ITemplate
	{
		protected RepositoryModel Model { get; private set; }

		public InterfaceRepositoryGeneratedTemplate(RepositoryModel model)
		{
			this.Model = model;
		}
	}
}

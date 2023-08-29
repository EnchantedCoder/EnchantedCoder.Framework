using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Templates
{
	public partial class InterfaceRepositoryTemplate : ITemplate
	{
		protected RepositoryModel Model { get; private set; }

		public InterfaceRepositoryTemplate(RepositoryModel model)
		{
			this.Model = model;
		}
	}
}

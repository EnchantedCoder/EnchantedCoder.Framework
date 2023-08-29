using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public class InterfaceDataSourceTemplateFactory : ITemplateFactory<InterfaceDataSourceModel>
	{
		public ITemplate CreateTemplate(InterfaceDataSourceModel model)
		{
			return new InterfaceDataSourceTemplate(model);
		}
	}

}

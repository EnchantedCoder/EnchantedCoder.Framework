using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public class FakeDataSourceTemplateFactory : ITemplateFactory<FakeDataSourceModel>
	{
		public ITemplate CreateTemplate(FakeDataSourceModel model)
		{
			return new FakeDataSourceTemplate(model);
		}
	}

}

using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public class DbDataSourceTemplateFactory : ITemplateFactory<DbDataSourceModel>
	{
		public ITemplate CreateTemplate(DbDataSourceModel model)
		{
			return new DbDataSourceTemplate(model);
		}
	}

}

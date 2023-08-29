using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public partial class DbDataSourceTemplate : ITemplate
	{
		protected DbDataSourceModel Model { get; private set; }

		public DbDataSourceTemplate(DbDataSourceModel model)
		{
			this.Model = model;
		}
	}
}

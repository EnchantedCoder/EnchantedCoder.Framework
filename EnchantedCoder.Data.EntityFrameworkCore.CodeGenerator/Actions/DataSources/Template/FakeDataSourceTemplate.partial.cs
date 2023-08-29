using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public partial class FakeDataSourceTemplate : ITemplate
	{
		protected FakeDataSourceModel Model { get; private set; }

		public FakeDataSourceTemplate(FakeDataSourceModel model)
		{
			this.Model = model;
		}
	}
}

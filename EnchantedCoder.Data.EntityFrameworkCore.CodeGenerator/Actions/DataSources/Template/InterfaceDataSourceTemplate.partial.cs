using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataSources.Template
{
	public partial class InterfaceDataSourceTemplate : ITemplate
	{
		protected InterfaceDataSourceModel Model { get; private set; }

		public InterfaceDataSourceTemplate(InterfaceDataSourceModel model)
		{
			this.Model = model;
		}
	}
}

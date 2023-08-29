using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Template
{
	public partial class DbDataEntriesTemplate : ITemplate
	{
		protected DataEntriesModel Model { get; private set; }

		public DbDataEntriesTemplate(DataEntriesModel model)
		{
			this.Model = model;
		}
	}
}

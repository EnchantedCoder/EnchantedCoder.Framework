using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Template
{
	public partial class InterfaceDataEntriesTemplate : ITemplate
	{
		protected DataEntriesModel Model { get; private set; }

		public InterfaceDataEntriesTemplate(DataEntriesModel model)
		{
			this.Model = model;
		}
	}
}

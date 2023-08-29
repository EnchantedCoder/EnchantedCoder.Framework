using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Template
{
	public class InterfaceDataEntriesTemplateFactory : ITemplateFactory<DataEntriesModel>
	{
		public ITemplate CreateTemplate(DataEntriesModel model)
		{
			return new InterfaceDataEntriesTemplate(model);
		}
	}

}

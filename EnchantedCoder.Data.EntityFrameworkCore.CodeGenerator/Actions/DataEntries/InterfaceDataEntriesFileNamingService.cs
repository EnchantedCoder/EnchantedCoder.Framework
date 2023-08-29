using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries
{
	public class InterfaceDataEntriesFileNamingService : FileNamingServiceBase<DataEntriesModel>
	{
		public InterfaceDataEntriesFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(DataEntriesModel model)
		{
			return model.InterfaceName;
		}

		protected override string GetNamespaceName(DataEntriesModel model)
		{
			return model.NamespaceName;
		}
	}
}

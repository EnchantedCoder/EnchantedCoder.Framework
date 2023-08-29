using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.DataEntries
{
	public class DbDataEntriesFileNamingService : FileNamingServiceBase<DataEntriesModel>
	{
		public DbDataEntriesFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(DataEntriesModel model)
		{
			return model.DbClassName;
		}

		protected override string GetNamespaceName(DataEntriesModel model)
		{
			return model.NamespaceName;
		}
	}
}

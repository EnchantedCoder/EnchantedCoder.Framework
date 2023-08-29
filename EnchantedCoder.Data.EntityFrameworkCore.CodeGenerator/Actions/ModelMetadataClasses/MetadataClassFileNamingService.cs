using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses
{
	public class MetadataClassFileNamingService : FileNamingServiceBase<MetadataClass>
	{
		public MetadataClassFileNamingService(IProject project)
			: base(project)
		{

		}

		protected override string GetClassName(MetadataClass model)
		{
			return model.ClassName;
		}

		protected override string GetNamespaceName(MetadataClass model)
		{
			return model.NamespaceName;
		}
	}
}

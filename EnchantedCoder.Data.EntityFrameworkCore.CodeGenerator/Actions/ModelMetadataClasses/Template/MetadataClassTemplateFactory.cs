using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses.Template
{
	public class MetadataClassTemplateFactory : ITemplateFactory<MetadataClass>
	{
		public ITemplate CreateTemplate(MetadataClass model)
		{
			return new MetadataClassTemplate(model);
		}
	}

}

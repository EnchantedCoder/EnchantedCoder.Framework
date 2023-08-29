using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.ModelMetadataClasses.Template
{
	public partial class MetadataClassTemplate : ITemplate
	{
		protected MetadataClass Model { get; private set; }

		public MetadataClassTemplate(MetadataClass model)
		{
			this.Model = model;
		}
	}
}

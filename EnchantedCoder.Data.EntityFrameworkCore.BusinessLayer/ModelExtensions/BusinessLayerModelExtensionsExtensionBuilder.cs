using EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.ModelExtensions.ExtendedProperties;
using EnchantedCoder.Data.EntityFrameworkCore.Migrations.ModelExtensions;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.ModelExtensions
{
	public class BusinessLayerModelExtensionsExtensionBuilder : ModelExtensionsExtensionBuilderBase
	{
		public BusinessLayerModelExtensionsExtensionBuilder(ModelExtensionsExtensionBuilder builder)
			: base(builder)
		{
		}

		public ModelExtensionsExtensionBuilder UseExtendedProperties() =>
			WithOption(e => e.WithAnnotationProvider<ExtendedPropertiesAnnotationProvider>());

		public ModelExtensionsExtensionBuilder UseBusinessLayerStoredProcedures() =>
			WithOption(e => e
				.WithAnnotationProvider<StoredProcedureAttachPropertyAnnotationProvider>()
				.WithAnnotationProvider<StoredProcedureMsDescriptionPropertyAnnotationProvider>());
	}
}
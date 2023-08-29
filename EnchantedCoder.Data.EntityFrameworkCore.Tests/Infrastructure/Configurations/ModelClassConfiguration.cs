using EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Configurations
{
	internal class ModelClassConfiguration : IEntityTypeConfiguration<ModelClass>
	{
		public void Configure(EntityTypeBuilder<ModelClass> builder)
		{
			// NOOP
		}
	}
}

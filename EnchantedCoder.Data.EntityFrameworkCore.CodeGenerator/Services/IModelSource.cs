using System.Collections.Generic;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services
{
	public interface IModelSource<TModel>
	{
		IEnumerable<TModel> GetModels();
	}
}

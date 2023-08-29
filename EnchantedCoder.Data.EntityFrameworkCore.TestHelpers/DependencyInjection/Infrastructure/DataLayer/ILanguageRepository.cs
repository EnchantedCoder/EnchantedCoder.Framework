using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.Model;
using EnchantedCoder.Data.Patterns.Repositories;

namespace EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.DataLayer
{
	public interface ILanguageRepository : IRepository<Language>
	{
	}
}

﻿using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Templates
{
	public class InterfaceRepositoryTemplateFactory : ITemplateFactory<RepositoryModel>
	{
		public ITemplate CreateTemplate(RepositoryModel model)
		{
			return new InterfaceRepositoryTemplate(model);
		}
	}

}

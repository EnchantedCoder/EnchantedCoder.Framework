﻿using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Model;
using EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Services;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Actions.Repositories.Templates
{
	public partial class DbRepositoryTemplate : ITemplate
	{
		protected RepositoryModel Model { get; private set; }

		public DbRepositoryTemplate(RepositoryModel model)
		{
			this.Model = model;
		}
	}
}

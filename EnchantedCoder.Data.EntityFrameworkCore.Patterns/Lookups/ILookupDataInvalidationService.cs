﻿using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.UnitOfWorks;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Lookups
{
	/// <summary>
	/// Interface pro lookup služby.
	/// Nemá význam pro běžný vývoj, slouží zejména UnitOfWork k možnosti provedení invalidace.
	/// </summary>
	public interface ILookupDataInvalidationService
	{
		/// <summary>
		/// Provede invalidaci lookup dat bez ohledu na provedené změny.
		/// </summary>
		void Invalidate();

		/// <summary>
		/// Přijme oznámení o změněných entitách aktualizuje data.
		/// </summary>
		void InvalidateAfterCommit(Changes changes);
	}
}

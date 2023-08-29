using EnchantedCoder.Model.Collections.Generic;
using System;
using System.Collections.Generic;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader.Model
{
	public class Master
	{
		public int Id { get; set; }

		public FilteringCollection<Child> Children { get; set; }

		public List<Child> ChildrenIncludingDeleted { get; } = new List<Child>();

		public DateTime? Deleted { get; set; }

		public Master()
		{
			Children = new FilteringCollection<Child>(ChildrenIncludingDeleted, child => child.Deleted == null);
		}
	}
}

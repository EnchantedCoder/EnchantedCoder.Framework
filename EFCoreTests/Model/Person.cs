using System;
using System.Collections.Generic;
using System.Text;

namespace EnchantedCoder.EFCoreTests.Model
{
	public class Person
	{
		public int Id { get; set; }

		public string Name { get; set; } = Guid.NewGuid().ToString();

		public Person Boss { get; set; }
		public int? BossId { get; set; }

		public List<Person> Subordinates { get; } = new List<Person>();
	}
}

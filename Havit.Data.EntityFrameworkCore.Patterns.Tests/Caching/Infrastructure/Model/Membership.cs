﻿namespace Havit.Data.EntityFrameworkCore.Patterns.Tests.Caching.Infrastructure.Model;

public class Membership
{
	public LoginAccount LoginAccount { get; set; }
	public int LoginAccountId { get; set; }

	public Role Role { get; set; }
	public int RoleId { get; set; }

}
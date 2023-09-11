using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EnchantedCoder.Data.SqlClient.Azure.SqlServerAadAuthentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Data.SqlClient.Azure.Tests.SqlServerAadAuthentication
{
	[TestClass]
	public class AadAuthenticationSupportDecisionTests
	{
		[TestMethod]
		public void AadAuthenticationSupportDecision_ShouldUseAadAuthentication()
		{
			// Act + Assert

			// no database.windows.net in Data Source
			Assert.IsFalse(AadAuthenticationSupportDecision.ShouldUseAadAuthentication("Data Source=fake;Initial Catalog=fake;User Id=fake;Password=fake"));

			// no database.windows.net in Data Source
			Assert.IsFalse(AadAuthenticationSupportDecision.ShouldUseAadAuthentication("Data Source=fake;Initial Catalog=fake"));

			// no database.windows.net in Data Source
			Assert.IsFalse(AadAuthenticationSupportDecision.ShouldUseAadAuthentication("Data Source=fake;Initial Catalog=fake;Application Name=fake.database.windows.net"));

			// database.windows.net but User Id specified
			Assert.IsFalse(AadAuthenticationSupportDecision.ShouldUseAadAuthentication("Data Source=fake.database.windows.net;Initial Catalog=fake;User Id=fake;Password=fake"));

			// database.windows.net in Data Source and no User Id
			Assert.IsTrue(AadAuthenticationSupportDecision.ShouldUseAadAuthentication("Data Source=fake.database.windows.net;Initial Catalog=fake"));
		}
	}
}

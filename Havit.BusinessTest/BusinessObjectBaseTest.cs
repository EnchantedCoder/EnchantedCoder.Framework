﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Havit.BusinessLayerTest;

namespace Havit.BusinessTest
{
	/// <summary>
	///This is a test class for Havit.Business.BusinessObjectBase and is intended
	///to contain all Havit.Business.BusinessObjectBase Unit Tests
	///</summary>
	[TestClass()]
	public class BusinessObjectBaseTest
	{
		#region EqualsTest_StejneID
		/// <summary>
		/// Základní test na funkčnost Equals při stejných ID.
		///</summary>
		[TestMethod()]
		public void EqualsTest_StejneID()
		{
			Role role1 = Role.GetObject(-1);
			Role role2 = Role.GetObject(-1);

			Assert.IsTrue(role1.Equals(role2));
			Assert.IsTrue(role2.Equals(role1));
			Assert.IsTrue(role1 == role2);
			Assert.IsTrue(role2 == role1);
		}
		#endregion

		#region EqualsTest_RuzneID
		/// <summary>
		/// Základní test na funkčnost Equals při různých ID.
		///</summary>
		[TestMethod()]
		public void EqualsTest_RuzneID()
		{
			Role role1 = Role.GetObject(-1);
			Role role2 = Role.GetObject(1);

			Assert.IsFalse(role1.Equals(role2));
			Assert.IsFalse(role2.Equals(role1));
			Assert.IsFalse(role1 == role2);
			Assert.IsFalse(role2 == role1);
		}
		#endregion

		#region EqualsTest_Nove
		/// <summary>
		/// Základní test na funkčnost Equals na nové objekty.
		///</summary>
		[TestMethod()]
		public void EqualsTest_Nove()
		{
			Subjekt subjekt1 = Subjekt.CreateObject();
			Subjekt subjekt2 = Subjekt.CreateObject();
			Subjekt subjekt3 = subjekt1;

			Assert.IsFalse(subjekt1.Equals(subjekt2));
			Assert.IsFalse(subjekt1.Equals(subjekt2));
			Assert.IsFalse(subjekt1 == subjekt2);
			Assert.IsFalse(subjekt1 == subjekt2);

			Assert.IsTrue(subjekt1.Equals(subjekt3));
			Assert.IsTrue(subjekt1.Equals(subjekt3));
			Assert.IsTrue(subjekt1 == subjekt3);
			Assert.IsTrue(subjekt1 == subjekt3);
		}
		#endregion


		#region TestContext
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}
		#endregion

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		/// <summary>
		/// Testuje, zda cachovaný GetAll vrací klon kolekce.
		/// </summary>
		[TestMethod]
		public void GetAllCacheClone()
		{
			RoleCollection roleCollection1 = Role.GetAll();
			RoleCollection roleCollection2 = Role.GetAll();
			Assert.IsTrue(roleCollection1 != roleCollection2);
		}
	}


}

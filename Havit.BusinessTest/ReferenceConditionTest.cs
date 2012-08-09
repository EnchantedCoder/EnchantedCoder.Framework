﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Havit.Business.Query;
using Havit.BusinessLayerTest;
namespace Havit.BusinessTest
{
	/// <summary>
	///This is a test class for Havit.Business.Query.ReferenceCondition and is intended
	///to contain all Havit.Business.Query.ReferenceCondition Unit Tests
	///</summary>
	[TestClass()]
	public class ReferenceConditionTest
	{
		#region CreateEquals
		/// <summary>
		///A test for CreateEquals (IOperand, int?)
		///</summary>
		[TestMethod()]
		public void CreateEqualsTest_ZaporneID_Neexistujici()
		{
			IOperand operand = Subjekt.Properties.Uzivatel;
			System.Nullable<int> id = -10145603;  // neexistující uživatel

			QueryParams qp = new QueryParams();
			qp.Conditions.Add(ReferenceCondition.CreateEquals(operand, id));
			SubjektCollection subjekty = Subjekt.GetList(qp); 

			Assert.AreEqual(subjekty.Count, 0);
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

	}
}

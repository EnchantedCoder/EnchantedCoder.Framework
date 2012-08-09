﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Havit.Web;
namespace Havit.WebTest
{
	/// <summary>
	///This is a test class for Havit.Web.QueryStringBuilder and is intended
	///to contain all Havit.Web.QueryStringBuilder Unit Tests
	///</summary>
	[TestClass()]
	public class QueryStringBuilderTest
	{
		#region Add_Null
		/// <summary>
		///A test for Add (string, string)
		///</summary>
		[TestMethod(), ExpectedException(typeof(ArgumentException))]
		public void AddTest_Null()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add(null, "cokoliv");
		}
		#endregion

		#region SetTest_Null
		/// <summary>
		///A test for Set (string, string)
		///</summary>
		[TestMethod(), ExpectedException(typeof(ArgumentException))]
		public void SetTest()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Set(null, "cokoliv");
		}
		#endregion

		#region ToStringTest_Simple
		/// <summary>
		///A test for ToString ()
		///</summary>
		[TestMethod()]
		public void ToStringTest_Simple()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key1", "value1");
			target.Add("key2", "value2");

			string expected = "key1=value1&key2=value2";
			string actual;

			actual = target.ToString();

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region ToStringTest_EncodeSpace
		/// <summary>
		///A test for ToString ()
		///</summary>
		[TestMethod()]
		public void ToStringTest_EncodeSpace()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "value with space");

			string expected = "key=value+with+space";
			string actual;

			actual = target.ToString();

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region ToStringTest_EncodeText
		/// <summary>
		///A test for ToString ()
		///</summary>
		[TestMethod()]
		public void ToStringTest_EncodeText()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "ěščřžýáíéúů");

			string expected = "key=%C4%9B%C5%A1%C4%8D%C5%99%C5%BE%C3%BD%C3%A1%C3%AD%C3%A9%C3%BA%C5%AF".ToLower();
			string actual;

			actual = target.ToString();

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region ToStringTest_EncodeAmpersand
		/// <summary>
		///A test for ToString ()
		///</summary>
		[TestMethod()]
		public void ToStringTest_EncodeAmpersand()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "text1&text2");

			string expected = "key=text1%26text2";
			string actual;

			actual = target.ToString();

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region GetUrlWithQueryStringTest_Simple
		/// <summary>
		///A test for GetUrlWithQueryString (string)
		///</summary>
		[TestMethod()]
		public void GetUrlWithQueryStringTest_Simple()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "value");

			string url = "foo.aspx";

			string expected = "foo.aspx?key=value";
			string actual;

			actual = target.GetUrlWithQueryString(url);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region GetUrlWithQueryStringTest_ExistingQueryString
		/// <summary>
		///A test for GetUrlWithQueryString (string)
		///</summary>
		[TestMethod()]
		public void GetUrlWithQueryStringTest_ExistingQueryStringSimple()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "value");

			string url = "foo.aspx?key1=value1";

			string expected = "foo.aspx?key1=value1&key=value";
			string actual;

			actual = target.GetUrlWithQueryString(url);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region GetUrlWithQueryStringTest_ExistingQueryStringWithAmpersand
		/// <summary>
		///A test for GetUrlWithQueryString (string)
		///</summary>
		[TestMethod()]
		public void GetUrlWithQueryStringTest_ExistingQueryStringWithAmpersand()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "value");

			string url = "foo.aspx?key1=value1&";

			string expected = "foo.aspx?key1=value1&key=value";
			string actual;

			actual = target.GetUrlWithQueryString(url);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region GetUrlWithQueryStringTest_UrlWithQuestionMark
		/// <summary>
		///A test for GetUrlWithQueryString (string)
		///</summary>
		[TestMethod()]
		public void GetUrlWithQueryStringTest_UrlWithQuestionMark()
		{
			QueryStringBuilder target = new QueryStringBuilder();
			target.Add("key", "value");

			string url = "foo.aspx?";

			string expected = "foo.aspx?key=value";
			string actual;

			actual = target.GetUrlWithQueryString(url);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region FillFromStringTest_Simple
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_Simple()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = "key1=value1&key2=value2";

			target.FillFromString(queryString);

			Assert.IsTrue(target["key1"] == "value1");
			Assert.IsTrue(target["key2"] == "value2");
			Assert.IsTrue(target.Count == 2);
			Assert.IsTrue(target.ToString() == queryString);
		}
		#endregion

		#region FillFromStringTest_UrlEncoded
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_UrlEncoded()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = "key=%C4%9B%C5%A1%C4%8D%C5%99%C5%BE%C3%BD%C3%A1%C3%AD%C3%A9%C3%BA%C5%AF";

			target.FillFromString(queryString);

			Assert.IsTrue(target["key"] == "ěščřžýáíéúů");
		}
		#endregion

		#region FillFromStringTest_Ampersands
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_Ampersands()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = "&key1=value1&&key2=value2&";

			target.FillFromString(queryString);

			Assert.IsTrue(target["key1"] == "value1");
			Assert.IsTrue(target["key2"] == "value2");
			Assert.IsTrue(target.Count == 2);
			Assert.IsTrue(target.ToString() == "key1=value1&key2=value2");
		}
		#endregion

		#region FillFromStringTest_EmptyValue
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_EmptyValue()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = "key1=";

			target.FillFromString(queryString);

			Assert.IsTrue(target["key1"] == String.Empty);
			Assert.IsTrue(target.Count == 1);
			Assert.IsTrue(target.ToString() == "key1=");
		}
		#endregion

		#region FillFromStringTest_EmptyInput
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_EmptyInput()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = "";

			target.FillFromString(queryString);

			Assert.IsTrue(target.Count == 0);
			Assert.IsTrue(target.ToString() == String.Empty);
		}
		#endregion

		#region FillFromStringTest_NullInput
		/// <summary>
		///A test for FillFromString (string)
		///</summary>
		[TestMethod()]
		public void FillFromStringTest_NullInput()
		{
			QueryStringBuilder target = new QueryStringBuilder();

			string queryString = null;

			target.FillFromString(queryString);

			Assert.IsTrue(target.Count == 0);
			Assert.IsTrue(target.ToString() == String.Empty);
		}
		#endregion

		/*****************************************************************/

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

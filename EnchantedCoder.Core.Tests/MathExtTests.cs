using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Tests
{
	[TestClass]
	public class MathExtTests
	{
		[TestMethod]
		public void MathExt_IsInteger_ReturnsFalseForNull()
		{
			// arange
			string text = null;
			bool expected = false;

			// act
			var actual = EnchantedCoder.MathExt.IsInteger(text);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MathExt_IsInteger_ReturnsTrueForPositiveInteger()
		{
			// arrange
			string text = "156";
			bool expected = true;

			// act
			var actual = EnchantedCoder.MathExt.IsInteger(text);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MathExt_IsInteger_ReturnsTrueForNegativeInteger()
		{
			// arrange
			string text = "-156";
			bool expected = true;

			// act
			var actual = EnchantedCoder.MathExt.IsInteger(text);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MathExt_IsIntegerTest_ReturnsFalseForEmptyString()
		{
			// arrange
			string text = String.Empty;
			bool expected = false;

			// act
			var actual = EnchantedCoder.MathExt.IsInteger(text);

			// assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MathExt_IsIntegerTest_ReturnsFalseForInvalidValue()
		{
			// arrange
			string text = "10.";
			bool expected = false;

			// act
			var actual = EnchantedCoder.MathExt.IsInteger(text);

			// assert
			Assert.AreEqual(expected, actual);
		}
	}
}

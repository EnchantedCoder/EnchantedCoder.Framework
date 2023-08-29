using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EnchantedCoder.Tests.ComponentModel
{
	[TestClass]
	public class UniversalConverterTests
	{
		[TestMethod]
		public void UniversalConverter_TryConvertTo()
		{
			object result;
			bool success;

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("A", typeof(int), out result);
			Assert.IsFalse(success);
			Assert.IsNull(result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("5", typeof(int), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("5", typeof(int?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("5", typeof(decimal), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("5", typeof(decimal?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5, typeof(int), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5, typeof(int?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5, typeof(decimal), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5, typeof(decimal?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5M, typeof(int), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5M, typeof(int?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5, (int?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5M, typeof(decimal), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(5M, typeof(decimal?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(5M, (decimal?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(null, typeof(int?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(null, (int?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo(null, typeof(decimal?), out result);
			Assert.IsTrue(success);
			Assert.AreEqual(null, (decimal?)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("1/2/2013", typeof(DateTime), out result, CultureInfo.GetCultureInfo("en-US"));
			Assert.IsTrue(success);
			Assert.AreEqual(new DateTime(2013, 1, 2), (DateTime)result);

			success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo("1/2/2013", typeof(DateTime), out result, CultureInfo.GetCultureInfo("en-GB"));
			Assert.IsTrue(success);
			Assert.AreEqual(new DateTime(2013, 2, 1), (DateTime)result);
		}

        [TestMethod]
        public void UniversalConverter_TryConvertTo_Generic()
        {
            bool success;

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int>("A", out int result1);
            Assert.IsFalse(success);
            Assert.AreEqual(result1, default(int));

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int>("5", out int result2);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result2);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int?>("5", out int? result3);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result3);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal>("5", out decimal result4);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result4);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal?>("5", out decimal? result5);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result5);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int>(5, out int result6);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result6);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int?>(5, out int? result7);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result7);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal>(5, out decimal result8);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result8);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal?>(5, out decimal? result9);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result9);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int>(5M, out int result10);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result10);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int?>(5M, out int? result11);
            Assert.IsTrue(success);
            Assert.AreEqual(5, result11);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal>(5M, out decimal result12);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result12);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal?>(5M, out decimal? result13);
            Assert.IsTrue(success);
            Assert.AreEqual(5M, result13);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<int?>(null, out int? result14);
            Assert.IsTrue(success);
            Assert.AreEqual(null, result14);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<decimal?>(null, out decimal? result15);
            Assert.IsTrue(success);
            Assert.AreEqual(null, result15);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<DateTime>("1/2/2013", out DateTime result16, CultureInfo.GetCultureInfo("en-US"));
            Assert.IsTrue(success);
            Assert.AreEqual(new DateTime(2013, 1, 2), result16);

            success = EnchantedCoder.ComponentModel.UniversalTypeConverter.TryConvertTo<DateTime>("1/2/2013", out DateTime result17, CultureInfo.GetCultureInfo("en-GB"));
            Assert.IsTrue(success);
            Assert.AreEqual(new DateTime(2013, 2, 1), result17);
        }
    }
}
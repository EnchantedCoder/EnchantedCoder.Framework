using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Tests.Reflection
{
	[TestClass]
	public class ReflectorTests
	{
		[TestMethod]
		public void Reflector_GetPropertyValue()
		{
			ReflectorTestClass data = new ReflectorTestClass();
			object value = new object();
			data.Value = value;
			object valueReflection = EnchantedCoder.Reflection.Reflector.GetPropertyValue(data, "Value");
			Assert.AreEqual(value, valueReflection);
		}

		[TestMethod]
		public void Reflector_SetPropertyValue()
		{
			ReflectorTestClass data = new ReflectorTestClass();
			object value = new object();
			EnchantedCoder.Reflection.Reflector.SetPropertyValue(data, "Value", value);
			Assert.AreEqual(value, data.Value);
		}

		private class ReflectorTestClass
		{
			public object Value { get; set; }
		}
	}
}

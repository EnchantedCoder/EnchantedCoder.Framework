using System;
using System.Reflection;

namespace Havit.Reflection
{
	/// <summary>
	/// T��da se statick�mi metodami pro jednoduch� operace reflexe.
	/// </summary>
	public sealed class Reflector
	{
		#region GetPropertyValue
		/// <summary>
		/// Z�sk� hodnotu property, i kdyby byla ozna�en� jako protected, internal, nebo private.
		/// </summary>
		/// <param name="target">Objekt, z kter�ho m� b�t property z�sk�na.</param>
		/// <param name="targetType">Typ z kter�ho m� b�t property z�sk�na (m��e b�t i rodi�ovsk�m typem targetu).</param>
		/// <param name="propertyName">Jm�no property.</param>
		/// <returns>Hodnota property, nebo null, nen�-li nalezena.</returns>
		public static object GetPropertyValue(Object target, Type targetType, String propertyName) 
		{
			PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic );
			if (property != null) 
			{
				return property.GetValue(target, null);
			} 
			else 
			{
				return null;
			}
		}
		#endregion

		#region private constructor
		/// <summary>
		/// private constructor, aby nebylo mo�no vytvo�it instanci t��dy
		/// </summary>
		private Reflector()	{}
		#endregion
	}
}

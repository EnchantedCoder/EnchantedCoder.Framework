using System;
using System.Reflection;

namespace Havit.Reflection
{
	/// <summary>
	/// T��da se statick�mi metodami pro jednoduch� operace reflexe.
	/// </summary>
	public static class Reflector
	{
		#region GetPropertyValue
		/// <summary>
		/// Z�sk� hodnotu property, i kdyby byla ozna�en� jako protected, internal, nebo private.
		/// Vlastnost je hled�na jen na zadan�m typu (targetType).
		/// </summary>
		/// <param name="target">Objekt, z kter�ho m� b�t property z�sk�na.</param>
		/// <param name="targetType">Typ z kter�ho m� b�t property z�sk�na (m��e b�t i rodi�ovsk�m typem targetu).</param>
		/// <param name="propertyName">Jm�no property.</param>
		/// <returns>Hodnota property, nebo null, nen�-li nalezena.</returns>
		public static object GetPropertyValue(Object target, Type targetType, String propertyName) 
		{
			return GetPropertyValue(
				target,
				targetType,
				propertyName,
				BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
			);
		}

		/// <summary>
		/// Z�sk� hodnotu property, i kdyby byla ozna�en� jako protected, internal, nebo private.
		/// </summary>
		/// <param name="target">Objekt, z kter�ho m� b�t property z�sk�na.</param>
		/// <param name="propertyName">Jm�no property.</param>
		/// <returns>Hodnota property, nebo null, nen�-li nalezena.</returns>
		public static object GetPropertyValue(Object target, String propertyName)
		{
			return GetPropertyValue(
				target,
				target.GetType(),
				propertyName,
				BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy
			);
		}

		private static object GetPropertyValue(Object target, Type targetType, String propertyName, BindingFlags bindingFlags)
		{
			PropertyInfo property = targetType.GetProperty(propertyName, bindingFlags);
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

		#region SetPropertyValue
		/// <summary>
		/// Nastav� hodnotu property, i kdyby byla ozna�en� jako protected, internal, nebo private.
		/// Pokud se nepoda�� vlastnost nal�zt, vyvol� v�jimku InvalidOperationException.
		/// </summary>
		/// <param name="target">Objekt, z kter�ho m� b�t property z�sk�na.</param>
		/// <param name="targetType">Typ z kter�ho m� b�t property z�sk�na (m��e b�t i rodi�ovsk�m typem targetu).</param>
		/// <param name="propertyName">Jm�no property.</param>
		public static void SetPropertyValue(Object target, Type targetType, String propertyName, object value)
		{
			SetPropertyValue(
				target,
				targetType,
				propertyName,
				BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
				value
			);
		}

		/// <summary>
		/// Nastav� hodnotu property, i kdyby byla ozna�en� jako protected, internal, nebo private.
		/// Vlastnost je hled�na jen na zadan�m typu (targetType).
		/// Pokud se nepoda�� vlastnost nal�zt, vyvol� v�jimku InvalidOperationException.
		/// </summary>
		/// <param name="target">Objekt, z kter�ho m� b�t property z�sk�na.</param>
		/// <param name="propertyName">Jm�no property.</param>
		public static void SetPropertyValue(Object target, String propertyName, object value)
		{
			SetPropertyValue(
				target,
				target.GetType(),
				propertyName, 
				BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
				value
			);
		}

		private static void SetPropertyValue(Object target, Type targetType, String propertyName, BindingFlags bindingFlags, object value)
		{
			PropertyInfo property = targetType.GetProperty(propertyName, bindingFlags);
			if (property == null)
			{
				throw new InvalidOperationException(String.Format("Vlastnost {0} nebyla v t��d� {1} nalezena.", propertyName, targetType.FullName));
			}
			property.SetValue(target, value, null);
		}
		
		#endregion
	}
}

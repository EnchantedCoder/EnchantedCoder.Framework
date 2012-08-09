using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Havit.Business
{
	/// <summary>
	/// Identity map pro business-objekty.
	/// </summary>
	public class IdentityMap
	{
		#region private fields
		/// <summary>
		/// Hashtable obsahuj�c� hashtable pro ka�d� typ.
		/// </summary>
		private Hashtable types;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci t��dy <see cref="IdentityMap"/>.
		/// </summary>
		public IdentityMap()
		{
			types = new Hashtable();
		}
		#endregion

		#region Store<T>
		/// <summary>
		/// Ulo�� business-objekt do identity-map.
		/// </summary>
		/// <typeparam name="T">typ business objektu</typeparam>
		/// <param name="businessObject">business objekt</param>
		public void Store<T>(T businessObject)
			where T : BusinessObjectBase
		{
			if (businessObject == null)
			{
				throw new ArgumentNullException("businessObject");
			}

			if (businessObject.IsNew)
			{
				throw new ArgumentException("businessObject ukl�dan� do IdentityMap nesm� b�t nov�.", "businessObject");
			}

			Hashtable typeHashtable = types[typeof(T)] as Hashtable;
			if (typeHashtable == null)
			{
				typeHashtable = new Hashtable();
				types.Add(typeof(T), typeHashtable);
			}

			if (typeHashtable.ContainsKey(businessObject.ID))
			{
#warning Co se m� st�t p�i ukl�d�n� existuj�c�ho objektu do IdentityMap?
				throw new InvalidOperationException("IdentityMap ji� tento objekt obsahuje.");
			}

			typeHashtable.Add(businessObject.ID, businessObject);
		}
		#endregion

		#region Get<T>
		/// <summary>
		/// Vr�t� business-objekt z identity-map.
		/// </summary>
		/// <typeparam name="T">typ business objektu</typeparam>
		/// <param name="id">ID business objektu</param>
		/// <returns>business-objekt z identity-map; <c>null</c>, pokud v n� nen�</returns>
		public T Get<T>(int id)
			where T : BusinessObjectBase
		{
			Hashtable typeHashtable = types[typeof(T)] as Hashtable;
			if (typeHashtable == null)
			{
				return null;
			}
			return (T)typeHashtable[id];
		}
		#endregion
	}
}

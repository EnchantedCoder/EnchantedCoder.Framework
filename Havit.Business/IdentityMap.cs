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

			WeakReference temp = (WeakReference)typeHashtable[businessObject.ID];
			if (temp != null)
			{
				if (temp.Target != null)
				{
					if (!Object.ReferenceEquals(temp.Target, businessObject))
					{
						throw new InvalidOperationException("V IdentityMap je ji� jin� instance tohoto objektu.");
					}
				}
				else
				{
					temp.Target = businessObject;
				}
			}
			else
			{
				typeHashtable.Add(businessObject.ID, new WeakReference(businessObject));
			}
		}
		#endregion

		#region TryGet<T>
		/// <summary>
		/// Na�te business-objekt z identity-map.
		/// </summary>
		/// <typeparam name="T">typ business objektu</typeparam>
		/// <param name="id">ID business objektu</param>
		/// <param name="target">c�l, kam m� b�t business-objekt na�ten</param>
		/// <returns><c>true</c>, pokud se poda�ilo na��st; <c>false</c>, pokud objekt v identity-map nen� (target pak obsahuje <c>null</c>)</returns>
		public bool TryGet<T>(int id, out T target)
			where T : BusinessObjectBase
		{
			target = null;
			Hashtable typeHashtable = types[typeof(T)] as Hashtable;
			if (typeHashtable == null)
			{
				return false;
			}
			WeakReference reference = (WeakReference)typeHashtable[id];
			if (reference != null)
			{
				target = (T)reference.Target;
			}
			
			return !(target == null);
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
			WeakReference reference = (WeakReference)typeHashtable[id];
			if (reference == null)
			{
				return null;
			}
			return (T)reference.Target;
		}
		#endregion
	}
}

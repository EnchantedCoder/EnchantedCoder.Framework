using System;
using System.Collections.Generic;
using System.Text;
using Havit.Business;
using System.Data;

namespace Havit.Business
{
	/// <summary>
	/// Reprezentuje sloupec v datab�zi,
	/// kter� je referenc� na jin� typ (je ciz�m kl��em do jin� tabulky).
	/// </summary>
	[Serializable]
	public class ReferenceFieldPropertyInfo : FieldPropertyInfo
	{
		#region Initialize
		/// <summary>
		/// Inicializuje instanci sloupce.
		/// </summary>
		/// <param name="owner">Nad�azen� objectInfo.</param>
		/// <param name="propertyName">N�zev property.</param>
		/// <param name="fieldName">N�zev sloupce v datab�zy.</param>
		/// <param name="isPrimaryKey">Indikuje, zda je sloupec prim�rn�m kl��em</param>
		/// <param name="nullable">Indukuje, zda je povolena hodnota null.</param>
		/// <param name="fieldType">Typ datab�zov�ho sloupce.</param>
		/// <param name="maximumLength">Maxim�ln� d�lka dat datab�zov�ho sloupce.</param>		
		/// <param name="targetType">Typ, jen� property nese.</param>
		/// <param name="targetObjectInfo">ObjectInfo na typ, jen� property nese.</param>
		public void Initialize(ObjectInfo owner, string propertyName, string fieldName, bool isPrimaryKey, SqlDbType fieldType, bool nullable, int maximumLength, Type targetType, ObjectInfo targetObjectInfo)
		{
			Initialize(owner, propertyName, fieldName, isPrimaryKey, fieldType, nullable, maximumLength);
			this.targetType = targetType;
			this.targetObjectInfo = targetObjectInfo;
		}
		#endregion

		#region TargetType
		/// <summary>
		/// Typ, jen� property nese.
		/// </summary>
		public Type TargetType
		{
			get { return targetType; }
		}
		private Type targetType; 
		#endregion

		#region TargetObjectInfo
		/// <summary>
		/// Deleg�t na metodu vracej�c� objekt na z�klad� ID.
		/// </summary>
		public ObjectInfo TargetObjectInfo
		{
			get
			{
				return targetObjectInfo;
			}
		}
		private ObjectInfo targetObjectInfo; 
		#endregion
	}
}

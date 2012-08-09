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
		/// <summary>
		/// Vytvo�� instanci sloupce.
		/// </summary>
		/// <param name="fieldName">N�zev sloupce v datab�zy.</param>
		/// <param name="isPrimaryKey">Indikuje, zda je sloupec prim�rn�m kl��em</param>
		/// <param name="nullable">Indukuje, zda je povolena hodnota null.</param>
		/// <param name="fieldType">Typ datab�zov�ho sloupce.</param>
		/// <param name="maximumLength">Maxim�ln� d�lka dat datab�zov�ho sloupce.</param>		
		/// <param name="targetType">Typ, jen� property nese.</param>
		/// <param name="targetObjectInfo">ObjectInfo na typ, jen� property nese.</param>
		public ReferenceFieldPropertyInfo(string fieldName, bool isPrimaryKey, SqlDbType fieldType, bool nullable, int maximumLength, Type targetType, ObjectInfo targetObjectInfo)
			: base(fieldName, isPrimaryKey, fieldType, nullable, maximumLength)
		{
			this.targetType = targetType;
			this.targetObjectInfo = targetObjectInfo;
		}

		/// <summary>
		/// Typ, jen� property nese.
		/// </summary>
		public Type TargetType
		{
			get { return targetType; }
		}
		private Type targetType;

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
	}
}

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
		/// <param name="memberType">Typ, jen� property nese.</param>
		/// <param name="getObjectMethod">Deleg�t na metodu vracej�c� objekt na z�klad� ID.</param>
		public ReferenceFieldPropertyInfo(string fieldName, bool isPrimaryKey, SqlDbType fieldType, bool nullable, int maximumLength, Type memberType, GetObjectDelegate getObjectMethod)
			: base(fieldName, isPrimaryKey, fieldType, nullable, maximumLength)
		{
			this.memberType = memberType;
			this.getObjectMethod = getObjectMethod;
		}

		/// <summary>
		/// Typ, jen� property nese.
		/// </summary>
		public Type MemberType
		{
			get { return memberType; }
		}
		private Type memberType;

		/// <summary>
		/// Deleg�t na metodu vracej�c� objekt na z�klad� ID.
		/// </summary>
		public GetObjectDelegate GetObjectMethod
		{
			get
			{
				return getObjectMethod;
			}
		}
		private GetObjectDelegate getObjectMethod;
	}
}

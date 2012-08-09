using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Havit.Business.Query;

namespace Havit.Business
{
	/// <summary>
	/// Reprezentuje sloupec v datab�zi,
	/// nese informace o dan�m sloupci a jeho vazbu na objektovou strukturu.
	/// </summary>
	[Serializable]
	public class FieldPropertyInfo : PropertyInfo, IFieldsBuilder, IOperand
	{
		/// <summary>
		/// Vytvo�� instanci sloupce.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="isPrimaryKey"></param>
		/// <param name="nullable"></param>
		/// <param name="fieldType"></param>
		public FieldPropertyInfo(string fieldName, bool isPrimaryKey, SqlDbType fieldType, bool nullable)
		{
			this.fieldName = fieldName;
			this.nullable = nullable;
			this.fieldType = fieldType;
			this.isPrimaryKey = isPrimaryKey;
		}

		/// <summary>
		/// N�zev sloupce v datab�zi.
		/// </summary>
		public string FieldName
		{
			get { return fieldName; }
		}
		private string fieldName;

		/// <summary>
		/// Ud�v�, zda je mo�n� ulo�it null hodnotu.
		/// </summary>
		public bool Nullable
		{
			get { return nullable; }
		}
		private bool nullable;

		/// <summary>
		/// Typ sloupce v datab�zi.
		/// </summary>
		public SqlDbType FieldType
		{
			get { return fieldType; }
		}
		private SqlDbType fieldType;

		/// <summary>
		/// Ud�v�, zda je sloupec prim�rn�m kl��em.
		/// </summary>
		public bool IsPrimaryKey
		{
			get { return isPrimaryKey; }
		}
		private bool isPrimaryKey;

		/// <summary>
		/// Vr�t� �et�zec pro vyta�en� dan�ho sloupce z datab�ze.
		/// </summary>
		public virtual string GetSelectFieldStatement(DbCommand command)
		{
			return fieldName;
		}

		string IOperand.GetCommandValue(DbCommand command)
		{
			return fieldName;
		}
	}
}

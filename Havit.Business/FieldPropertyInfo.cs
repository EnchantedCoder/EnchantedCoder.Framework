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
		public void Initialize(ObjectInfo owner, string propertyName, string fieldName, bool isPrimaryKey, SqlDbType fieldType, bool nullable, int maximumLength)
		{
			Initialize(owner, propertyName);
			this.fieldName = fieldName;
			this.nullable = nullable;
			this.fieldType = fieldType;
			this.isPrimaryKey = isPrimaryKey;
			this.maximumLength = maximumLength;
		} 
		#endregion

		#region FieldName
		/// <summary>
		/// N�zev sloupce v datab�zi.
		/// </summary>
		public string FieldName
		{
			get
			{
				CheckInitialization();
				return fieldName;
			}
		}
		private string fieldName; 
		#endregion

		#region Nullable
		/// <summary>
		/// Ud�v�, zda je mo�n� ulo�it null hodnotu.
		/// </summary>
		public bool Nullable
		{
			get
			{
				CheckInitialization();
				return nullable;
			}
		}
		private bool nullable; 
		#endregion

		#region FieldType
		/// <summary>
		/// Typ sloupce v datab�zi.
		/// </summary>
		public SqlDbType FieldType
		{
			get
			{
				CheckInitialization();
				return fieldType;
			}
		}
		private SqlDbType fieldType; 
		#endregion

		#region IsPrimaryKey
		/// <summary>
		/// Ud�v�, zda je sloupec prim�rn�m kl��em.
		/// </summary>
		public bool IsPrimaryKey
		{
			get
			{
				CheckInitialization();
				return isPrimaryKey;
			}
		}
		private bool isPrimaryKey; 
		#endregion

		#region MaximumLength
		/// <summary>
		/// Maxim�ln� d�lka �et�zce (u typ� varchar, nvarchar, apod.), p��padn� velikost datov�ho typu (u typ� 
		/// </summary>
		public int MaximumLength
		{
			get
			{
				CheckInitialization();
				return maximumLength;
			}

		}
		private int maximumLength; 
		#endregion

		#region GetSelectFieldStatement
		/// <summary>
		/// Vr�t� �et�zec pro vyta�en� dan�ho sloupce z datab�ze.
		/// </summary>
		public virtual string GetSelectFieldStatement(DbCommand command)
		{
			CheckInitialization();
			return "[" + fieldName + "]";
		} 
		#endregion

		#region IOperand.GetCommandValue
		string IOperand.GetCommandValue(DbCommand command)
		{
			CheckInitialization();
			return fieldName;
		} 
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Data;

namespace Havit.Data
{
	/// <summary>
	/// DataRecord usnad�uje na��t�n� dat objektu z datab�ze.
	/// Zejm�na vhodn� je pro situace, kdy je mo�n� i ��ste�n� na��t�n�.
	/// </summary>
	/// <remarks>
	/// DataRecord pracuje tak, �e v constructoru zkop�ruje cel� datov� z�znam do slovn�ku Dictionary&lt;field, value&gt;.
	/// V jednotliv�ch Loadech pak u� jenom na��t� data ze slovn�ku.<br/>
	/// Datov� zdroj je tedy pot�eba pouze v okam�iku vol�n� constructoru a n�sledn� ho m��eme zlikvidovat.<br/>
	/// Stejn�tak je vhodn� pou��t na v�echny loady jeden DataRecord a p�ed�vat si ho mezi objekty.
	/// </remarks>
	public class DataRecord
	{
		#region Properties
		/// <summary>
		/// Indikuje, zda-li je po�adov�na 100% �sp�nost pro na��t�n� polo�ek (true), nebo zda-li se maj� ne�sp�chy ignorovat.
		/// </summary>
		public bool FullLoad
		{
			get { return fullLoad; }
			set { fullLoad = value; }
		}
		private bool fullLoad;
		#endregion

		#region private data fields
		/// <summary>
		/// Data z datab�ze.
		/// </summary>
		private Dictionary<string, object> dataDictionary;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.DataRow"/>.
		/// </summary>
		/// <param name="row">datov� zdroj typu DataRow</param>
		/// <param name="fullLoad">true, m�-li b�t p�i nenalezen� parametru vyvol�na v�jimka</param>
		public DataRecord(DataRow row, bool fullLoad)
		{
			this.fullLoad = fullLoad;

			// zkop�ruje data do dataDictionary
			this.dataDictionary = new Dictionary<string, object>(row.Table.Columns.Count);
			for (int i = 0; i < row.Table.Columns.Count; i++)
			{
				if (!this.dataDictionary.ContainsKey(row.Table.Columns[i].ColumnName))
				{
					this.dataDictionary.Add(row.Table.Columns[i].ColumnName, row[i]);
				}
			}
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.DataRow"/>.
		/// </summary>
		/// <param name="row">datov� zdroj typu <see cref="System.Data.DataRow"/></param>
		public DataRecord(DataRow row)
		{
			this.fullLoad = true;

			// zkop�ruje data do dataDictionary
			this.dataDictionary = new Dictionary<string, object>(row.Table.Columns.Count);
			for (int i = 0; i < row.Table.Columns.Count; i++)
			{
				if (!this.dataDictionary.ContainsKey(row.Table.Columns[i].ColumnName))
				{
					this.dataDictionary.Add(row.Table.Columns[i].ColumnName, row[i]);
				}
			}
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.IDataRecord"/>
		/// (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>).
		/// </summary>
		/// <param name="record">datov� zdroj <see cref="System.Data.IDataRecord"/> (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>)</param>
		/// <param name="fullLoad">true, m�-li b�t p�i nenalezen� parametru vyvol�na v�jimka</param>
		public DataRecord(IDataRecord record, bool fullLoad)
		{
			this.fullLoad = fullLoad;

			// zkop�ruje data do dataDictionary
			this.dataDictionary = new Dictionary<string, object>(record.FieldCount);
			for (int i = 0; i < record.FieldCount; i++)
			{
				if (!this.dataDictionary.ContainsKey(record.GetName(i)))
				{
					this.dataDictionary.Add(record.GetName(i), record[i]);
				}
			}
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.IDataRecord"/>
		/// (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>).
		/// </summary>
		/// <param name="record">datov� zdroj <see cref="System.Data.IDataRecord"/> (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>)</param>
		public DataRecord(IDataRecord record)
		{
			this.fullLoad = true;

			// zkop�ruje data do dataDictionary
			this.dataDictionary = new Dictionary<string, object>(record.FieldCount);
			for (int i = 0; i < record.FieldCount; i++)
			{
				if (!this.dataDictionary.ContainsKey(record.GetName(i)))
				{
					this.dataDictionary.Add(record.GetName(i), record[i]);
				}
			}
		}
		#endregion

		#region Indexer
		/// <summary>
		/// Indexer pro z�sk�n� k prvku pomoc� n�zvu pole.
		/// </summary>
		/// <param name="field">pole, sloupec</param>
		/// <returns>hodnota</returns>
		public object this[string field]
		{
			get
			{
				return this.dataDictionary[field];
			}
		}
		#endregion

		#region Load<T>, Get<T>
		/// <summary>
		/// Na�te parametr zadan�ho generick�ho typu T.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool Load<T>(string fieldName, ref T target)
		{
			if (dataDictionary.ContainsKey(fieldName))
			{
				if (dataDictionary[fieldName] != DBNull.Value)
				{
					try
					{
						target = (T)dataDictionary[fieldName];
					}
					catch (InvalidCastException e)
					{
						throw new InvalidCastException("Specified cast is not valid, field type is " + dataDictionary[fieldName].GetType().FullName, e);
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (fullLoad)
			{
				throw new ArgumentException("Parametr ve vstupn�ch datech nebyl nalezen", fieldName);
			}
			return false;
		}

		/// <summary>
		/// Vr�t� parametr zadan�ho generick�ho typu.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>
		/// vr�t� hodnotu typu T;<br/>
		/// pokud parametr neexistuje a nen� FullLoad, pak vrac� default(T), ve FullLoad h�z� v�jimku ArgumentException;<br/>
		/// pokud m� parametr hodnotu NULL, pak vrac� <c>null</c> pro referen�n� typy, pro hodnotov� typy h�z� v�jimku InvalidCastException<br/>
		/// </returns>
		public T Get<T>(string fieldName)
		{
			object value;
			if (dataDictionary.TryGetValue(fieldName, out value))
			{
				if (value == DBNull.Value)
				{
					if (default(T) != null)
					{
						throw new InvalidCastException("Hodnota NULL nelze p�ev�st na ValueType.");
					}
					return default(T);
				}
				else
				{
					if (value is T)
					{
						return (T)value;
					}
					else if (value is IConvertible)
					{
						return (T)Convert.ChangeType(value, typeof(T));
					}
					else
					{
						try
						{
							return (T)dataDictionary[fieldName];
						}
						catch (InvalidCastException e)
						{
							throw new InvalidCastException("Specified cast is not valid, field '" + fieldName + "', type " + dataDictionary[fieldName].GetType().FullName, e);
						}
					}
				}
			}
			else if (fullLoad)
			{
				throw new ArgumentException("Parametr po�adovan�ho jm�na nebyl v DataRecordu nalezen.", fieldName);
			}

			return default(T);
		}
		#endregion

		#region LoadObject, GetObject
		/// <summary>
		/// Na�te parametr typu Object.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadObject(string fieldName, ref object target)
		{
			return Load<object>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu Object.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu Object</returns>
		public object GetObject(string fieldName)
		{
			return Get<object>(fieldName);
		}
		#endregion

		#region LoadString, GetString
		/// <summary>
		/// Na�te parametr typu string.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadString(string fieldName, ref string target)
		{
			return Load<string>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu String.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu String</returns>
		public string GetString(string fieldName)
		{
			return Get<string>(fieldName);
		}
		#endregion

		#region LoadInt32, GetNullableInt32
		/// <summary>
		/// Na�te parametr typu Int32.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadInt32(string fieldName, ref Int32 target)
		{
			return Load<Int32>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu Int32?.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu Int32</returns>
		public Int32? GetNullableInt32(string fieldName)
		{
			return Get<Int32?>(fieldName);
		}
		#endregion

		#region LoadDouble, GetNullableDouble
		/// <summary>
		/// Na�te parametr typu Double.
		/// </summary>
		/// <param name="field">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadDouble(string fieldName, ref double target)
		{
			return Load<double>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu Double?.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu Double</returns>
		public double? GetNullableDouble(string fieldName)
		{
			return Get<double?>(fieldName);
		}

		#endregion

		#region LoadBoolean, GetNullableBoolean
		/// <summary>
		/// Na�te parametr typu Boolean.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadBoolean(string fieldName, ref bool target)
		{
			return Load<Boolean>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu bool?.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu Boolean</returns>
		public bool? GetNullableBoolean(string fieldName)
		{
			return Get<bool?>(fieldName);
		}
		#endregion

		#region LoadDateTime, GetNullableDateTime
		/// <summary>
		/// Na�te parametr typu DateTime.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		public bool LoadDateTime(string fieldName, ref DateTime target)
		{
			return Load<DateTime>(fieldName, ref target);
		}

		/// <summary>
		/// Vr�t� parametr typu DateTime?.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>null, pokud m� parametr hodnotu NULL, nebo neexistuje; jinak hodnota typu DateTime</returns>
		public DateTime? GetNullableDateTime(string fieldName)
		{
			return Get<DateTime?>(fieldName);
		}
		#endregion
	}
}

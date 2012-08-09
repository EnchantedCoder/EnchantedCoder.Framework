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
			get { return DataLoadPower == DataLoadPower.FullLoad; }
//			set { 
//				fullLoad = value; }
		}
		//private bool fullLoad;

		/// <summary>
		/// Indikuje mno�stv� dat, kter� jsou ulo�eny v DataRecordu v��i v�em mo�n�m sloupc�m ��dk�.
		/// </summary>
		public DataLoadPower DataLoadPower
		{
			get { return dataLoadPower; }
			set { dataLoadPower = value; }
		}
		private DataLoadPower dataLoadPower;
		
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
		/// <param name="dataLoadPower">Rozsah dat v datov�m zdroji.</param>
		public DataRecord(DataRow row, DataLoadPower dataLoadPower)
		{
			this.dataLoadPower = dataLoadPower;

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
		/// <param name="row">datov� zdroj typu DataRow</param>
		/// <param name="fullLoad">true, m�-li b�t p�i nenalezen� parametru vyvol�na v�jimka</param>
		[Obsolete]
		public DataRecord(DataRow row, bool fullLoad): this(row, fullLoad ? DataLoadPower.FullLoad : DataLoadPower.PartialLoad)
		{
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.DataRow"/>.
		/// </summary>
		/// <param name="row">datov� zdroj typu <see cref="System.Data.DataRow"/></param>
		[Obsolete]
		public DataRecord(DataRow row): this(row, true)
		{
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.IDataRecord"/>
		/// (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>).
		/// </summary>
		/// <param name="record">datov� zdroj <see cref="System.Data.IDataRecord"/> (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>)</param>
		/// <param name="dataLoadPower">Rozsah dat v datov�m zdroji.</param>
		public DataRecord(IDataRecord record, DataLoadPower dataLoadPower)
		{
			this.dataLoadPower = dataLoadPower;

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
		/// <param name="fullLoad">true, m�-li b�t p�i nenalezen� parametru vyvol�na v�jimka</param>
		[Obsolete]
		public DataRecord(IDataRecord record, bool fullLoad): this(record, fullLoad ? DataLoadPower.FullLoad : DataLoadPower.PartialLoad)
		{
		}

		/// <summary>
		/// Vytvo�� instanci DataRecordu a na�te do n� data z <see cref="System.Data.IDataRecord"/>
		/// (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>).
		/// </summary>
		/// <param name="record">datov� zdroj <see cref="System.Data.IDataRecord"/> (nap�. <see cref="System.Data.SqlClient.SqlDataReader"/>)</param>
		[Obsolete]
		public DataRecord(IDataRecord record): this(record, true)
		{			
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

		#region Get<T>, TryGet<T>, Load<T>
		/// <summary>
		/// Na�te parametr zadan�ho generick�ho typu T.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>
		/// <c>true</c>, pokud byla na�tena hodnota;<br/>
		/// <c>false</c>, pokud field v data recordu nen� a vlastnost <see cref="FullLoad"/> je <c>false</c> (target je pak nastaven na <c>default(T)</c>);<br/>
		/// </returns>
		/// <exception cref="ArgumentException">pokud field v data recordu nen� a vlastnost <see cref="FullLoad"/> je <c>true</c></exception>
		/// <exception cref="InvalidCastException">pokud nelze p�ev�st field na v�stupn� typ, nebo pokud je field <see cref="DBNull"/> a v�stupn� typ nem� <c>null</c></exception>
		public bool TryGet<T>(string fieldName, out T target)
		{
			target = default(T);
			object value;
			if (dataDictionary.TryGetValue(fieldName, out value))
			{
				if (value == DBNull.Value)
				{
					if (default(T) != null)
					{
						throw new InvalidCastException("Hodnota NULL nelze p�ev�st na ValueType.");
					}
					target = default(T); // null
				}
				else
				{
					if (value is T)
					{
						target = (T)value;
					}
					else
					{
						if (value is IConvertible)
						{
							try
							{
								target = (T)Convert.ChangeType(value, typeof(T));	 // posledn� pokus nap�. pro konverzi decimal -> double
							}
							catch (InvalidCastException e)
							{
								throw new InvalidCastException("Specified cast is not valid, field '" + fieldName + "', from " + value.GetType().FullName, e);
							}
						}
					}
				}

				return true;
			}
			else if (dataLoadPower == DataLoadPower.FullLoad)
			{
				throw new ArgumentException("Parametr po�adovan�ho jm�na nebyl v DataRecordu nalezen.", fieldName);
			}
			else
			{
				target = default(T);
				return false;
			}
		}

		/// <summary>
		/// Vr�t� parametr zadan�ho generick�ho typu.
		/// </summary>
		/// <remarks>
		/// Mimo castingu se pokou�� i o konverzi typu pomoc� IConvertible.
		/// </remarks>
		/// <param name="fieldName">jm�no parametru</param>
		/// <returns>
		/// vr�t� hodnotu typu T;<br/>
		/// pokud parametr neexistuje a nen� <see cref="FullLoad"/>, pak vrac� <c>default(T)</c>, ve FullLoad h�z� v�jimku ArgumentException;<br/>
		/// pokud m� parametr hodnotu NULL, pak vrac� <c>null</c> pro referen�n� typy, pro hodnotov� typy h�z� v�jimku InvalidCastException<br/>
		/// </returns>
		/// <exception cref="ArgumentException">pokud field v data recordu nen� a vlastnost <see cref="FullLoad"/> je <c>true</c></exception>
		/// <exception cref="InvalidCastException">pokud nelze p�ev�st field na v�stupn� typ, nebo pokud je field <see cref="DBNull"/> a v�stupn� typ nem� <c>null</c></exception>
		public T Get<T>(string fieldName)
		{
			T target;
			TryGet<T>(fieldName, out target);
			return target;
		}

		/// <summary>
		/// Na�te parametr zadan�ho generick�ho typu T.
		/// </summary>
		/// <remarks>
		/// Narozd�l od <see cref="TryGet{T}(string, out T)"/> neindikuje p��tomnost fieldu v data recordu, n�br� je-li field roven <see cref="DBNull"/>.<br/>
		/// Pokud je field <see cref="DBNull"/>, pak parametr <c>target</c> nezm�n�
		/// </remarks>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>
		/// <c>false</c>, pokud m� field hodnotu <see cref="DBNull"/>;<br/>
		/// <c>false</c>, pokud nebyl field nalezen a <see cref="FullLoad"/> je <c>false</c>;
		/// <c>true</c>, pokud byla na�tena hodnota
		/// </returns>
		/// <exception cref="ArgumentException">pokud nebyl field nalezen a <see cref="FullLoad"/> je <c>true</c></exception>
		/// <exception cref="InvalidCastException">pokud nelze p�ev�st field na v�stupn� typ</exception>
		[Obsolete("Metoda Load<T>() je obsolete, pou�ijte TryGet<T>().")]
		public bool Load<T>(string fieldName, ref T target)
		{
			object value;
			if (dataDictionary.TryGetValue(fieldName, out value))
			{
				if (value == DBNull.Value)
				{
					// nem�n�me hodnotu target
					return false;
				}
				else
				{
					if (value is T)
					{
						target = (T)value;
						return true;
					}
					else
					{
						if (value is IConvertible)
						{
							try
							{
								target = (T)Convert.ChangeType(value, typeof(T));	 // posledn� pokus nap�. pro konverzi decimal -> double
								return true;
							}
							catch (InvalidCastException e)
							{
								throw new InvalidCastException("Specified cast is not valid, field '" + fieldName + "', from " + value.GetType().FullName, e);
							}
						}
					}
				}
			}
			else if (dataLoadPower == DataLoadPower.FullLoad)
			{
				throw new ArgumentException("Parametr ve vstupn�ch datech nebyl nalezen", fieldName);
			}
			return false;
		}
		#endregion

		#region LoadObject, GetObject
		/// <summary>
		/// Na�te parametr typu Object.
		/// </summary>
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		[Obsolete("Metoda LoadObject() je obsolete, pou�ijte TryGet<object>().")]
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
		[Obsolete("Metoda LoadString() je obsolete, pou�ijte TryGet<string>().")]
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
		[Obsolete("Metoda LoadInt32() je obsolete, pou�ijte TryGet<int>().")]
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
		/// <param name="fieldName">jm�no parametru</param>
		/// <param name="target">c�l, kam m� b�t parametr ulo�en</param>
		/// <returns>false, pokud m� parametr hodnotu NULL; true, pokud byla na�tena hodnota</returns>
		[Obsolete("Metoda LoadDouble() je obsolete, pou�ijte TryGet<double>().")]
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
		[Obsolete("Metoda LoadBoolean() je obsolete, pou�ijte TryGet<bool>().")]
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
		[Obsolete("Metoda LoadDateTime() je obsolete, pou�ijte TryGet<DateTime>().")]
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

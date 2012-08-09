using System;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Havit.Data.SqlTypes
{
	/// <summary>
	/// UDT reprezentuj�c� pole SqlInt32 (SQL: int) hodnot.
	/// </summary>
	/// <remarks>
	/// Vzhledem k tomu, �e maxim�ln� velikost UDT je 8KB, maximem je 1999 hodnot (jedna je stavov�).
	/// Bin�rn� serializace je takov�to:
	/// byte 1-4 ~ Int32 Length (velikost pole, pokud je 0, pak je hodnota NULL)
	/// byte 5-(8000) ~ values
	/// </remarks>
	/// <example>
	/// Vytvo�en� UDT typu:
	/// <code>
	/// CREATE TYPE [dbo].IntArray
	/// EXTERNAL NAME [Havit.Data.SqlServer].[Havit.Data.SqlTypes.SqlInt32Array]
	/// </code>
	/// Vytvo�en� funkce:
	/// <code>
	/// CREATE FUNCTION IntArrayToTable
	/// (
	///     @array dbo.IntArray
	/// )
	/// RETURNS TABLE
	/// (
	///     [Value] int
	/// )
	/// AS EXTERNAL NAME [Havit.Data.SqlServer].[Havit.Data.SqlTypes.SqlInt32Array].[GetInt32Values]
	/// </code>
	/// Pou�it� ve filtru:
	/// <code>
	/// CREATE PROCEDURE Filter
	/// (
	///     @Vlastnosti dbo.IntArray = NULL
	/// )
	/// AS
	///     SELECT col FROM tab
	///         WHERE ((@Vlastnosti IS NULL) OR (VlastnostID IN (SELECT Value FROM dbo.IntArrayToTable(@Vlastnosti))))
	/// </code>
	/// </example>
	[Serializable]
	[SqlUserDefinedType(
		Format.UserDefined,
		Name = "IntArray",
		IsByteOrdered = true,
		MaxByteSize = 8000)]
	public class SqlInt32Array : INullable, IBinarySerialize
	{
		#region private value holder
		private List<SqlInt32> values = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci s hodnotou NULL.
		/// </summary>
		public SqlInt32Array()
		{
			this.values = null;
		}

		/// <summary>
		/// Vytvo�� instanci a napln� ji p�edan�mi hodnotami.
		/// </summary>
		/// <param name="values">hodnoty, kter� maj� instance reprezentovat</param>
		public SqlInt32Array(int[] values)
		{
			if ((values == null) || (values.Length == 0))
			{
				this.values = null;
				return;
			}
			if (values.Length > 1999)
			{
				throw new ArgumentException(String.Format("Maxim�ln� velikost pole je 1999 hodnot, po�adov�no je v�ak {0} hodnot.",
					values.Length));
			}

			this.values = new List<SqlInt32>();
			for (int i = 0; i < values.Length; i++)
			{
				this.values.Add(new SqlInt32(values[i]));
			}
		}
		#endregion

		#region Add
		/// <summary>
		/// P�id� prvek do pole.
		/// </summary>
		/// <param name="value"></param>
		public void Add(SqlInt32 value)
		{
			if (!value.IsNull && (value.Value == Int32.MinValue))
			{
				throw new ArgumentException("Prvek nesm� m�t vyhrazenou hodnotu Int32.MinValue.");
			}

			if (this.values == null)
			{
				this.values = new List<SqlInt32>();
			}
			values.Add(value);
		}
		#endregion

		#region Count
		/// <summary>
		/// Po�et prvk� v seznamu.
		/// </summary>
		public int Count
		{
			get { return values.Count; }
		}
		#endregion

		#region Indexer
		/// <summary>
		/// Indexer pro p��stup k prvk�m podle jejich po�ad�.
		/// </summary>
		/// <param name="index">index (po�ad�) prvku</param>
		/// <returns>hodnota <see cref="SqlInt32"/></returns>
		public SqlInt32 this[int index]
		{
			get { return values[index]; }
		}
		#endregion

		#region Merge
		/// <summary>
		/// Spoj� dv� pole v jedno.
		/// </summary>
		/// <param name="array">p�id�van� pole</param>
		public void Merge(SqlInt32Array array)
		{
			if (!array.IsNull)
			{
				for (int i = 0; i < array.values.Count; i++)
				{
					this.values.Add(array.values[i]);
				}
			}
		}
		#endregion

		#region Accessors
		/// <summary>
		/// Vr�t� pole SqlInt32[] s hodnotami.
		/// </summary>
		/// <returns>Pole SqlInt32[] s hodnotami.</returns>
		public SqlInt32[] GetSqlInt32Array()
		{
			if (this.IsNull)
			{
				return null;
			}
			return (SqlInt32[])values.ToArray();
		}

		/// <summary>
		/// Vrac� tabulku Int32 hodnot.
		/// Metoda ur�en� pro mapov�n� do T-SQL na table-valued function (TVF).
		/// </summary>
		/// <param name="values">Prom�nn�, kter� m� b�t rozbalena do tabulky hodnot Int32.</param>
		/// <returns>tabulka Int32 hodnot (pomoc� FillInt32Row)</returns>
		[SqlFunctionAttribute(
			Name= "IntArrayToTable",
			TableDefinition = "[Value] int",
			FillRowMethodName = "FillSqlInt32Row")]
		public static IEnumerable GetSqlInt32Values(SqlInt32Array values)
		{
			if (values != null)
			{
				return values.GetSqlInt32Array();
			}
			return null;
		}

		/// <summary>
		/// Metoda zaji��uj�c� p�evod ��dku v table-valued function (TVF).
		/// </summary>
		/// <param name="sqlInt32ArrayElement">vstupn� hodnota ��dku</param>
		/// <param name="value">v�stupn� hodnota ��dku</param>
		public static void FillSqlInt32Row(object sqlInt32ArrayElement, out SqlInt32 value)
		{
			value = (SqlInt32)sqlInt32ArrayElement;
		}
		#endregion

		#region Parse
		/// <summary>
		/// Vytvo�� z CSV textov� reprezentace hodnotu pole.
		/// </summary>
		/// <param name="text">CSV text hodnot</param>
		/// <returns>pole s hodnotami dle CSV</returns>
		[SqlMethod(DataAccess = DataAccessKind.None, SystemDataAccess = SystemDataAccessKind.None)]
		public static SqlInt32Array Parse(SqlString text)
		{
			if (text.IsNull)
			{
				return Null;
			}
			string[] parts = text.Value.Split(',');
			int length = parts.Length;
			SqlInt32Array result = new SqlInt32Array();
			for (int i = 0; i < length; i++)
			{
				if (String.Compare(parts[i].Trim(), "NULL", true) == 0)
				{
					result.Add(SqlInt32.Null);
				}
				else
				{
					result.Add(new SqlInt32(Convert.ToInt32(parts[i])));
				}
			}
			return result;
		}
		#endregion

		#region ToString
		/// <summary>
		/// P�evede hodnotu na CSV textovou reprezentaci string
		/// </summary>
		/// <returns>CSV seznam hodnot</returns>
		public override string ToString()
		{
			if (this.IsNull)
			{
				return null;
			}

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < this.values.Count; i++)
			{
				if (this.values[i].IsNull)
				{
					sb.Append("NULL");
				}
				else
				{
					sb.Append(this.values[i].Value);
				}
				if (i < this.values.Count - 1)
				{
					sb.Append(",");
				}
			}
			return sb.ToString();
		}
		#endregion

		#region Null (static)
		/// <summary>
		/// Hodnota NULL.
		/// </summary>
		public static SqlInt32Array Null
		{
			get
			{
				return new SqlInt32Array();
			}
		}
		#endregion

		#region INullable Members
		/// <summary>
		/// Indikuje, zda-li je hodnota NULL.
		/// </summary>
		public bool IsNull
		{
			get
			{
				return ((this.values == null) || (this.values.Count == 0));
			}
		}
		#endregion

		#region IBinarySerialize Members
		/// <summary>
		/// Na�te hodnotu z bin�rn� reprezentace.
		/// </summary>
		/// <remarks>
		/// Bin�rn� serializace je takov�to:
		/// byte 1-4 ~ Int32 Length (velikost pole, pokud je 0, pak je hodnota NULL)
		/// byte 5-(8000) ~ values (NULL hodnoty reprezentuje Int32.MinValue)
		/// </remarks>
		/// <param name="r"><see cref="System.IO.BinaryReader"/> s bin�rn� reprezentac� hodnoty</param>
		public void Read(System.IO.BinaryReader r)
		{
			// byte 1 - po�et hodnot
			Int32 length = r.ReadInt32();
			if (length == 0)
			{
				// NULL
				this.values = null;
			}
			else
			{
				// hodnoty
				this.values = new List<SqlInt32>();
				for (int i = 0; i < length; i++)
				{
					Int32 temp = r.ReadInt32();
					if (temp == Int32.MinValue)
					{
						this.values.Add(SqlInt32.Null);
					}
					else
					{
						this.values.Add(new SqlInt32(temp));
					}
				}
			}
		}

		/// <summary>
		/// Vytvo�� bin�rn� reprezentace hodnoty.
		/// </summary>
		/// <remarks>
		/// Bin�rn� serializace je takov�to:
		/// byte 1-4 ~ Int32 Length (velikost pole, pokud je 0, pak je hodnota NULL)
		/// byte 5-(8000) ~ values (NULL hodnoty implementuje Int32.MinValue)
		/// </remarks>
		/// <param name="w"><see cref="System.IO.BinaryWriter"/> do kter�ho m� b�t bin�rn� reprezentace zaps�na</param>
		public void Write(System.IO.BinaryWriter w)
		{
			// byte 1 - po�et hodnot
			if (this.IsNull)
			{
				w.Write(0);
			}
			else
			{
				w.Write(this.values.Count);

				// hodnoty
				for (int i = 0; i < this.values.Count; i++)
				{
					if (this.values[i].IsNull)
					{
						w.Write(Int32.MinValue);
					}
					else
					{
						w.Write(this.values[i].Value);
					}
				}
			}
		}
		#endregion
	}
}


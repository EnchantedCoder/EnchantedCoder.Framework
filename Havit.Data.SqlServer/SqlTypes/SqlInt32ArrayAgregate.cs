using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;

namespace Havit.Data.SqlTypes
{
	/// <summary>
	/// Aggregate k UDT SqlInt32Array, kter� zaji��uje p�evod tabulky hodnot na pole.
	/// </summary>
	/// <example>
	/// Vytvo�en� agreg�tu typu:<br/>
	/// <code>
	/// CREATE AGGREGATE [dbo].IntArrayAggregate<br/>
	/// RETURNS IntArray<br/>
	/// EXTERNAL NAME [Havit.Data.SqlServer].[Havit.Data.SqlTypes.SqlInt32ArrayAggregate]<br/>
	/// </code>
	/// Pou�it� agreg�tu pro vytvo�en� pole hodnot:<br/>
	/// <code>
	/// SELECT IntArrayAggreagate(ItemID) AS Items FROM dbo.Item WHERE ...<br/>
	/// </code>
	/// </example>
	[Serializable]
	[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
		Format.UserDefined,
		IsInvariantToDuplicates = false, IsInvariantToNulls = false, IsInvariantToOrder = true, IsNullIfEmpty = true,
		MaxByteSize = 8000,	Name = "IntArrayAggregate")]
	public class SqlInt32ArrayAggregate : IBinarySerialize
	{
		#region private value holder
		/// <summary>
		/// Uchov�v� meziv�sledek.
		/// </summary>
		private SqlInt32Array array;
		#endregion

		#region Init
		/// <summary>
		/// Inicializace agreg�toru.
		/// </summary>
		public void Init()
		{
			array = new SqlInt32Array();
		}
		#endregion

		#region Accumulate
		/// <summary>
		/// P�id� dal�� hodnotu do agregace.
		/// </summary>
		/// <param name="value">p�id�van� hodnota</param>
		public void Accumulate(SqlInt32 value)
		{
			array.Add(value);
		}
		#endregion

		#region Merge
		/// <summary>
		/// Spoj� dva agreg�ty v jeden
		/// </summary>
		/// <param name="group">druh� agregace</param>
		public void Merge(SqlInt32ArrayAggregate group)
		{
			group.array.Merge(group.array);
		}
		#endregion

		#region Terminate
		/// <summary>
		/// Vr�t� v�sledek agregace.
		/// </summary>
		public SqlInt32Array Terminate()
		{
			return this.array;
		}
		#endregion

		#region IBinarySerialize Members
		/// <summary>
		/// De-serializuje agregaci.
		/// </summary>
		/// <param name="r">BinaryReader</param>
		public void Read(BinaryReader r)
		{
			this.array = new SqlInt32Array();
			this.array.Read(r);
		}

		/// <summary>
		/// Serializuje agregaci.
		/// </summary>
		/// <param name="w">BinaryWriter</param>
		public void Write(BinaryWriter w)
		{
			this.array.Write(w);
		}
		#endregion
	}
}
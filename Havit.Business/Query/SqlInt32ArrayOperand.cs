using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Havit.Data.SqlTypes;

namespace Havit.Business.Query
{
	/// <summary>
	/// SqlInt32Array jako operand datab�zov�ho dotazu.
	/// </summary>
	[Serializable]
	public sealed class SqlInt32ArrayOperand : IOperand
	{
		#region Private fields
		/// <summary>
		/// Hodnota konstanty ValueOperandu.
		/// </summary>
		int[] value;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci t��dy SqlInt32ArrayOperand.
		/// </summary>
		private SqlInt32ArrayOperand(int[] value)
		{
			this.value = value;
		}
		#endregion

		#region IOperand Members
		string IOperand.GetCommandValue(System.Data.Common.DbCommand command)
		{
			if (!(command is SqlCommand))
			{
				throw new ArgumentException("Typ SqlInt32ArrayOperand p�edpokl�d� SqlCommand.");	
			}

			SqlCommand sqlCommand = command as SqlCommand;

			string parameterName;
			int index = 1;
			do
			{
				parameterName = "@param" + (command.Parameters.Count + index).ToString();
				index += 1;
			} while (command.Parameters.Contains(parameterName));

			SqlParameter parameter = new SqlParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = new SqlInt32Array(value);
			parameter.SqlDbType = SqlDbType.Udt;
			parameter.UdtTypeName = "IntArray";
			sqlCommand.Parameters.Add(parameter);

			return parameterName;
		}
		#endregion

		#region Create
		/// <summary>
		/// Vytvo�� operand z pole integer�.
		/// </summary>
		public static IOperand Create(int[] ids)
		{
			return new SqlInt32ArrayOperand(ids);
		}
		#endregion

	}
}

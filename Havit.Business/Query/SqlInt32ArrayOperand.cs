﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Havit.Data.SqlTypes;

namespace Havit.Business.Query
{
	/// <summary>
	/// SqlInt32Array jako operand databázového dotazu.
	/// </summary>
	public sealed class SqlInt32ArrayOperand : IOperand
	{
		#region Private fields
		/// <summary>
		/// Hodnota konstanty ValueOperandu.
		/// </summary>
		private int[] value;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvoří instanci třídy SqlInt32ArrayOperand.
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
				throw new ArgumentException("Typ SqlInt32ArrayOperand předpokládá SqlCommand.");	
			}

			SqlCommand sqlCommand = command as SqlCommand;

			string parameterName;
			int index = 1;
			do
			{
				parameterName = "@param" + (command.Parameters.Count + index).ToString();
				index += 1;
			}
			while (command.Parameters.Contains(parameterName));

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
		/// Vytvoří operand z pole integerů.
		/// </summary>
		public static IOperand Create(int[] ids)
		{
			return new SqlInt32ArrayOperand(ids);
		}
		#endregion

	}
}

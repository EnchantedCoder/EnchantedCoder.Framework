using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Havit.Business.Query
{
	/// <summary>
	/// Konstanta jako operand datab�zov�ho dotazu.
	/// </summary>
	public class ValueOperand: IOperand
	{
	
		#region Private fields		
		/// <summary>
		/// Hodnota konstanty ValueOperandu.
		/// </summary>
		object value;

		/// <summary>
		/// Datab�zov� typ nesen� ValueOperandem.
		/// </summary>
		DbType dbType;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci t��dy ValueOperand.
		/// </summary>
		public ValueOperand(object value, DbType dbType)
		{
			this.value = value;
			this.dbType = dbType;
		}
		#endregion

		#region IOperand Members
		string IOperand.GetCommandValue(System.Data.Common.DbCommand command)
		{
			string parameterName;
			do
			{
				parameterName = "@param" + new Random().Next(Int32.MaxValue);
			} while (command.Parameters.Contains(parameterName));

			DbParameter parameter = command.CreateParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = value;
			parameter.DbType = dbType;
			command.Parameters.Add(parameter);

			return parameterName;
		}

		#endregion
		
		/// <summary>
		/// Vytvo�� operand z �et�zce.
		/// </summary>
		public static IOperand Create(string value)
		{
			return new ValueOperand(value, DbType.String);
		}

		#region Create - integery
		/// <summary>
		/// Vytvo�� operand z cel�ho ��sla.
		/// </summary>
		public static IOperand Create(Int16 value)
		{
			return new ValueOperand(value, DbType.Int16);
		}

		/// <summary>
		/// Vytvo�� operand z cel�ho ��sla.
		/// </summary>
		public static IOperand Create(Int32 value)
		{
			return new ValueOperand(value, DbType.Int32);
		}

		/// <summary>
		/// Vytvo�� operand z cel�ho ��sla.
		/// </summary>
		public static IOperand Create(Int64 value)
		{
			return new ValueOperand(value, DbType.Int64);
		}
		#endregion
		
		/// <summary>
		/// Vytvo�� operand z logick� hodnoty.
		/// </summary>
		public static IOperand Create(bool value)
		{
			return new ValueOperand(value, DbType.Boolean);
		}


	}
}

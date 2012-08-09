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

		#region Create - Boolean
		/// <summary>
		/// Vytvo�� operand z logick� hodnoty.
		/// </summary>
		public static IOperand Create(bool value)
		{
			return new ValueOperand(value, DbType.Boolean);
		}
		#endregion

		#region Create - DateTime
		/// <summary>
		/// Vytvo�� operand z DateTime.
		/// </summary>
		public static IOperand Create(DateTime value)
		{
			return new ValueOperand(value, DbType.DateTime);
		}
		#endregion

		#region Create - Int16, Int32, Int64
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

		#region Create - Single, Double, Decimal
		/// <summary>
		/// Vytvo�� operand z ��sla.
		/// </summary>
		public static IOperand Create(Single value)
		{
			return new ValueOperand(value, DbType.Single);
		}

		/// <summary>
		/// Vytvo�� operand z ��sla.
		/// </summary>
		public static IOperand Create(Double value)
		{
			return new ValueOperand(value, DbType.Double);
		}

		/// <summary>
		/// Vytvo�� operand z ��sla.
		/// </summary>
		public static IOperand Create(decimal value)
		{
			return new ValueOperand(value, DbType.Decimal);
		}
		#endregion

		#region Create - GUID
		/// <summary>
		/// Vytvo�� operand z GUIDu.
		/// </summary>
		public static IOperand Create(Guid value)
		{
			return new ValueOperand(value, DbType.Guid);
		}
		#endregion

		#region Create - String
		/// <summary>
		/// Vytvo�� operand z �et�zce.
		/// </summary>
		public static IOperand Create(string value)
		{
			return new ValueOperand(value, DbType.String);
		}
		#endregion
	}
}

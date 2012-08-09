using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Havit.Business.Query
{
	/// <summary>
	/// Objektov� struktura SQL dotazu.
	/// Obsahuje seznam properties, kter� se maj� z datab�ze z�skat, seznam podm�nek fitruj�c� z�znamy
	/// a �azen� v jak�m se maj� z�znamy (objekty) z�skat.
	/// </summary>
	[Serializable]	
	public class QueryParams
	{
		#region Parametry dotazu
		/// <summary>
		/// Maxim�ln� po�et z�znam�, kter� se vrac� z datab�ze - (SELECT TOP n ...).
		/// </summary>
		public int? TopRecords
		{
			get { return topRecords;}
			set { topRecords = value;}
		}
		private int? topRecords;

		/// <summary>
		/// N�zev tabulky nebo view, do kter� se tvo�� dotaz (FROM xxx).
		/// </summary>
		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}
		private string tableName;

		/// <summary>
		/// Seznam sloupc� (sekce SQL dotazu SELECT), kter� se vyt�hnou v p��pad�, �e kolekce fields je pr�zdn�.
		/// </summary>
		internal string FieldsWhenEmpty
		{
			get { return fieldsWhenEmpty; }
			set { fieldsWhenEmpty = value; }
		}
		private string fieldsWhenEmpty;

		/// <summary>
		/// Seznam sloupc�, kter� jsou v�sledkem dotazu (SELECT sloupec1, sloupec2...).
		/// </summary>
		public PropertyCollection Properties
		{
		  get { return properties; }
		}
		private PropertyCollection properties = new PropertyCollection();

		/// <summary>
		/// Podm�nky dotazu (WHERE ...).
		/// </summary>
		public CompositeCondition Conditions
		{
		  get { return conditions; }
		}
		private AndCondition conditions = new AndCondition();

		/// <summary>
		/// Po�ad� z�znam� (ORDER BY ...).
		/// </summary>
		public OrderItemCollection OrderBy
		{
			get { return orderBy; }
		}
		private OrderItemCollection orderBy = new OrderItemCollection();
		#endregion

		#region PrepareCommand
		/// <summary>
		/// Vytvo�� dotaz, nastav� jej do commandu.
		/// P�id� parametry.
		/// </summary>
		/// <param name="command"></param>
		public void PrepareCommand(DbCommand command)
		{
			OnBeforePrepareCommand();

			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append(GetSelectStatement(command));
			commandBuilder.Append(" ");
			commandBuilder.Append(GetSelectFieldsStatement(command));
			commandBuilder.Append(" ");
			commandBuilder.Append(GetFromStatement(command));
			commandBuilder.Append(" ");
			//GetJoinStatement(commandBuilder, command);
			//commandBuilder.Append(" ");
			commandBuilder.Append(GetWhereStatement(command));
			commandBuilder.Append(" ");
			commandBuilder.Append(GetOrderByStatement(command));
			commandBuilder.Append(";");

			OnAfterPrepareCommand(command, commandBuilder);

			command.CommandText = command.CommandText + commandBuilder.ToString();
		}

		#endregion

		/// <summary>
		/// Slou�� k p��prav� objektu p�ed za��tkem skl�d�n� datab�zov�ho dotazu.
		/// </summary>
		public virtual void OnBeforePrepareCommand()
		{
		}

		/// <summary>
		/// Slou�� k dokon�en� skl�d�n� datab�zov�ho dotazu.
		/// Vol�no po poskl�d�n� datab�zov�ho dotazu, naskl�d�n� parametr� do commandu,
		/// ale P�ED nastaven�m property command.CommandText. Je tak mo�no datab�zov�
		/// dotaz upravit na posledn� chv�li.
		/// </summary>
		public virtual void OnAfterPrepareCommand(DbCommand command, StringBuilder commandBuilder)
		{
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu SELECT.
		/// </summary>
		protected virtual string GetSelectStatement(DbCommand command)
		{
			if (topRecords == null)
				return "SELECT";
			else
				return "SELECT TOP " + topRecords.Value.ToString();
		}

		/// <summary>
		/// Vr�t� seznam sloupc�, kter� se z datab�ze z�sk�vaj�.
		/// </summary>
		protected virtual string GetSelectFieldsStatement(DbCommand command)
		{
			if (properties.Count == 0)
				return fieldsWhenEmpty;

			StringBuilder fieldsBuilder = new StringBuilder();			
			for (int i = 0; i < properties.Count; i++)				
			{
				if (i > 0)
					fieldsBuilder.Append(", ");
				fieldsBuilder.Append(properties[i].GetSelectFieldStatement(command));
			}
			return fieldsBuilder.ToString();
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu FROM.
		/// </summary>
		protected virtual string GetFromStatement(DbCommand command)
		{
			return String.Format("FROM {0}", tableName);
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu WHERE.
		/// </summary>
		public virtual string GetWhereStatement(DbCommand command)
		{
			StringBuilder whereBuilder = new StringBuilder();
			
			Conditions.GetWhereStatement(command, whereBuilder);
			if (whereBuilder.Length > 0)
				whereBuilder.Insert(0, "WHERE ");
						
			return whereBuilder.ToString();
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu ORDER BY.
		/// </summary>
		protected virtual string GetOrderByStatement(DbCommand command)
		{
			if (orderBy.Count == 0)
				return String.Empty;
			
			StringBuilder orderByBuilder = new StringBuilder();
			orderByBuilder.Append("ORDER BY ");
			for (int i = 0; i < orderBy.Count; i++)
			{
				if (i > 0)
					orderByBuilder.Append(", ");
				orderByBuilder.Append(orderBy[i].GetSqlOrderBy());
			}
			return orderBy.ToString();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Globalization;
using Havit.Data;

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
		/// Instance t��dy ObjectInfo nesouc� informace o tom, z jak� tabulky se bude dotaz dotazovat.
		/// </summary>
		public ObjectInfo ObjectInfo
		{
			get { return objectInfo; }
			set { objectInfo = value; }
		}
		private ObjectInfo objectInfo;

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
		/// Ud�v�, zda se maj� vracet i z�znamy ozna�en� za smazan�.
		/// V�choz� hodnota je false, smazan� z�znamy se nevrac�.
		/// </summary>
		public bool IncludeDeleted
		{
			get { return includeDeleted; }
			set { includeDeleted = value; }
		}
		private bool includeDeleted = false;

		/// <summary>
		/// Seznam sloupc�, kter� jsou v�sledkem dotazu (SELECT sloupec1, sloupec2...).
		/// </summary>
		public PropertyInfoCollection Properties
		{
		  get { return properties; }
		}
		private PropertyInfoCollection properties = new PropertyInfoCollection();

		/// <summary>
		/// Podm�nky dotazu (WHERE ...).
		/// </summary>
		public ConditionList Conditions
		{
			get { return conditions.Conditions; }
		}
		private AndCondition conditions = new AndCondition();

		/// <summary>
		/// Po�ad� z�znam� (ORDER BY ...).
		/// </summary>
		public OrderByCollection OrderBy
		{
			get { return orderBy; }
		}
		private OrderByCollection orderBy = new OrderByCollection();
		#endregion

		#region GetDataLoadPower
		/// <summary>
		/// Podle kolekce properties ur�� re�im z�znam�, kter� budou vr�ceny.
		/// Pro pr�zdnou kolekci vrac� FullLoad, pro kolekci o jednom prvku, kter� je prim�rn�m kl��em, vrac� Ghost. Jinak vrac� PartialLoad.
		/// </summary>		
		public DataLoadPower GetDataLoadPower()
		{
			if (properties.Count == 0)
			{
				return DataLoadPower.FullLoad;
			}

			if (properties.Count == 1)
			{
				FieldPropertyInfo fieldPropertyInfo = properties[0] as FieldPropertyInfo;
				if ((fieldPropertyInfo != null) && (fieldPropertyInfo.IsPrimaryKey))
				{
					return DataLoadPower.Ghost;
				}
			}
			
			return DataLoadPower.PartialLoad;
		}
		#endregion

		#region PrepareCommand
		/// <summary>
		/// Vytvo�� dotaz, nastav� jej do commandu.
		/// P�id� parametry.
		/// </summary>
		/// <param name="command"></param>
		public void PrepareCommand(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

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
			commandBuilder.Append(" ");
			commandBuilder.Append(GetOptionStatementStatement(command));
			commandBuilder.Append(";");

			OnAfterPrepareCommand(command, commandBuilder);

			command.CommandText = command.CommandText + commandBuilder.ToString();
		}

		#endregion

		#region OnBeforePrepareCommand, OnAfterPrepareCommand
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
		#endregion

		#region SQL Statement builder
		/// <summary>
		/// Vr�t� sekci SQL dotazu SELECT.
		/// </summary>
		protected virtual string GetSelectStatement(DbCommand command)
		{
			if (topRecords == null)
			{
				return "SELECT";
			}
			else
			{
				return String.Format("SELECT TOP ({0})", topRecords.Value.ToString(CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// Vr�t� seznam sloupc�, kter� se z datab�ze z�sk�vaj�.
		/// </summary>
		protected virtual string GetSelectFieldsStatement(DbCommand command)
		{
			PropertyInfoCollection queryProperties = properties;

			if (queryProperties.Count == 0)
			{
				queryProperties = objectInfo.Properties;
			}

			StringBuilder fieldsBuilder = new StringBuilder();
			for (int i = 0; i < queryProperties.Count; i++)				
			{
				if (i > 0)
					fieldsBuilder.Append(", ");

				if (queryProperties[i] is IFieldsBuilder)
				{
#warning P�epracovat tak, aby ka�d� property obecn� mohl emitovat fieldy, kter� pot�ebuje ke sv� inicializaci.
					fieldsBuilder.Append(((IFieldsBuilder)queryProperties[i]).GetSelectFieldStatement(command));
				}
			}
			return fieldsBuilder.ToString();
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu FROM.
		/// </summary>
		protected virtual string GetFromStatement(DbCommand command)
		{
            if (String.IsNullOrEmpty(objectInfo.DbSchema))
            {
                return String.Format(CultureInfo.InvariantCulture, "FROM [{0}]", objectInfo.DbTable);
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, "FROM [{0}].[{1}]", objectInfo.DbSchema, objectInfo.DbTable);
            }
		}

		/// <summary>
		/// Vr�t� sekci SQL dotazu WHERE.
		/// </summary>
		public virtual string GetWhereStatement(DbCommand command)
		{
			StringBuilder whereBuilder = new StringBuilder();

			if (!includeDeleted && objectInfo.DeletedProperty != null)
			{
				if (objectInfo.DeletedProperty.FieldType == System.Data.SqlDbType.Bit)
				{
					Conditions.Add(BoolCondition.CreateFalse(objectInfo.DeletedProperty));
				}

				if ((objectInfo.DeletedProperty.FieldType == System.Data.SqlDbType.DateTime) || (objectInfo.DeletedProperty.FieldType == System.Data.SqlDbType.SmallDateTime))
				{
					Conditions.Add(NullCondition.CreateIsNull(objectInfo.DeletedProperty));
				}
			}

			conditions.GetWhereStatement(command, whereBuilder);
			if (whereBuilder.Length > 0)
			{
				whereBuilder.Insert(0, "WHERE ");
			}
						
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

#warning nen� moc OOP
				orderByBuilder.Append(orderBy[i].Expression);
				if (orderBy[i].Direction == Havit.Collections.SortDirection.Descending)
					orderByBuilder.Append(" DESC");
			}
			return orderByBuilder.ToString();
		}
		#endregion

		/// <summary>
		/// Vr�t� sekci SQL dotazu OPTION - pou�ito na OPTION (RECOMPILE).
		/// OPTION (RECOMPILE): workaround pro http://connect.microsoft.com/SQLServer/feedback/ViewFeedback.aspx?FeedbackID=256717
		/// </summary>
		protected virtual string GetOptionStatementStatement(DbCommand command)
		{
			PropertyInfoCollection queryProperties = properties;

			if (queryProperties.Count == 0)
			{
				queryProperties = objectInfo.Properties;
			}

			foreach (PropertyInfo propertyInfo in queryProperties)
			{
				if (propertyInfo is CollectionPropertyInfo)
				{
					return "OPTION (RECOMPILE)";
				}
			}
			return "";			
		}
	}
}

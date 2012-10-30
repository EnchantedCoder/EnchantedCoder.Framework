﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Globalization;
using Havit.Data;
using System.Diagnostics.Contracts;

namespace Havit.Business.Query
{
	/// <summary>
	/// Objektová struktura SQL dotazu.
	/// Obsahuje seznam properties, které se mají z databáze získat, seznam podmínek fitrující záznamy
	/// a řazení v jakém se mají záznamy (objekty) získat.
	/// </summary>
	public class QueryParams
	{
		#region Parametry dotazu
		/// <summary>
		/// Instance třídy ObjectInfo nesoucí informace o tom, z jaké tabulky se bude dotaz dotazovat.
		/// </summary>
		public ObjectInfo ObjectInfo
		{
			get { return objectInfo; }
			set { objectInfo = value; }
		}
		private ObjectInfo objectInfo;

		/// <summary>
		/// Maximální počet záznamů, který se vrací z databáze - (SELECT TOP n ...).
		/// </summary>
		public int? TopRecords
		{
			get { return topRecords; }
			set { topRecords = value; }
		}
		private int? topRecords;

		/// <summary>
		/// Udává, zda se mají vracet i záznamy označené za smazané.
		/// Výchozí hodnota je false, smazané záznamy se nevrací.
		/// </summary>
		public bool IncludeDeleted
		{
			get { return includeDeleted; }
			set { includeDeleted = value; }
		}
		private bool includeDeleted = false;

		/// <summary>
		/// Seznam sloupců, které jsou výsledkem dotazu (SELECT sloupec1, sloupec2...).
		/// </summary>
		public PropertyInfoCollection Properties
		{
			get
			{
				Contract.Ensures(Contract.Result<PropertyInfoCollection>() != null);
				return properties;
			}
		}
		private PropertyInfoCollection properties = new PropertyInfoCollection();

		/// <summary>
		/// Podmínky dotazu (WHERE ...).
		/// </summary>
		public ConditionList Conditions
		{
			get
			{
				Contract.Ensures(Contract.Result<ConditionList>() != null);
				return conditions.Conditions;
			}
		}
		private AndCondition conditions = new AndCondition();

		/// <summary>
		/// Pořadí záznamů (ORDER BY ...).
		/// </summary>
		public OrderByCollection OrderBy
		{
			get
			{
				Contract.Ensures(Contract.Result<OrderByCollection>() != null);
				return orderBy;
			}
		}
		private OrderByCollection orderBy = new OrderByCollection();
		#endregion

		#region GetDataLoadPower
		/// <summary>
		/// Podle kolekce properties určí režim záznamů, které budou vráceny.
		/// Pro prázdnou kolekci vrací FullLoad, pro kolekci o jednom prvku, který je primárním klíčem, vrací Ghost. Jinak vrací PartialLoad.
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
		/// Vytvoří dotaz, nastaví jej do commandu.
		/// Přidá parametry.
		/// </summary>
		public void PrepareCommand(DbCommand command)
		{
			Contract.Requires<ArgumentNullException>(command != null, "command");

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
		/// Slouží k přípravě objektu před začátkem skládání databázového dotazu.
		/// </summary>
		public virtual void OnBeforePrepareCommand()
		{
		}

		/// <summary>
		/// Slouží k dokončení skládání databázového dotazu.
		/// Voláno po poskládání databázového dotazu, naskládání parametrů do commandu,
		/// ale PŘED nastavením property command.CommandText. Je tak možno databázový
		/// dotaz upravit na poslední chvíli.
		/// </summary>
		public virtual void OnAfterPrepareCommand(DbCommand command, StringBuilder commandBuilder)
		{
		}
		#endregion

		#region SQL Statement builder
		/// <summary>
		/// Vrátí sekci SQL dotazu SELECT.
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
		/// Vrátí seznam sloupců, které se z databáze získávají.
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
				{
					fieldsBuilder.Append(", ");
				}

				if (queryProperties[i] is IFieldsBuilder)
				{
					fieldsBuilder.Append(((IFieldsBuilder)queryProperties[i]).GetSelectFieldStatement(command));
				}
			}
			return fieldsBuilder.ToString();
		}

		/// <summary>
		/// Vrátí sekci SQL dotazu FROM.
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
		/// Vrátí sekci SQL dotazu WHERE.
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
		/// Vrátí sekci SQL dotazu ORDER BY.
		/// </summary>
		protected virtual string GetOrderByStatement(DbCommand command)
		{
			if (orderBy.Count == 0)
			{
				return String.Empty;
			}

			StringBuilder orderByBuilder = new StringBuilder();
			orderByBuilder.Append("ORDER BY ");
			for (int i = 0; i < orderBy.Count; i++)
			{
				if (i > 0)
				{
					orderByBuilder.Append(", ");
				}

				orderByBuilder.Append(orderBy[i].Expression);
				if (orderBy[i].Direction == Havit.Collections.SortDirection.Descending)
				{
					orderByBuilder.Append(" DESC");
				}
			}
			return orderByBuilder.ToString();
		}
		#endregion

		/// <summary>
		/// Vrátí sekci SQL dotazu OPTION - použito na OPTION (RECOMPILE).
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

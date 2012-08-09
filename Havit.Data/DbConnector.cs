using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Configuration;
using System.Data;

namespace Havit.Data
{
	/// <summary>
	/// T��da usnad�uj�c� pr�ci s datab�zemi. N�stupce <see cref="Havit.Data.SqlClient.SqlDataAccess"/>.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase")]
	public class DbConnector
	{
		#region ConnectionString
		/// <summary>
		/// Vr�t� connection-string, kter� spolu s <see cref="DbConnector.ProviderFactory"/> ur�uje parametry DbConnectoru.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}
		private string _connectionString;
		#endregion

		#region ProviderFactory
		/// <summary>
		/// Vr�t� <see cref="DbProviderFactory"/>, kter� spolu s <see cref="DbConnector.ConnectionString"/>em ur�uje parametry DbConnectoru.
		/// </summary>
		public DbProviderFactory ProviderFactory
		{
			get
			{
				return _providerFactory;
			}
		}
		private DbProviderFactory _providerFactory;
		#endregion

		#region Constructors
		/// <summary>
		/// Inicializuje instanci t��dy <see cref="DbConnector"/>.
		/// </summary>
		/// <param name="connectionString">connection-string</param>
		/// <param name="providerFactory">DbProviderFactory</param>
		public DbConnector(string connectionString, DbProviderFactory providerFactory)
		{
			if (String.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException("Parametr nesm� b�t null ani String.Empty.", "connectionString");
			}
			if (providerFactory == null)
			{
				throw new ArgumentNullException("providerFactory");
			}

			this._connectionString = connectionString;
			this._providerFactory = providerFactory;
		}

		/// <summary>
		/// Inicializuje instanci t��dy <see cref="DbConnector"/>.
		/// </summary>
		/// <param name="connectionString">Connection-string</param>
		/// <param name="providerInvariantName">Invariant name of a provider.</param>
		public DbConnector(string connectionString, string providerInvariantName)
		{
			if (String.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException("Parametr nesm� b�t null ani String.Empty.", "connectionString");
			}
			if (String.IsNullOrEmpty(providerInvariantName))
			{
				throw new ArgumentException("Parametr nesm� b�t null ani String.Empty.", "providerInvariantName");
			}

			this._connectionString = connectionString;
			this._providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
		}

		/// <summary>
		/// Inicializuje instanci t��dy <see cref="DbConnector"/>.
		/// </summary>
		/// <param name="connectionStringSettings">Nastaven� <see cref="ConnectionStringSettings"/> na�ten� z .config souboru. Nap�. z�skan� p�es ConfigurationManager.ConnectionStrings["ConnectionStringName"].</param>
		public DbConnector(ConnectionStringSettings connectionStringSettings)
		{
			if (connectionStringSettings == null)
			{
				throw new ArgumentNullException("connectionStringSettings");
			}
			if (String.IsNullOrEmpty(connectionStringSettings.ConnectionString))
			{
				throw new ArgumentException("Argument nem� nastavenu vlastnost ConnectionString.", "connectionStringSettings");
			}

			this._connectionString = connectionStringSettings.ConnectionString;
			if (String.IsNullOrEmpty(connectionStringSettings.ProviderName))
			{
				this._providerFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
			}
			else
			{
				this._providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
			}
		}
		#endregion

		/**********************************************************************************************************/

		#region private CreateCommand, SetCommandDefaults
		/// <summary>
		/// Vytvo�� DbCommand dle zadan�ch parametr�. Nenastavuje spojen� ani jin� vlastnosti.
		/// </summary>
		/// <param name="commandText">SQL text p��kazu</param>
		/// <param name="commandType">typ p��kazu <see cref="CommandType"/></param>
		private DbCommand CreateCommand(string commandText, CommandType commandType)
		{
			if (String.IsNullOrEmpty(commandText))
			{
				throw new ArgumentException("commandText", "Argument nesm� b�t null ani String.Empty.");
			}

			// CommandType nem��e b�t null a nen� pot�eba ho kontrolovat

			DbCommand cmd = this.ProviderFactory.CreateCommand();
			cmd.CommandText = commandText;
			cmd.CommandType = commandType;

			return cmd;
		}

		/// <summary>
		/// Nastav� p��kazu default parametry (zat�m pouze Connection), nejsou-li nastaveny.
		/// </summary>
		/// <remarks>
		/// Pokud jsme v transakci, pak zde sjednot�me Connection (nech�pu, pro� to ned�l� s�m .NET Framework).
		/// </remarks>
		/// <param name="command"><see cref="DbCommand"/> k nastaven�</param>
		private void SetCommandDefaults(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			if (command.Transaction != null)
			{
				command.Connection = command.Transaction.Connection;
			}

			if (command.Connection == null)
			{
				command.Connection = GetConnection();
			}
		}
		#endregion

		#region GetConnection
		/// <summary>
		/// Vr�t� novou instanci provider-specific potomka <see cref="DbConnection"/> a pokud to po�adujeme, tak ji rovnou otev�e.
		/// </summary>
		/// <param name="openConnection"><c>true</c>, m�-li se nov� SqlConnection rovnou otev��t</param>
		public DbConnection GetConnection(bool openConnection)
		{
			DbConnection conn = this.ProviderFactory.CreateConnection();
			conn.ConnectionString = this.ConnectionString;
			if (openConnection)
			{
				conn.Open();
			}
			return conn;
		}


		/// <summary>
		/// Vr�t� novou instanci provider-specific potomka <see cref="DbConnection"/>.
		/// Connection nen� otev�ena.
		/// </summary>
		public DbConnection GetConnection()
		{
			return GetConnection(false);
		}
		#endregion

		#region ExecuteNonQuery
		/// <summary>
		/// Vykon� <see cref="DbCommand"/> a vr�t� po�et dot�en�ch ��dek.
		/// Nejobecn�j�� metoda, kterou pou��vaj� ostatn� overloady.
		/// </summary>
		/// <remarks>
		/// Nen�-li Connection p��kazu nastaveno, pou�ije imlicitn�.
		/// Nen�-li Connection dosud otev�eno, otev�e ho, vykon� p��kaz a zav�e.
		/// Nem�-li po�et dot�en�ch ��dek smysl, vrac� -1.
		/// </remarks>
		/// <param name="command"><see cref="DbCommand"/>, kter� m� b�t vykon�n</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public int ExecuteNonQuery(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			SetCommandDefaults(command);

			bool mustCloseConnection = false;
			if (command.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				command.Connection.Open();
			}

			int result;
			try
			{
				result = command.ExecuteNonQuery();
			}
			catch
			{
				command.Connection.Close();
				throw;
			}

			if (mustCloseConnection)
			{
				command.Connection.Close();
			}

			return result;
		}


		/// <summary>
		/// Vykon� zadan� p��kaz ur�en�ho typu bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// </summary>
		/// <param name="commandText">SQL p��kaz</param>
		/// <param name="commandType"><see cref="CommandType"/> p��kazu</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public int ExecuteNonQuery(string commandText, CommandType commandType)
		{
			return ExecuteNonQuery(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vykon� zadan� p��kaz bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// </summary>
		/// <remarks>
		/// Jako <see cref="CommandType"/> pou��v� <see cref="CommandType.Text"/>.
		/// </remarks>
		/// <param name="commandText">SQL p��kaz</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public int ExecuteNonQuery(string commandText)
		{
			return ExecuteNonQuery(commandText, CommandType.Text);
		}
		#endregion

		#region ExecuteDataSet
		/// <summary>
		/// Vykon� <see cref="DbCommand"/> a vr�t� resultset ve form� <see cref="DataSet"/>u.
		/// </summary>
		/// <remarks>
		/// Je-li cmd.Connection otev�eno, nech� ho otev�en�. Nen�-li, otev�e si ho a zase zav�e.
		/// </remarks>
		/// <param name="command">DbCommand k vykon�n�</param>
		/// <returns>resultset p��kazu ve form� <see cref="DataSet"/>u</returns>
		public DataSet ExecuteDataSet(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			SetCommandDefaults(command);

			using (DbDataAdapter adapter = this.ProviderFactory.CreateDataAdapter())
			{
				adapter.SelectCommand = command;

				DataSet ds = new DataSet();

				adapter.Fill(ds);

				return ds;
			}
		}


		/// <summary>
		/// Vykon� p��kaz commandText dan�ho commandType a vr�t� resultset ve form� <see cref="DataSet"/>u.
		/// </summary>
		/// <param name="commandText">SQL p��kaz</param>
		/// <param name="commandType">typ p��kazu</param>
		/// <returns>resultset p��kazu ve form� <see cref="DataSet"/>u</returns>
		public DataSet ExecuteDataSet(string commandText, CommandType commandType)
		{
			return ExecuteDataSet(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vykon� SQL p��kaz cmdText typu <see cref="CommandType.Text"/> a vr�t� resultset ve form� <see cref="DataSet"/>u.
		/// </summary>
		/// <param name="commandText">textov� SQL p��kaz</param>
		/// <returns>resultset p��kazu ve form� <see cref="DataSet"/>u</returns>
		public DataSet ExecuteDataSet(string commandText)
		{
			return ExecuteDataSet(commandText, CommandType.Text);
		}

		#endregion

		#region ExecuteDataTable
		/// <summary>
		/// Vykon� <see cref="DbCommand"/> a vr�t� prvn� resultset ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <remarks>
		/// Je-li cmd.Connection otev�eno, nech� ho otev�en�. Nen�-li, otev�e si ho a zase zav�e.
		/// </remarks>
		/// <param name="command"><see cref="DbCommand"/> k vykon�n�</param>
		/// <returns>prvn� resultset p��kazu ve form� <see cref="System.Data.DataTable"/></returns>
		public DataTable ExecuteDataTable(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			SetCommandDefaults(command);

			using (DbDataAdapter adapter = this.ProviderFactory.CreateDataAdapter())
			{
				adapter.SelectCommand = command;

				DataTable dt = new DataTable();

				adapter.Fill(dt);

				return dt;
			}
		}


		/// <summary>
		/// Vykon� p��kaz commandText typu commandType a vr�t� prvn� tabulku resultsetu ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="commandText">SQL p��kaz</param>
		/// <param name="commandType">typ p��kazu</param>
		/// <returns>prvn� tabulka resultsetu vykonan�ho p��kazu ve form� <see cref="System.Data.DataTable"/></returns>
		public DataTable ExecuteDataTable(string commandText, CommandType commandType)
		{
			return ExecuteDataTable(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vykon� p��kaz commandText typu <see cref="CommandType.Text"/> a vr�t� prvn� tabulku resultsetu ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="commandText">textov� SQL p��kaz</param>
		/// <returns>prvn� tabulka resultsetu vykonan�ho p��kazu ve form� <see cref="System.Data.DataTable"/></returns>
		public DataTable ExecuteDataTable(string commandText)
		{
			return ExecuteDataTable(commandText, CommandType.Text);
		}
		#endregion

		#region ExecuteReader
		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> pomoc� <see cref="CommandBehavior"/> a vr�t� v�sledn� resultset ve form� <see cref="DbDataReader"/>u.
		/// </summary>
		/// <param name="command">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <param name="behavior">po�adovan� "chov�n�"</param>
		/// <returns>resultset vykonan�ho p��kazu jako <see cref="DbDataReader"/></returns>
		public DbDataReader ExecuteReader(DbCommand command, CommandBehavior behavior)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			SetCommandDefaults(command);

			bool mustCloseConnection = false;
			if (command.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				command.Connection.Open();
			}

			DbDataReader reader;

			try
			{
				if (mustCloseConnection)
				{
					// otev�eme-li si spojen� sami, postar�me se i o jeho zav�en�
					reader = command.ExecuteReader(behavior | CommandBehavior.CloseConnection);
				}
				else
				{
					// spojen� bylo ji� otev�eno, tak ho tak nech�me, a� se star� nad�azen� aplikace
					reader = command.ExecuteReader(behavior);
				}
			}
			catch
			{
				if (mustCloseConnection)
				{
					command.Connection.Close();
				}
				throw;
			}

			return reader;
		}


		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> a vr�t� v�sledn� resultset ve form� <see cref="DbDataReader"/>u.
		/// </summary>
		/// <param name="command">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>resultset vykonan�ho p��kazu jako <see cref="DbDataReader"/></returns>
		public DbDataReader ExecuteReader(DbCommand command)
		{
			return ExecuteReader(command, CommandBehavior.Default);
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� <see cref="DbCommand"/> dle zadan�ch parametr� a vr�t� v�sledn� resultset ve form� <see cref="DbDataReader"/>u.
		/// </summary>
		/// <param name="commandText">text SQL p��kazu</param>
		/// <param name="commandType">typ p��kazu <see cref="CommandType"/></param>
		/// <returns>resultset ve form� <see cref="DbDataReader"/>u</returns>
		public DbDataReader ExecuteReader(string commandText, CommandType commandType)
		{
			return ExecuteReader(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� <see cref="DbCommand"/> dle zadan�ho SQL p��kazu typu <see cref="CommandType.Text"/>
		/// a vr�t� v�sledn� resultset ve form� <see cref="DbDataReader"/>u.
		/// </summary>
		/// <param name="commandText">text SQL p��kazu</param>
		/// <returns>resultset ve form� <see cref="DbDataReader"/>u</returns>
		public DbDataReader ExecuteReader(string commandText)
		{
			return ExecuteReader(commandText, CommandType.Text);
		}
		#endregion

		#region ExecuteDataRecord
		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> pomoc� <see cref="CommandBehavior"/> a vr�t� prvn� ��dek prvn� tabulky resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� <c>null</c>.
		/// </summary>
		/// <param name="command">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <param name="behavior">po�adovan� "chov�n�"</param>
		/// <param name="dataLoadPower"><see cref="DataLoadPower"/>, kter� se m� pou��t pro <see cref="DataRecord"/></param>
		/// <returns>prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public DataRecord ExecuteDataRecord(DbCommand command, CommandBehavior behavior, DataLoadPower dataLoadPower)
		{
			using (DbDataReader reader = ExecuteReader(command, behavior))
			{
				if (reader.Read())
				{
					return new DataRecord(reader, dataLoadPower);
				}
				return null;
			}
		}

		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> pomoc� <see cref="CommandBehavior"/> a vr�t� prvn� ��dek prvn� tabulky resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� <c>null</c>.
		/// </summary>
		/// <remarks>
		/// <see cref="DataLoadPower"/> v�sledn�ho <see cref="DataRecord"/>u nastav� na <see cref="DataLoadPower.FullLoad"/>.
		/// </remarks>
		/// <param name="command">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <param name="behavior">po�adovan� "chov�n�"</param>
		/// <returns>prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public DataRecord ExecuteDataRecord(DbCommand command, CommandBehavior behavior)
		{
			using (DbDataReader reader = ExecuteReader(command, behavior))
			{
				if (reader.Read())
				{
					return new DataRecord(reader, DataLoadPower.FullLoad);
				}
				return null;
			}
		}


		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> a vr�t� prvn� ��dek prvn� tabulky resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� <c>null</c>.
		/// </summary>
		/// <remarks>
		/// <see cref="DataLoadPower"/> v�sledn�ho <see cref="DataRecord"/>u nastav� na <see cref="DataLoadPower.FullLoad"/>.
		/// </remarks>
		/// <param command="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public DataRecord ExecuteDataRecord(DbCommand command)
		{
			return ExecuteDataRecord(command, CommandBehavior.Default);
		}


		/// <summary>
		/// Vytvo�� <see cref="DbCommand"/> dle zadan�ch parametr�, donasatav� ho a vr�t� prvn� ��dek prvn� tabulky resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� <c>null</c>.
		/// </summary>
		/// <remarks>
		/// <see cref="DataLoadPower"/> v�sledn�ho <see cref="DataRecord"/>u nastav� na <see cref="DataLoadPower.FullLoad"/>.
		/// </remarks>
		/// <param name="commandText">text SQL p��kazu</param>
		/// <param name="commandType"><see cref="CommandType"/> SQL p��kazu</param>
		/// <returns>prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public DataRecord ExecuteDataRecord(string commandText, CommandType commandType)
		{
			return ExecuteDataRecord(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� <see cref="DbCommand"/> dle zadan�ho SQL p��kazu
		/// a vr�t� prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� <c>null</c>.
		/// </summary>
		/// <remarks>
		/// <see cref="DataLoadPower"/> v�sledn�ho <see cref="DataRecord"/>u nastav� na <see cref="DataLoadPower.FullLoad"/>.
		/// </remarks>
		/// <param name="commandText">text SQL p��kazu</param>
		/// <returns>prvn� ��dek prvn� tabulky resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public DataRecord ExecuteDataRecord(string commandText)
		{
			return ExecuteDataRecord(commandText, CommandType.Text);
		}
		#endregion

		#region ExecuteScalar

		/// <summary>
		/// Donastav� a vykon� <see cref="DbCommand"/> a vr�t� prvn� sloupec prvn�ho ��dku prvn� tabulky jeho resultsetu.
		/// </summary>
		/// <example>
		/// int result = (int)ExecuteScalar(cmd);
		/// </example>
		/// <param name="command">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>prvn� sloupec prvn�ho ��dku prvn� tabulky resultsetu</returns>
		public object ExecuteScalar(DbCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			SetCommandDefaults(command);

			bool mustCloseConnection = false;
			if (command.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				command.Connection.Open();
			}

			object result;

			try
			{
				result = command.ExecuteScalar();
			}
			catch
			{
				command.Connection.Close();
				throw;
			}

			if (mustCloseConnection)
			{
				command.Connection.Close();
			}

			return result;
		}


		/// <summary>
		/// Vytvo�� ze zadan�ch parametr� <see cref="DbCommand"/>, nastav�, vykon� ho a vr�t� prvn� sloupec
		/// prvn�ho ��dku prvn� tabulky jeho resultsetu.
		/// </summary>
		/// <param name="commandText">text SQL p��kazu</param>
		/// <param name="commandType">typ p��kazu</param>
		/// <returns>prvn� sloupec prvn�ho ��dku prvn� tabulky resultsetu</returns>
		public object ExecuteScalar(string commandText, CommandType commandType)
		{
			return ExecuteScalar(CreateCommand(commandText, commandType));
		}


		/// <summary>
		/// Vytvo�� <see cref="DbCommand"/> typu <see cref="CommandType.Text"/>, vykon� ho a vr�t�
		/// prvn� sloupec prvn�ho ��dku prvn� tabulky jeho resultsetu.
		/// </summary>
		/// <param name="commandText">text SQL p��kazu typu <see cref="CommandType.Text"/></param>
		/// <returns>prvn� sloupec prvn�ho ��dku resultsetu</returns>
		public object ExecuteScalar(string commandText)
		{
			return ExecuteScalar(CreateCommand(commandText, CommandType.Text));
		}

		#endregion

		#region ExecuteTransaction
		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Pokud je outerTransaction <c>null</c>, je spu�t�na a commitov�na nov�.
		/// Pokud je outerTransaction zad�no, jsou po�adovan� kroky v r�mci n� pouze vykon�ny, pokud se shoduje IsolationLevel.
		/// Pokud se IsolationLevel neshoduje, zalo�� se nov� nested-transakce s po�adovan�m IsolationLevelem.
		/// </summary>
		/// <param name="transactionWork"><see cref="DbTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		/// <param name="outerTransaction">vn�j�� transakce, pokud existuje; jinak <c>null</c></param>
		/// <param name="isolationLevel">po�adovan� <see cref="IsolationLevel"/> transakce; pokud je <see cref="IsolationLevel.Unspecified"/>, pou�ije se outerTransaction, pokud je definov�na, nebo zalo�� nov� transakce s defaultn�m isolation-levelem</param>
		public void ExecuteTransaction(DbTransactionDelegate transactionWork, DbTransaction outerTransaction, IsolationLevel isolationLevel)
		{
			if (transactionWork == null)
			{
				throw new ArgumentNullException("transactionWork");
			}

			DbTransaction currentTransaction = outerTransaction;
			DbConnection connection;

			bool mustCloseConnection = false;
			bool mustCommitOrRollbackTransaction = false;

			if (outerTransaction == null)
			{
				// otev�en� spojen�, pokud nen� ji� otev�eno
				connection = this.GetConnection();
				connection.Open();
				mustCloseConnection = true;
			}
			else
			{
				connection = outerTransaction.Connection;
			}

			if ((outerTransaction == null) || 
				((isolationLevel != IsolationLevel.Unspecified) && (outerTransaction.IsolationLevel != isolationLevel)))
			{
				if (isolationLevel == IsolationLevel.Unspecified)
				{
					currentTransaction = connection.BeginTransaction();
				}
				else
				{
					currentTransaction = connection.BeginTransaction(IsolationLevel.Unspecified);
				}
				mustCommitOrRollbackTransaction = true;
			}

			try
			{
				transactionWork(currentTransaction);

				if (mustCommitOrRollbackTransaction)
				{
					// commit chceme jen v p��pad�, �e jsme sami transakci zalo�ili (a� u� �pln� novou, nebo nested)
					currentTransaction.Commit();
				}
			}
			catch
			{
				try
				{
					if (mustCommitOrRollbackTransaction)
					{
						// rollback chceme taky jen v p��pad�, �e jsme sami transakci zalo�ili (a� u� �pln� novou, nebo nested)
						// p��padn� vn�j�� transakce shod� na�e v�jimka
						currentTransaction.Rollback();
					}
				}
				catch
				{
					// NOOP: chceme vyhodit v�cnou v�jimku, ne probl�m s rollbackem
				}
				throw;
			}
			finally
			{
				if (mustCloseConnection)
				{
					// uzav�en� spojen�, pokud jsme inici�tory transakce
					connection.Close();
				}
			}
		}

		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Pokud je outerTransaction <c>null</c>, je spu�t�na a commitov�na nov�.
		/// Pokud je outerTransaction zad�no, jsou po�adovan� kroky v r�mci n� pouze vykon�ny.
		/// </summary>
		/// <remarks>
		/// Pokud je outerTransaction <c>null</c>, pak zalo�� novou transakci s dan�m IsolationLevelem.
		/// Pokud je outerTransakce zad�na, pak se pro zadan� transactionWork pou�ije zadan� IsolationLevel a pak ho vr�t� na p�vodn� hodnotu.
		/// </remarks>
		/// <param name="transactionWork"><see cref="DbTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		/// <param name="outerTransaction">vn�j�� transakce, pokud existuje; jinak <c>null</c></param>
		public void ExecuteTransaction(DbTransactionDelegate transactionWork, DbTransaction outerTransaction)
		{
			ExecuteTransaction(transactionWork, outerTransaction, IsolationLevel.Unspecified);
		}

		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce s dan�m isolation-levelem.
		/// </summary>
		/// <param name="transactionWork"><see cref="DbTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		/// <param name="isolationLevel">po�adovan� <see cref="IsolationLevel"/> transakce</param>
		public void ExecuteTransaction(DbTransactionDelegate transactionWork, IsolationLevel isolationLevel)
		{
			ExecuteTransaction(transactionWork, null, isolationLevel);
		}


		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Je spu�t�na a commitov�na nov� samostatn� transakce.
		/// </summary>
		/// <param name="transactionWork"><see cref="DbTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		public void ExecuteTransaction(DbTransactionDelegate transactionWork)
		{
			ExecuteTransaction(transactionWork, null, IsolationLevel.Unspecified);
		}
		#endregion

		/**********************************************************************************************************/

		#region Default (static)
		/// <summary>
		/// Defaultn� <see cref="DbConnector"/>. Pokud nen� nastaven ru�n�, pak se vytvo�� p�i prvn�m p��stupu z defaultn�ho connection-stringu na�ten�ho z .config souboru.
		/// Nastaven�m na null mohu pro p��t� p��stup vynutit op�tovnou inicializaci z .config souboru.
		/// </summary>
		/// <example>
		/// Bu� <b>DefaultConnectionString</b> z konfigura�n� sekce &lt;connectionStrings&gt;:
		/// <code>
		/// &lt;configuration&gt;
		///		&lt;connectionStrings&gt;
		///			&lt;add name="DefaultConnectionString"
		///				connectionString="Server=localhost;Database=pubs;UID=user;PWD=heslo;"
		///				providerName="System.Data.SqlClient"
		///			/&gt;
		///		&lt;/connectionStrings&gt;
		///	&lt;/configuration&gt;
		/// </code>
		/// a nebo po staru <b>ConnectionString</b> z konfigura�n� sekce &lt;appStrings&gt;:
		/// <code>
		/// &lt;configuration&gt;
		///		&lt;appSettings&gt;
		///			&lt;add key="ConnectionString" value="Server=localhost;Database=pubs;UID=user;PWD=heslo;" /&gt;
		///		&lt;/appSettings&gt;
		///	&lt;/configuration&gt;
		/// </code>
		/// </example>
		public static DbConnector Default
		{
			get
			{
				if (_default == null)
				{
					_default = GetDbConnectorFromDefaultConfig();
				}
				return _default;
			}
			set
			{
				_default = value;
			}
		}
		private static DbConnector _default;

		/// <summary>
		/// Vr�t� <see cref="DbConnection"/> inicializovan� defaulty z .config souboru.
		/// </summary>
		/// <remarks>Viz vlastnost <see cref="DbConnector.Default"/>.</remarks>
		private static DbConnector GetDbConnectorFromDefaultConfig()
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["DefaultConnectionString"];
			if (connectionStringSettings != null)
			{
				return new DbConnector(connectionStringSettings);
			}
			else
			{
				string appSettingsConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
				if (String.IsNullOrEmpty(appSettingsConnectionString))
				{
					throw new InvalidOperationException("Z konfigura�n�ho souboru se nepoda�ilo na��st defaultn� parametry p�ipojen� k datab�zi.");
				}
				return new DbConnector(appSettingsConnectionString, "System.Data.SqlClient");
			}
		}
		#endregion

	}
}


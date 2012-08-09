using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Havit.Data.SqlClient
{
	/// <summary>
	/// T��da SqlDataAccess obsahuje metody usnad�uj�c� pr�ci se z�kladn� t��dou
	/// <b>System.Data.</b><see cref="System.Data.SqlClient"/>.
	/// </summary>
	/// <remarks>
	/// Inspirov�no <a href="http://msdn.microsoft.com/vcsharp/downloads/components/default.aspx?pull=/library/en-us/dnbda/html/daab-rm.asp">
	/// Microsoft Data Access Application Block verze 2.0</a>.
	/// </remarks>
	public static class SqlDataAccess
	{
		#region private CreateCommand, SetCommandDefaults
		/// <summary>
		/// Vytvo�� pr�zdn� SqlCommand dle zadan�ch parametr�. Nenastavuje spojen� ani jin� vlastnosti.
		/// </summary>
		/// <param name="cmdText">SQL text p��kazu</param>
		/// <param name="cmdType">typ p��kazu</param>
		/// <returns></returns>
		private static SqlCommand CreateCommand(string cmdText, CommandType cmdType)
		{
			if ((cmdText == null) || (cmdText.Length == 0))
			{
				throw new ArgumentNullException("cmdText");
			}

			// CommandType nem��e b�t null a nen� pot�eba ho kontrolovat

			SqlCommand cmd = new SqlCommand(cmdText);
			cmd.CommandType = cmdType;

			return cmd;
		}

		/// <summary>
		/// Nastav� p��kazu default parametry (zat�m pouze Connection), nejsou-li nastaveny.
		/// </summary>
		/// <remarks>
		/// Pokud jsme v transakci, pak sjednot�me Connection (nech�pu, pro� to ned�l� s�m .NET Framework).
		/// </remarks>
		/// <param name="cmd">SqlCommand k nastaven�</param>
		private static void SetCommandDefaults(SqlCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			if (cmd.Transaction != null)
			{
				cmd.Connection = cmd.Transaction.Connection;
			}
			
			if (cmd.Connection == null)
			{
				cmd.Connection = GetConnection();
			}
		}
		#endregion

		#region ConnectionString, ConfigConnectionString
		/// <summary>
		/// Implicitn� ConnectionString.
		/// Pou�ije se v�dy, nen�-li metod� jakkoliv explicitn� p�ed�n jin�.
		/// Nen�-li nastaven klientskou aplikac�, na�te se z .config souboru.
		/// </summary>
		/// <remarks>
		/// Pokus o na�ten� z .config souboru je proveden v�dy, je-li pr�v� nastaven
		/// na <c>null</c> nebo <c>String.Empty</c>. Jeho nastaven�m na <c>null</c> lze tedy
		/// poru�it, aby se ConnectionString na�etl z .config souboru.
		/// </remarks>
		public static string ConnectionString
		{
			get
			{
				if ((connectionString == null) || (connectionString.Length == 0))
				{
					connectionString = ConfigConnectionString;
				}
				return connectionString;
			}
			set
			{
				connectionString = value;
			}
		}
		private static string connectionString;


		/// <summary>
		/// ConnectionString z .config souboru (web.config nebo applikace.exe.config).
		/// </summary>
		/// <remarks>
		/// Pro .NET Framework 2.0 roz���eno o pou�it� hodnoty <b>DefaultConnectionString</b>
		/// z konfigura�n� sekce &lt;connectionStrings&gt;.
		/// </remarks>
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
		public static string ConfigConnectionString
		{
			get
			{
				ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["DefaultConnectionString"]; 
				if (css != null)
				{
					return css.ConnectionString;
				}
				else
				{
					string result = ConfigurationManager.AppSettings["ConnectionString"];
					if (result == null)
					{
						throw new ConfigurationErrorsException("V konfigura�n�m souboru nen� nastaven v�choz� ConnectionString");
					}
					return result;
				}
			}
		}
		#endregion

		#region DefaultCommandType

		/// <summary>
		/// CommandType, kter� se pou�ije pro intern� vytv��en� p��kazy,
		/// je-li jako parametr metody zad�n pouze textov� SQL p��kaz.
		/// </summary>
		/// <remarks>
		/// V�choz� hodnota nastavena na <c>CommandType.StoredProcedure</c>.
		/// </remarks>
		public static CommandType DefaultCommandType
		{
			get
			{
				return defaultCommandType;
			}
			set
			{
				defaultCommandType = value;
			}
		}
		private static CommandType defaultCommandType = CommandType.StoredProcedure;

		#endregion

		#region GetConnection
		/// <summary>
		/// Vytvo�� novou instanci SqlConnection podle po�adovan�ho connectionStringu
		/// a p��padn� ji rovnou otev�e.
		/// </summary>
		/// <param name="connectionString">ConnectionString</param>
		/// <param name="open"><c>true</c>, m�-li se nov� SqlConnection rovnou otev��t</param>
		/// <returns>nov� instance SqlConnection</returns>
		public static SqlConnection GetConnection(string connectionString, bool open)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			if (open)
			{
				conn.Open();
			}
			return conn;
		}


		/// <summary>
		/// Vytvo�� novou instanci SqlConnection podle z implicitn�ho <see cref="ConnectionString"/>
		/// a p��padn� ji rovnou otev�e.
		/// </summary>
		/// <param name="open"><c>true</c>, m�-li se nov� SqlConnection rovnou otev��t</param>
		/// <returns>nov� instance SqlConnection</returns>
		public static SqlConnection GetConnection(bool open)
		{
			return GetConnection(ConnectionString, open);
		}


		/// <summary>
		/// Vytvo�� novou instanci SqlConnection podle implicitn�ho <see cref="ConnectionString"/>.
		/// SqlConnection nen� otev�ena.
		/// </summary>
		/// <returns>nov� instance SqlConnection (nen� otev�ena)</returns>
		public static SqlConnection GetConnection()
		{
			return GetConnection(ConnectionString, false);
		}
		#endregion

		#region ExecuteNonQuery

		/// <summary>
		/// Vykon� SqlCommand a vr�t� po�et dot�en�ch ��dek.
		/// Nejobecn�j�� metoda, kterou pou��vaj� ostatn� overloady.
		/// </summary>
		/// <remarks>
		/// Nen�-li SqlConnection p��kazu nastaveno, pou�ije imlicitn�.
		/// Nen�-li SqlConnection dosud otev�eno, otev�e ho, vykon� p��kaz a zav�e.
		/// Nem�-li po�et dot�en�ch ��dek smysl, vrac� -1.
		/// </remarks>
		/// <param name="cmd">SqlCommand, kter� m� b�t vykon�n</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public static int ExecuteNonQuery(SqlCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			SetCommandDefaults(cmd);

			bool mustCloseConnection = false;
			if (cmd.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				cmd.Connection.Open();
			}

			int result;
			try
			{
				result = cmd.ExecuteNonQuery();
			}
			catch
			{
				cmd.Connection.Close();
				throw;
			}

			if (mustCloseConnection)
			{
				cmd.Connection.Close();
			}

			return result;
		}


		/// <summary>
		/// Vykon� zadan� p��kaz ur�en�ho typu bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <param name="cmdType">CommandType p��kazu</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public static int ExecuteNonQuery(string cmdText, CommandType cmdType)
		{
			return ExecuteNonQuery(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vykon� zadan� p��kaz bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// CommandType p��kazu je nastaven z property DefaultCommandType.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public static int ExecuteNonQuery(string cmdText)
		{
			return ExecuteNonQuery(cmdText, DefaultCommandType);
		}

		#endregion

		#region ExecuteDataSet

		/// <summary>
		/// Vykon� SqlCommand a vr�t� resultset ve form� DataSetu.
		/// </summary>
		/// <remarks>
		/// Je-li cmd.Connection otev�eno, nech� ho otev�en�. Nen�-li, otev�e si ho a zase zav�e.
		/// </remarks>
		/// <param name="cmd">SqlCommand k vykon�n�</param>
		/// <returns>resultset p��kazu ve form� DataSetu</returns>
		public static DataSet ExecuteDataSet(SqlCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			SetCommandDefaults(cmd);

			using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
			{
				DataSet ds = new DataSet();

				adapter.Fill(ds);

				return ds;
			}
		}


		/// <summary>
		/// Vykon� p��kaz cmdText dan�ho cmdType proti implicitn� connection
		/// a vr�t� resultset ve form� DataSetu.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <param name="cmdType">typ p��kazu</param>
		/// <returns>resultset p��kazu ve form� DataSetu</returns>
		public static DataSet ExecuteDataSet(string cmdText, CommandType cmdType)
		{
			return ExecuteDataSet(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vykon� SQL p��kaz cmdText s implicitn�ho typu (DefaultCommandType)
		/// proti implicitn� connection (GetConnection).
		/// </summary>
		/// <param name="cmdText">textov� SQL p��kaz</param>
		/// <returns>resultset ve form� DataSetu</returns>
		public static DataSet ExecuteDataSet(string cmdText)
		{
			return ExecuteDataSet(cmdText, DefaultCommandType);
		}

		#endregion

		#region ExecuteDataTable
		/// <summary>
		/// Vykon� SqlCommand a vr�t� prvn� resultset ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <remarks>
		/// Je-li cmd.Connection otev�eno, nech� ho otev�en�. Nen�-li, otev�e si ho a zase zav�e.
		/// </remarks>
		/// <param name="cmd">SqlCommand k vykon�n�</param>
		/// <returns>prvn� resultset p��kazu ve form� <see cref="System.Data.DataTable"/></returns>
		public static DataTable ExecuteDataTable(SqlCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			SetCommandDefaults(cmd);

			using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
			{
				DataTable dt = new DataTable();

				adapter.Fill(dt);

				return dt;
			}
		}


		/// <summary>
		/// Vykon� p��kaz cmdText dan�ho cmdType proti implicitn� connection
		/// a vr�t� prvn� resultset ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <param name="cmdType">typ p��kazu</param>
		/// <returns>prvn� resultset p��kazu ve form� <see cref="System.Data.DataTable"/></returns>
		public static DataTable ExecuteDataTable(string cmdText, CommandType cmdType)
		{
			return ExecuteDataTable(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vykon� SQL p��kaz cmdText s implicitn�ho typu (DefaultCommandType)
		/// proti implicitn� connection (GetConnection) a vr�t� prvn� resultset
		/// ve form� <see cref="System.Data.DataTable"/>.
		/// </summary>
		/// <param name="cmdText">textov� SQL p��kaz</param>
		/// <returns>prvn� resultset ve form� <see cref="System.Data.DataTable"/></returns>
		public static DataTable ExecuteDataTable(string cmdText)
		{
			return ExecuteDataTable(cmdText, DefaultCommandType);
		}
		#endregion

		#region ExecuteReader

		/// <summary>
		/// Donastav� a vykon� SqlCommand pomoc� CommandBehavior a vr�t� v�sledn� resultset
		/// ve form� SqlDataReaderu.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <param name="behavior">po�adovan� "chov�n�"</param>
		/// <returns>Resultset jako SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(SqlCommand cmd, CommandBehavior behavior)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			SetCommandDefaults(cmd);

			bool mustCloseConnection = false;
			if (cmd.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				cmd.Connection.Open();
			}

			SqlDataReader reader;

			try
			{
				if (mustCloseConnection)
				{
					// otev�eme-li si spojen� sami, postar�me se i o jeho zav�en�
					reader = cmd.ExecuteReader(behavior | CommandBehavior.CloseConnection);
				}
				else
				{
					// spojen� bylo ji� otev�eno, tak ho tak nech�me, a� se star� nad�azen� aplikace
					reader = cmd.ExecuteReader(behavior);
				}
			}
			catch
			{
				if (mustCloseConnection)
				{
					cmd.Connection.Close();
				}
				throw;
			}

			return reader;
		}


		/// <summary>
		/// Donastav� a vykon� SqlCommand a vr�t� v�sledn� resultset ve form� SqlDataReaderu.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns></returns>
		public static SqlDataReader ExecuteReader(SqlCommand cmd)
		{
			return ExecuteReader(cmd, CommandBehavior.Default);
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� SqlCommand dle zadan�ch parametr� a vr�t�
		/// v�sledn� resultset ve form� SqlDataReaderu.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <param name="cmdType">typ p��kazu</param>
		/// <returns>resultset ve form� SqlDataReaderu</returns>
		public static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType)
		{
			return ExecuteReader(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� SqlCommand dle zadan�ho SQL p��kazu
		/// a vr�t� v�sledn� resultset ve form� SqlDataReaderu.
		/// Jako typ p��kazu se pou�ije DefaultCommandType.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <returns>resultset ve form� SqlDataReaderu</returns>
		public static SqlDataReader ExecuteReader(string cmdText)
		{
			return ExecuteReader(cmdText, DefaultCommandType);
		}

		#endregion

		#region ExecuteDataRecord
		/// <summary>
		/// Donastav� a vykon� SqlCommand pomoc� CommandBehavior a vr�t� prvn� ��dek prvn�ho resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� null.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <param name="behavior">po�adovan� "chov�n�"</param>
		/// <returns>prvn� ��dek prvn�ho resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public static DataRecord ExecuteDataRecord(SqlCommand cmd, CommandBehavior behavior)
		{
			using (SqlDataReader reader = ExecuteReader(cmd, behavior))
			{
				if (reader.Read())
				{
					return new DataRecord(reader);
				}
				return null;
			}
		}

		/// <summary>
		/// Donastav� a vykon� SqlCommand a vr�t� prvn� ��dek prvn�ho resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� null.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>prvn� ��dek prvn�ho resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public static DataRecord ExecuteDataRecord(SqlCommand cmd)
		{
			return ExecuteDataRecord(cmd, CommandBehavior.Default);
		}


		/// <summary>
		/// Vytvo�� SqlCommand dle zadan�ch parametr�, donasatav� ho a vr�t� prvn� ��dek prvn�ho resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� null.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <param name="cmdType">typ SQL p��kazu</param>
		/// <returns>prvn� ��dek prvn�ho resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public static DataRecord ExecuteDataRecord(string cmdText, CommandType cmdType)
		{
			return ExecuteDataRecord(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vytvo��, nastav� a vykon� SqlCommand dle zadan�ho SQL p��kazu
		/// a vr�t� v�sledn� resultset ve form� <see cref="Havit.Data.DataRecord"/>.
		/// Jako typ p��kazu se pou�ije <see cref="DefaultCommandType"/>.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <returns>prvn� ��dek prvn�ho resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public static DataRecord ExecuteDataRecord(string cmdText)
		{
			return ExecuteDataRecord(cmdText, DefaultCommandType);
		}
		#endregion

		#region ExecuteScalar

		/// <summary>
		/// Donastav� a vykon� SqlCommand a vr�t� prvn� sloupec prvn�ho ��dku jeho resultsetu.
		/// </summary>
		/// <example>
		/// int result = (int)ExecuteScalar(cmd);
		/// </example>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>prvn� sloupec prvn�ho ��dku resultsetu</returns>
		public static object ExecuteScalar(SqlCommand cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}

			SetCommandDefaults(cmd);

			bool mustCloseConnection = false;
			if (cmd.Connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				cmd.Connection.Open();
			}

			object result;

			try
			{
				result = cmd.ExecuteScalar();
			}
			catch
			{
				cmd.Connection.Close();
				throw;
			}

			if (mustCloseConnection)
			{
				cmd.Connection.Close();
			}

			return result;
		}


		/// <summary>
		/// Vytvo�� ze zadan�ch parametr� SqlCommand, vykon� ho a vr�t� prvn� sloupec
		/// prvn�ho ��dku jeho resultsetu.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <param name="cmdType">typ p��kazu</param>
		/// <returns>prvn� sloupec prvn�ho ��dku resultsetu</returns>
		public static object ExecuteScalar(string cmdText, CommandType cmdType)
		{
			return ExecuteScalar(CreateCommand(cmdText, cmdType));
		}


		/// <summary>
		/// Vytvo�� SqlCommand, nastav� mu DefaultCommandType, vykon� ho a vr�t�
		/// prvn� sloupec prvn�ho ��dku jeho resultsetu.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <returns>prvn� sloupec prvn�ho ��dku resultsetu</returns>
		public static object ExecuteScalar(string cmdText)
		{
			return ExecuteScalar(CreateCommand(cmdText, DefaultCommandType));
		}

		#endregion

		#region ExecuteTransaction
		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Pokud je zadan� transakce <c>null</c>, je spu�t�na a commitov�na nov�.
		/// Pokud zadan� transakce nen� <c>null</c>, jsou zadan� kroky pouze vykon�ny.
		/// </summary>
		/// <param name="transaction">transakce</param>
		public static void ExecuteTransaction(SqlTransactionDelegate transactionWork, SqlTransaction transaction)
		{
			SqlTransaction currentTransaction = transaction;
			SqlConnection connection;
			if (transaction == null)
			{
				// otev�en� spojen�, pokud jsme inici�tory transakce
				connection = SqlDataAccess.GetConnection();
				connection.Open();
				currentTransaction = connection.BeginTransaction();
			}
			else
			{
				connection = currentTransaction.Connection;
			}

			try
			{
				transactionWork(currentTransaction);

				if (transaction == null)
				{
					// commit chceme jen v p��pad�, �e nejsme uvnit� vn�j�� transakce
					currentTransaction.Commit();
				}
			}
			catch
			{
				try
				{
					currentTransaction.Rollback();
				}
				catch
				{
					// chceme vyhodit vn�j�� v�jimku, ne probl�m s rollbackem
				}
				throw;
			}
			finally
			{
				// uzav�en� spojen�, pokud jsme inici�tory transakce
				if (transaction == null)
				{
					connection.Close();
				}
			}
		}

		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Je spu�t�na a commitov�na nov� samostatn� transakce.
		/// </summary>
		public static void ExecuteTransaction(SqlTransactionDelegate transactionWork)
		{
			ExecuteTransaction(transactionWork, null);
		}
		#endregion
	}
}

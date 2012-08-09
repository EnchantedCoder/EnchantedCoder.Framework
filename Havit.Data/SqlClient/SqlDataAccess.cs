using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

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
	[Obsolete("M�sto t��dy SqlDataAccess pou�ijte Havit.Data.DbConnector.")]
	public static class SqlDataAccess
	{
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
				return DbConnector.Default.ConnectionString;
			}
		}


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
			return (SqlConnection)DbConnector.Default.GetConnection(open);
		}


		/// <summary>
		/// Vytvo�� novou instanci SqlConnection podle implicitn�ho <see cref="ConnectionString"/>.
		/// SqlConnection nen� otev�ena.
		/// </summary>
		/// <returns>nov� instance SqlConnection (nen� otev�ena)</returns>
		public static SqlConnection GetConnection()
		{
			return (SqlConnection)DbConnector.Default.GetConnection();
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
			return DbConnector.Default.ExecuteNonQuery(cmd);
		}


		/// <summary>
		/// Vykon� zadan� p��kaz ur�en�ho typu bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <param name="cmdType">CommandType p��kazu</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public static int ExecuteNonQuery(string cmdText, CommandType cmdType)
		{
			return DbConnector.Default.ExecuteNonQuery(cmdText, cmdType);
		}


		/// <summary>
		/// Vykon� zadan� p��kaz bez parametr�. Vr�t� po�et dot�en�ch ��dek.
		/// CommandType p��kazu je nastaven z property DefaultCommandType.
		/// </summary>
		/// <param name="cmdText">SQL p��kaz</param>
		/// <returns>po�et dot�en�ch ��dek</returns>
		public static int ExecuteNonQuery(string cmdText)
		{
			return DbConnector.Default.ExecuteNonQuery(cmdText);
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
			return DbConnector.Default.ExecuteDataSet(cmd);
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
			return DbConnector.Default.ExecuteDataSet(cmdText, cmdType);
		}


		/// <summary>
		/// Vykon� SQL p��kaz cmdText s implicitn�ho typu (DefaultCommandType)
		/// proti implicitn� connection (GetConnection).
		/// </summary>
		/// <param name="cmdText">textov� SQL p��kaz</param>
		/// <returns>resultset ve form� DataSetu</returns>
		public static DataSet ExecuteDataSet(string cmdText)
		{
			return DbConnector.Default.ExecuteDataSet(cmdText);			
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
			return DbConnector.Default.ExecuteDataTable(cmd);
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
			return DbConnector.Default.ExecuteDataTable(cmdText, cmdType);
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
			return DbConnector.Default.ExecuteDataTable(cmdText);
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
			return (SqlDataReader)DbConnector.Default.ExecuteReader(cmd, behavior);
		}


		/// <summary>
		/// Donastav� a vykon� SqlCommand a vr�t� v�sledn� resultset ve form� SqlDataReaderu.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns></returns>
		public static SqlDataReader ExecuteReader(SqlCommand cmd)
		{
			return (SqlDataReader)DbConnector.Default.ExecuteReader(cmd);
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
			return (SqlDataReader)DbConnector.Default.ExecuteReader(cmdText, cmdType);
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
			return (SqlDataReader)DbConnector.Default.ExecuteReader(cmdText);
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
			return DbConnector.Default.ExecuteDataRecord(cmd, behavior);
		}

		/// <summary>
		/// Donastav� a vykon� SqlCommand a vr�t� prvn� ��dek prvn�ho resultsetu
		/// ve form� <see cref="Havit.Data.DataRecord"/>. Pokud neexistuje, vr�t� null.
		/// </summary>
		/// <param name="cmd">p��kaz (nemus� m�t nastaveno Connection)</param>
		/// <returns>prvn� ��dek prvn�ho resultsetu ve form� <see cref="Havit.Data.DataRecord"/></returns>
		public static DataRecord ExecuteDataRecord(SqlCommand cmd)
		{
			return DbConnector.Default.ExecuteDataRecord(cmd);
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
			return DbConnector.Default.ExecuteDataRecord(cmdText, cmdType);
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
			return DbConnector.Default.ExecuteDataRecord(cmdText);
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
			return DbConnector.Default.ExecuteScalar(cmd);
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
			return DbConnector.Default.ExecuteScalar(cmdText, cmdType);
		}


		/// <summary>
		/// Vytvo�� SqlCommand, nastav� mu DefaultCommandType, vykon� ho a vr�t�
		/// prvn� sloupec prvn�ho ��dku jeho resultsetu.
		/// </summary>
		/// <param name="cmdText">text SQL p��kazu</param>
		/// <returns>prvn� sloupec prvn�ho ��dku resultsetu</returns>
		public static object ExecuteScalar(string cmdText)
		{
			return DbConnector.Default.ExecuteScalar(cmdText);
		}

		#endregion

		#region ExecuteTransaction
		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Pokud je zadan� transakce <c>null</c>, je spu�t�na a commitov�na nov�.
		/// Pokud zadan� transakce nen� <c>null</c>, jsou zadan� kroky pouze vykon�ny.
		/// </summary>
		/// <param name="transactionWork"><see cref="SqlTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		/// <param name="outerTransaction">transakce</param>
		public static void ExecuteTransaction(SqlTransactionDelegate transactionWork, SqlTransaction outerTransaction)
		{
			DbConnector.Default.ExecuteTransaction(delegate(DbTransaction innerTransaction)
			{
				transactionWork((SqlTransaction)innerTransaction);
			}, outerTransaction);
		}

		/// <summary>
		/// Vykon� po�adovan� kroky v r�mci transakce.
		/// Je spu�t�na a commitov�na nov� samostatn� transakce.
		/// </summary>
		/// <param name="transactionWork"><see cref="SqlTransactionDelegate"/> reprezentuj�c� s �kony, kter� maj� b�t sou��st� transakce</param>
		public static void ExecuteTransaction(SqlTransactionDelegate transactionWork)
		{
			DbConnector.Default.ExecuteTransaction(delegate(DbTransaction innerTransaction)
			{
				transactionWork((SqlTransaction)innerTransaction);
			}, null);
		}
		#endregion
	}
}

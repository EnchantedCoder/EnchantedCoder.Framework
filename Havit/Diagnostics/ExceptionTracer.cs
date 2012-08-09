using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Havit.Diagnostics
{
	/// <summary>
	/// T��da zaji��uj�c� pos�l�n� v�jimek do trace prost�ednictv�m TraceSource.
	/// Mimo explicitn�ho vol�n� lze t��du p�ihl�sit k odb�ru neo�et�en�ch v�jimek, v�etn� WinForms.
	/// <example>
	/// P��klad pou�it� v ConsoleApplication:
	/// <code>
	/// namespace ExceptionLogging
	/// {
	///		class Program
	///		{
	///			static void Main(string[] args)
	///			{
	///				ExceptionTracer.Default.SubscribeToUnhandledExceptions(false);
	///
	///				ExceptionTracer.Default.TraceException(new ArgumentNullException("param", "Chyb���!"));
	///
	///				throw new InvalidOperationException("Chybka!");
	///			}
	///		}
	/// }
	/// </code>
	/// P��klad pou�it� ve WindowsApplication:
	/// <code>
	///	static void Main()
	///	{
	///		ExceptionTracer.Default.SubscribeToUnhandledExceptions(true);
	///
	///		Application.EnableVisualStyles();
	///		Application.SetCompatibleTextRenderingDefault(false);
	///		Application.Run(new Form1());
	///	}
	/// </code>
	/// P��klad konfigurace App.config:
	/// <code>
	/// &lt;configuration&gt;
	///		&lt;system.diagnostics&gt;
	///			&lt;sources&gt;
	///				&lt;source name="Exceptions" switchValue="Error"&gt;
	///					&lt;listeners&gt;
	///						&lt;add name="LogFileListener"
	///							type="System.Diagnostics.TextWriterTraceListener"
	///							 initializeData="Exceptions.log"
	///						/&gt;
	///						&lt;add name="XmlListener"
	///							 initializeData="Exceptions.xml"
	///							 type="System.Diagnostics.XmlWriterTraceListener"
	///					/&gt;
	///					&lt;/listeners&gt;
	///				&lt;/source&gt;
	///			&lt;/sources&gt;
	///		&lt;/system.diagnostics&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// </example>
	/// </summary>
	public class ExceptionTracer
	{
		private const TraceEventType traceExceptionMethodDefaultEventType = TraceEventType.Error;
		private const int traceExceptionMethodDefaultEventId = 0;

		#region TraceSourceName
		/// <summary>
		/// Jm�no TraceSource, p�es kter� se budou v�jimky emitovat.
		/// </summary>
		public string TraceSourceName
		{
			get;
			private set;
		}
		#endregion

		#region constructor
		/// <summary>
		/// Vytvo�� instanci ExceptionTraceru, kter� bude sv�j v�stup sm��ovat p�es TraceSource se zadan�m jm�nem.
		/// </summary>
		/// <param name="traceSourceName">jm�no TraceSource, p�es kter� se budou v�jimky emitovat</param>
		public ExceptionTracer(string traceSourceName)
		{
			this.TraceSourceName = traceSourceName;
		}
		#endregion

		#region SubscribeToUnhandledExceptions
		/// <summary>
		/// P�ihl�s� ExceptionTracer k odb�ru v�ech neobslou�en�ch v�jimek (event AppDomain.CurrentDomain.UnhandledException).
		/// </summary>
		public void SubscribeToUnhandledExceptions(bool includeWindowsFormsThreadExceptions)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			if (includeWindowsFormsThreadExceptions)
			{
				SubscribeToWindowsFormsThreadExceptions();
			}
		}

		/// <summary>
		/// Obsluha ud�losti AppDomain.CurrentDomain.UnhandledException.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception)
			{
				TraceException((Exception)e.ExceptionObject, TraceEventType.Critical);
			}
		}
		#endregion

		#region SubscribeToWindowsFormsThreadExceptions
		/// <summary>
		/// P�ihl�s� ExceptionTracer k odb�ru v�ech neobslou�en�ch v�jimek WinForm (event Application.ThreadException).
		/// </summary>
		public void SubscribeToWindowsFormsThreadExceptions()
		{
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
		}

		/// <summary>
		/// Obsluha ud�losti Application.ThreadException.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
		private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			TraceException(e.Exception, TraceEventType.Critical);

			// p�vodn� implementace obsluhy v�jimky
			using (ThreadExceptionDialog excptDlg = new ThreadExceptionDialog(e.Exception))
			{
				DialogResult result = excptDlg.ShowDialog();
				if (result == DialogResult.Abort)
				{
					Application.Exit();
				}
			}
		}
		#endregion

		#region TraceException
		/// <summary>
		/// Po�le do trace zadanou v�jimku.
		/// </summary>
		/// <param name="exception">v�jimka k zaznamen�n�</param>
		/// <param name="eventType">typ ud�losti, pod kter�m se m� v�jimka zaznamenat</param>
		/// <param name="eventId">ID eventu, pod kter�m se m� v�jimka zaznamenat</param>
		public void TraceException(Exception exception, TraceEventType eventType, int eventId)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}

			RunUsingTraceSource(delegate(TraceSource ts)
			{
				ts.TraceEvent(eventType, eventId, FormatException(exception));
			});
		}

		/// <summary>
		/// Po�le do trace zadanou v�jimku.
		/// </summary>
		/// <param name="exception">v�jimka k zaznamen�n�</param>
		/// <param name="eventType">typ ud�losti, pod kter�m se m� v�jimka zaznamenat</param>
		public void TraceException(Exception exception, TraceEventType eventType)
		{
			TraceException(exception, eventType, traceExceptionMethodDefaultEventId);
		}


		/// <summary>
		/// Po�le do trace zadanou v�jimku.
		/// </summary>
		/// <param name="exception">v�jimka k zaznamen�n�</param>
		public void TraceException(Exception exception)
		{
			TraceException(exception, traceExceptionMethodDefaultEventType, traceExceptionMethodDefaultEventId);
		}
		#endregion

		#region FormatException (private)
		/// <summary>
		/// Naform�tuje v�jimku pro z�pis do trace.
		/// </summary>
		/// <param name="exception">v�jimka</param>
		/// <returns>textov� v�stup, kter� se po�le do trace (informace o v�jimce)</returns>
		private string FormatException(Exception exception)
		{
			// do budoucna je mo�n� roz���it objektov� model o ExceptionTraceFormatter, atp.
			return exception.ToString();
		}
		#endregion

		#region RunUsingTraceSource (private)
		/// <summary>
		/// Vykon� akci pomoc� TraceSource pou��van�ho ExceptionListenerem.
		/// </summary>
		/// <param name="action">akce k vykon�n� (deleg�t)</param>
		private void RunUsingTraceSource(Action<TraceSource> action)
		{
			Debug.Assert(action != null);

			TraceSource ts = new TraceSource(this.TraceSourceName);

			action(ts);

			ts.Flush();
			ts.Close();
		}
		#endregion

		/*********************************************************************/

		#region DefaultTraceSourceName (const)
		/// <summary>
		/// N�zev v�choz�ho TraceSource, p�es kter� jsou v�jimky emitov�ny.
		/// </summary>
		public const string DefaultTraceSourceName = "Exceptions";
		#endregion

		#region Default (static singleton)
		/// <summary>
		/// V�choz� ExceptionTracer sm��uj�c� v�stup p�es TraceSource s DefaultTraceSourceName.
		/// </summary>
		public static ExceptionTracer Default
		{
			get
			{
				if (_default == null)
				{
					lock (defaultLock)
					{
						if (_default == null)
						{
							_default = new ExceptionTracer(DefaultTraceSourceName);
						}
					}
				}
				return _default;
			}
			set
			{
				_default = value;
			}
		}
		private static ExceptionTracer _default;
		private static object defaultLock = new object();
		#endregion
	}
}

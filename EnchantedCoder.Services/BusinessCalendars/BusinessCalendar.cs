using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EnchantedCoder.Services.TimeServices;

namespace EnchantedCoder.Services.BusinessCalendars
{
	/// <summary>
	/// Třída poskytující funkčnost pro práci s pracovním kalendářem,
	/// pracovními dny, svátky, atp.
	/// </summary>
	/// <remarks>
	/// Pracovním dnem (business day) je den, který není sobotou, nedělí ani svátkem.<br/>
	/// Třída se instancializuje se sadou významných dnů (zpravidla svátků), nebo bez svátků (pracovním
	/// dnem je pak den, který není sobotou ani nedělí).<br/>
	/// Jako svátky (holiday) lze samozřejmě předat i různé dovolené apod.<br/>
	/// <br/>
	/// Jednou vytvořenou instanci třídy lze s výhodou opakovaně používat.
	/// </remarks>
	public class BusinessCalendar
	{
		/// <summary>
		/// Interní seznam významných dnů, tj. dnů, které se liší od běžného pracovního dne (např. svátků, atp.).<br/>
		/// Klíč je DateTime, hodnota je DateInfo.
		/// </summary>
		private readonly IDictionary<DateTime, IDateInfo> dateInfos;

		/// <summary>
		/// Strategie používaná pro zjištění, zda je daný den podle kalendáře víkendem.
		/// </summary>
		private readonly IIsWeekendStrategy isWeekendStrategy;

		/// <summary>
		/// Vytvoří instanci <see cref="BusinessCalendar"/> bez významných dnů.<br/>
		/// Pracovními dny budou všechny dny mimo víkendů, dokud nebudou přidány nějaké svátky.
		/// Víkendové dny jsou sobota a neděle.
		/// </summary>
		public BusinessCalendar()
		{
			this.dateInfos = new Dictionary<DateTime, IDateInfo>();
			this.isWeekendStrategy = BusinessCalendarWeekendStrategy.GetSaturdaySundayStrategy();
		}

		/// <summary>
		/// Vytvoří instanci <see cref="BusinessCalendar"/> bez významných dnů.<br/>
		/// Pracovními dny budou všechny dny mimo víkendů, dokud nebudou přidány nějaké svátky.
		/// Víkendové dny jsou určeny strategií weekendStrategy
		/// </summary>
		public BusinessCalendar(IIsWeekendStrategy isWeekendStrategy)
		{
			this.dateInfos = new Dictionary<DateTime, IDateInfo>();
			this.isWeekendStrategy = isWeekendStrategy;
		}

		/// <summary>
		/// Přidá do nastavení <see cref="BusinessCalendar"/> významné dny. Pokud již některý den v kalendáři existuje, přepíše ho.
		/// </summary>
		/// <typeparam name="T">typ významných dnů (musí implementovat rozhraní <see cref="IDateInfo"/>)</typeparam>
		/// <param name="dateInfos">kolekce významných dnů</param>
		[SuppressMessage("SonarLint", "S1117", Justification = "Není chybou mít parametr metody stejného jména ve třídě.")]
		public void FillDates<T>(IEnumerable<T> dateInfos)
			where T : IDateInfo
		{
			foreach (T item in dateInfos)
			{
				this.dateInfos[item.Date.Date] = item;
			}
		}

		/// <summary>
		/// Vrací IDateInfo pro daný den. Pokud není záznam pro daný den evidován, vrací null.
		/// </summary>
		protected IDateInfo GetDateInfo(DateTime time)
		{
			IDateInfo dateInfo;
			if (dateInfos.TryGetValue(time.Date, out dateInfo))
			{
				return dateInfo;
			}
			return null;
		}

		/// <summary>
		/// Určí následující pracovní den.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, ke kterému má být následující pracovní den určen.</param>
		/// <returns><see cref="System.DateTime"/>, který je následujícím pracovním dnem.</returns>
		/// <remarks>Časový údaj zůstane nedotčen.</remarks>
		public DateTime GetNextBusinessDay(DateTime time)
		{
			do
			{
				time = time.AddDays(1);
			}
			while (!IsBusinessDay(time));

			return time;
		}

		/// <summary>
		/// Určí předchozí pracovní den.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, ke kterému má být předchozí pracovní den určen.</param>
		/// <returns><see cref="System.DateTime"/>, který je předchozím pracovním dnem.</returns>
		/// <remarks>Časový údaj zůstane nedotčen.</remarks>
		public DateTime GetPreviousBusinessDay(DateTime time)
		{
			do
			{
				time = time.AddDays(-1);
			}
			while (!IsBusinessDay(time));

			return time;
		}

		/// <summary>
		/// Určí den, který je x-tým následujícím pracovním dnem po dni zadaném.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, od kterého se určovaný den odvíjí.</param>
		/// <param name="businessDays">kolikátý pracovní den má být určen</param>
		/// <returns><see cref="System.DateTime"/>, který je x-tým následujícím pracovním dnem po dni zadaném.</returns>
		/// <remarks>Časový údaj zůstane nedotčen.</remarks>
		public DateTime AddBusinessDays(DateTime time, int businessDays)
		{
			if (businessDays >= 0)
			{
				for (int i = 0; i < businessDays; i++)
				{
					time = GetNextBusinessDay(time);
				}
			}
			else
			{
				for (int i = 0; i > businessDays; i--)
				{
					time = GetPreviousBusinessDay(time);
				}
			}
			return time;
		}

		/// <summary>
		/// Určí, zdali je zadaný den dnem pracovním. Za pracovní považujeme den, který není ani víkendem, ani svátkem.
		/// </summary>
		public virtual bool IsBusinessDay(DateTime time)
		{
			if (IsWeekend(time) || IsHoliday(time))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Zjistí, zdali je den svátkem.
		/// </summary>
		public virtual bool IsHoliday(DateTime date)
		{
			IDateInfo dateInfo = GetDateInfo(date);
			if (dateInfo != null)
			{
				return dateInfo.IsHoliday;
			}
			return false;
		}

		/// <summary>
		/// Určí, zdali je zadaný den víkendem.
		/// </summary>
		public virtual bool IsWeekend(DateTime time)
		{
			return isWeekendStrategy.IsWeekend(time.Date);
		}

		/// <summary>
		/// Spočítá počet pracovních dní mezi dvěma daty.
		/// </summary>
		/// <param name="startDate">Počáteční datum</param>
		/// <param name="endDate">Koncové datum</param>
		/// <param name="options">Options pro počítání dnů.</param>
		/// <returns>Počet pracovních dnů mezi počátečním a koncovým datem (v závislosti na <c>options</c>).</returns>
		public int CountBusinessDays(DateTime startDate, DateTime endDate, CountBusinessDaysOptions options)
		{
			// pokud jsou data obráceně, vrátíme záporný výsledek sama sebe v opačném pořadí dat
			if (startDate > endDate)
			{
				return -CountBusinessDays(endDate, startDate, options);
			}

			int counter = 0;
			DateTime currentDate = startDate;

			// procházíme všecha data až před endDate
			while (currentDate.Date < endDate.Date)
			{
				if (this.IsBusinessDay(currentDate))
				{
					counter++;
				}
				currentDate = currentDate.AddDays(1);
			}

			// pokud chceme zohlednit endDate pak ho započteme (pokud je pracovní)
			if ((options == CountBusinessDaysOptions.IncludeEndDate) && (this.IsBusinessDay(endDate)))
			{
				counter++;
			}

			return counter;
		}
	}
}

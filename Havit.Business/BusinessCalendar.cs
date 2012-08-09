using System;
using System.Collections;
using System.Collections.Generic;

namespace Havit.Business
{
	/// <summary>
	/// T��da poskytuj�c� funk�nost pro pr�ci s pracovn�m kalend��em,
	/// pracovn�mi dny, sv�tky, atp.
	/// </summary>
	/// <remarks>
	/// Pracovn�m dnem (business day) je den, kter� nen� sobotou, ned�l� ani sv�tkem.<br/>
	/// T��da se instancializuje se sadou v�znamn�ch dn� (zpravidla sv�tk�), nebo bez sv�tk� (pracovn�m
	/// dnem je pak den, kter� nen� sobotou ani ned�l�).<br/>
	/// Jako sv�tky (holiday) lze samoz�ejm� p�edat i r�zn� dovolen� apod.<br/>
	/// <br/>
	/// Jednou vytvo�enou instanci t��dy lze s v�hodou opakovan� pou��vat.
	/// </remarks>
	public class BusinessCalendar
	{
		#region dates (private)
		/// <summary>
		/// Intern� seznam v�znamn�ch dn�, tj. dn�, kter� se li�� od b�n�ho pracovn�ho dne (nap�. sv�tk�, atp.).<br/>
		/// Kl�� je DateTime, hodnota je DateInfo.
		/// </summary>
		private readonly IDictionary<DateTime, IDateInfo> dates = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/> bez v�znamn�ch dn�.<br/>
		/// Pracovn�mi dny budou v�echny dny mimo v�kend�, dokud nebudou p�id�ny n�jak� sv�tky.
		/// </summary>
		public BusinessCalendar()
		{
			this.dates = new Dictionary<DateTime, IDateInfo>();
		}

		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/>.<br/>
		/// </summary>
		/// <param name="dateInfoDictionary">dictionary s informacemi o v�znamn�ch dnech</param>
		public BusinessCalendar(IDictionary<DateTime, IDateInfo> dateInfoDictionary)
		{
			this.dates = dateInfoDictionary;
		}

		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/>.<br/>
		/// Sv�tky jsou p�ed�ny v poli <see cref="System.DateTime"/>.
		/// </summary>
		/// <param name="holidays">pole sv�tk� <see cref="System.DateTime"/></param>
		public BusinessCalendar(DateTime[] holidays)
		{
			this.dates = new Dictionary<DateTime, IDateInfo>();
			foreach (DateTime holiday in holidays)
			{
				DateInfo di = new DateInfo(holiday.Date);
				di.SetAsHoliday();
				this.dates.Add(holiday.Date, di);
			}
		}
		#endregion

		#region FillDates
		/// <summary>
		/// P�id� do nastaven� <see cref="BusinessCalendar"/> v�znamn� dny. Pokud ji� n�kter� den v kalend��i existuje, p�ep�e ho. 
		/// </summary>
		/// <typeparam name="T">typ v�znamn�ch dn� (mus� implementovat rozhran� <see cref="IDateInfo"/>)</typeparam>
		/// <param name="dateInfos">kolekce v�znamn�ch dn�</param>
		public void FillDates<T>(IEnumerable<T> dateInfos)
			where T : IDateInfo
		{
			foreach (T item in dateInfos)
			{
				dates[item.Date.Date] = item;
			}
		}
		#endregion

		#region GetNextBusinessDay, GetPreviousBusinessDay
		/// <summary>
		/// Ur�� n�sleduj�c� pracovn� den.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, ke kter�mu m� b�t n�sleduj�c� pracovn� den ur�en.</param>
		/// <returns><see cref="System.DateTime"/>, kter� je n�sleduj�c�m pracovn�m dnem.</returns>
		/// <remarks>�asov� �daj z�stane nedot�en.</remarks>
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
		/// Ur�� p�edchoz� pracovn� den.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, ke kter�mu m� b�t p�edchoz� pracovn� den ur�en.</param>
		/// <returns><see cref="System.DateTime"/>, kter� je p�edchoz�m pracovn�m dnem.</returns>
		/// <remarks>�asov� �daj z�stane nedot�en.</remarks>
		public DateTime GetPreviousBusinessDay(DateTime time)
		{
			do
			{
				time = time.AddDays(-1);
			}
			while (!IsBusinessDay(time));

			return time;
		}
		#endregion

		#region AddBusinessDays
		/// <summary>
		/// Ur�� den, kter� je x-t�m n�sleduj�c�m pracovn�m dnem po dni zadan�m.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, od kter�ho se ur�ovan� den odv�j�.</param>
		/// <param name="businessDays">kolik�t� pracovn� den m� b�t ur�en</param>
		/// <returns><see cref="System.DateTime"/>, kter� je x-t�m n�sleduj�c�m pracovn�m dnem po dni zadan�m.</returns>
		/// <remarks>�asov� �daj z�stane nedot�en.</remarks>
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
		#endregion

		#region IsBusinessDay
		/// <summary>
		/// Ur��, zda-li je zadan� den dnem pracovn�m.
		/// </summary>
		/// <param name="time"><see cref="DateTime"/>, u kter�ho chceme vlastnosti zjistit</param>
		/// <returns><b>false</b>, pokud je <see cref="DateTime"/> v�kendem nebo sv�tkem; jinak <b>true</b></returns>
		public virtual bool IsBusinessDay(DateTime time)
		{
			if (IsWeekend(time) || IsHoliday(time))
			{
				return false;
			}
			return true;
		}
		#endregion

		#region IsHoliday
		/// <summary>
		/// Zjist�, zda-li je <see cref="System.DateTime"/> sv�tkem (dovolenou, ...).
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, u kter�ho m� b�t vlastnost zji�t�na</param>
		/// <returns><b>true</b>, pokud je den v seznamu sv�tk�, s nimi� byl <see cref="BusinessCalendar"/> instanciov�n; jinak <b>false</b></returns>
		public virtual bool IsHoliday(DateTime time)
		{
			if (dates == null)
			{
				return false;
			}

			IDateInfo dateInfo;
			if (dates.TryGetValue(time.Date, out dateInfo))
			{
				return dateInfo.IsHoliday;
			}
			return false;
		}
		#endregion

		#region IsWeekend
		/// <summary>
		/// Ur��, zda-li je zadan� den sobotou nebo ned�l�.
		/// </summary>
		/// <param name="time"><see cref="System.DateTime"/>, u kter�ho ur�ujeme</param>
		/// <returns><b>true</b>, pokud je zadan� <see cref="System.DateTime"/> sobota nebo ned�le; jinak <b>false</b></returns>
		public virtual bool IsWeekend(DateTime time)
		{
			DayOfWeek dayOfWeek = time.DayOfWeek;
			if ((dayOfWeek == DayOfWeek.Saturday) || (dayOfWeek == DayOfWeek.Sunday))
			{
				return true;
			}
			return false;
		}
		#endregion

		#region CountBusinessDays
		/// <summary>
		/// Spo��t� po�et pracovn�ch dn� mezi dv�ma daty. 
		/// </summary>
		/// <param name="startDate">po��te�n� datum</param>
		/// <param name="endDate">koncov� datum</param>
		/// <param name="options">Options pro po��t�n� dn�.</param>
		/// <returns>po�et pracovn�ch dn� mezi po��te�n�m a koncov�m datem (v z�vislosti na <c>options</c>)</returns>
		public int CountBusinessDays(DateTime startDate, DateTime endDate, CountBusinessDaysOptions options)
		{
			// pokud jsou data obr�cen�, vr�t�me z�porn� v�sledek sama sebe v opa�n�m po�ad� dat
			if (startDate > endDate)
			{
				return -CountBusinessDays(endDate, startDate, options);
			}

			int counter = 0;
			DateTime currentDate = startDate;

			// proch�z�me v�echa data a� p�ed endDate 
			while (currentDate.Date < endDate.Date)
			{
				if (this.IsBusinessDay(currentDate))
				{
					counter++;
				}
				currentDate = currentDate.AddDays(1);
			}

			// pokud chceme zohlednit endDate pak ho zapo�teme (pokud je pracovn�)
			if ((options == CountBusinessDaysOptions.IncludeEndDate) && (this.IsBusinessDay(endDate)))
			{
				counter++;
			}

			return counter;
		}
		#endregion
	}
}

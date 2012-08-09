using System;
using System.Collections;

namespace Havit.Business
{
	/// <summary>
	/// T��da poskytuj�c� funk�nost pro pr�ci s pracovn�m kalend��em,
	/// pracovn�mi dny, sv�tky, atp.
	/// </summary>
	/// <remarks>
	/// Pracovn�m dnem (business day) je den, kter� nen� sobotou, ned�l� ani sv�tkem.<br/>
	/// T��da se instancializuje se sadou sv�tk� (holidays), nebo bez sv�tk� (pracovn�m
	/// dnem je pak den, kter� nen� sobotou ani ned�l�).<br/>
	/// Jako sv�tky (holiday) lze samoz�ejm� p�edat i r�zn� dovolen� apod.<br/>
	/// <br/>
	/// Jednou vytvo�enou instanci t��dy lze s v�hodou cachovat.
	/// </remarks>
	public sealed class BusinessCalendar
	{
		#region Private data fields
		/// <summary>
		/// Intern� dictionary sv�tk�.<br/>
		/// Kl�� je DateTime, hodnota je DateInfo.
		/// </summary>
		private readonly DateInfoDictionary holidayDictionary = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/> bez sv�tk�.<br/>
		/// Pracovn�mi dny budou v�echny dny mimo v�kend�, dokud nebudou p�id�ny n�jak� sv�tky.
		/// </summary>
		public BusinessCalendar()
		{
		}

		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/> se sv�tky.<br/>
		/// </summary>
		/// <param name="holidayDictionary"><see cref="DateInfoDictionary"/> se sv�tky (POUZE SE SV�TKY!!!)</param>
		/// <remarks>
		/// Nekontroluje se, jestli maj� v�echny dny z holidayDictionary nastaveno <see cref="DateInfo.IsHoliday"/>.
		/// </remarks>
		public BusinessCalendar(DateInfoDictionary holidayDictionary)
		{
			this.holidayDictionary = holidayDictionary;
		}

		/// <summary>
		/// Vytvo�� instanci <see cref="BusinessCalendar"/> se sv�tky.<br/>
		/// Sv�tky jsou p�ed�ny v poli <see cref="System.DateTime"/>.
		/// </summary>
		/// <param name="holidays">pole sv�tk� <see cref="System.DateTime"/></param>
		public BusinessCalendar(DateTime[] holidays)
		{
			this.holidayDictionary = new DateInfoDictionary();
			foreach (DateTime holiday in holidays)
			{
				this.holidayDictionary.Add(new DateInfo(holiday));
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
		public bool IsBusinessDay(DateTime time)
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
		public bool IsHoliday(DateTime time)
		{
			if (holidayDictionary == null)
			{
				return false;
			}
			if (holidayDictionary.Contains(time))
			{
				return true;
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
		public bool IsWeekend(DateTime time)
		{
			DayOfWeek dayOfWeek = time.DayOfWeek;
			if ((dayOfWeek == DayOfWeek.Saturday) || (dayOfWeek == DayOfWeek.Sunday))
			{
				return true;
			}
			return false;
		}
		#endregion
	}
}

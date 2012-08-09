using System;

namespace Havit.Business
{
	/// <summary>
	/// T��da pro informace o dni.
	/// </summary>
	public class DateInfo : IDateInfo
	{
		#region Data Fields - hodnoty
		/// <summary>
		/// Vr�t� den, kter�mu DateInfo pat��.
		/// </summary>
		public DateTime Date
		{
			get { return _date; }
		}
		private readonly DateTime _date;

		/// <summary>
		/// Indikuje, zda-li je den sv�tkem.
		/// </summary>
		public bool IsHoliday
		{
			get { return _isHoliday; }
		}
		private bool _isHoliday;

		/// <summary>
		/// Textov� popis sv�tku, pokud je den sv�tkem.
		/// </summary>
		public string HolidayDescription
		{
			get { return _holidayDescription; }
		}
		private string _holidayDescription;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci <see cref="DateInfo"/>.
		/// </summary>
		/// <param name="date">den, kter� m� b�t reprezentov�n</param>
		public DateInfo(DateTime date)
		{
			this._date = date.Date;
		}
		#endregion

		#region SetAsHoliday
		/// <summary>
		/// Nastav� den jako sv�tek.
		/// </summary>
		/// <param name="holidayDescription">textov� popis sv�tku</param>
		public void SetAsHoliday(string holidayDescription)
		{
			this._isHoliday = true;
			this._holidayDescription = holidayDescription;
		}

		/// <summary>
		/// Nastav� den jako sv�tek.
		/// </summary>
		public void SetAsHoliday()
		{
			SetAsHoliday(null);
		}
		#endregion
	}
}

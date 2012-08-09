using System;

namespace Havit.Business
{
	/// <summary>
	/// T��da pro informace o dni.
	/// </summary>
	public class DateInfo
	{
		#region Data Fields - hodnoty
		/// <summary>
		/// Vr�t� den, kter�mu DateInfo pat��.
		/// </summary>
		public DateTime Date
		{
			get { return date; }
		}
		private readonly DateTime date;

		/// <summary>
		/// Indikuje, zda-li je den sv�tkem.
		/// </summary>
		public bool IsHoliday
		{
			get { return isHoliday; }
		}
		private bool isHoliday;

		/// <summary>
		/// Textov� popis sv�tku, pokud je den sv�tkem.
		/// </summary>
		public string HolidayDescription
		{
			get { return holidayDescription; }
		}
		private string holidayDescription;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci <see cref="DateInfo"/>.
		/// </summary>
		/// <param name="date">den, kter� m� b�t reprezentov�n</param>
		public DateInfo(DateTime date)
		{
			this.date = date.Date;
		}
		#endregion

		#region SetAsHoliday
		/// <summary>
		/// Nastav� den jako sv�tek.
		/// </summary>
		/// <param name="holidayDescription">textov� popis sv�tku</param>
		public void SetAsHoliday(string holidayDescription)
		{
			this.isHoliday = true;
			this.holidayDescription = holidayDescription;
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

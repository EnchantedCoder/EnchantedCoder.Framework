using System;
using System.Collections.Generic;

namespace Havit.AspNetCore.Mvc.Filters.ErrorToJson
{
	/// <summary>
	/// Zaji��uje konfiguraci mapov�n� z u�ivatelsk�ho k�du.
	/// </summary>
    public class ErrorToJsonSetup
    {
        private readonly List<ErrorToJsonMappingItem> mapping = new List<ErrorToJsonMappingItem>();

		/// <summary>
		/// Mapuje dan� typ v�jimky na dan� stavov� k�d. Jako n�vratov� hodnota je vr�cen objekt s vlastnostmi StatusCode a Message s hodnotou text v�jimky (exception.Message).
		/// </summary>
		/// <param name="exceptionType">Typ v�jimky.</param>
		/// <param name="statusCode">Stavov� k�d pro http odpov�� (a objekt s odpov�d�).</param>
		/// <param name="markExceptionAsHandled">Indikace, zda m� b�t v�jimka ozna�ena za zpracovanou.</param>
        public void Map(Type exceptionType, int statusCode, bool markExceptionAsHandled = false)
        {
	        if (!typeof(Exception).IsAssignableFrom(exceptionType))
	        {
		        throw new ArgumentException("Only exception types can be used.", nameof(exceptionType));
	        }

			this.Map(e => exceptionType.IsAssignableFrom(e.GetType()), e => statusCode, e => new { StatusCode = statusCode, Message = e.Message }, e => markExceptionAsHandled);
        }

		/// <summary>
		/// Mapuje dan� typ v�jimky na dan� stavov� k�d. Jako n�vratov� hodnota je vr�cen objekt s vlastnostmi StatusCode a Message s hodnotou text v�jimky (exception.Message).
		/// </summary>
		/// <param name="predicate">Predik�t ov��uj�c� polo�ku mapov�n� podle v�jimky.</param>
		/// <param name="statusCodeSelector">Funkce vracej�c� pro danou v�jimku stavov� k�d http odpov�di.</param>
		/// <param name="resultSelector">Funkce vracej�c� pro danou v�jimku odpov�� (objekt n�sledn� vracen� jako JSON).</param>
		/// <param name="markExceptionAsHandled">Funkce rozhoduj�c�, zda m� b�t v�jimka ozna�ena za zpracovanou.</param>
        public void Map(Predicate<Exception> predicate, Func<Exception, int> statusCodeSelector, Func<Exception, object> resultSelector, Func<Exception, bool> markExceptionAsHandled)
        {
            mapping.Add(new ErrorToJsonMappingItem(predicate, statusCodeSelector, resultSelector, markExceptionAsHandled));
        }

		/// <summary>
		/// Vrac� provedenou konfiguraci.
		/// </summary>
        public ErrorToJsonConfiguration GetConfiguration()
        {
            return new ErrorToJsonConfiguration(mapping.AsReadOnly());
        }
    }
}
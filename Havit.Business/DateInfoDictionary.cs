using System;
using System.Collections;

namespace Havit.Business
{
	/// <summary>
	/// Dictionary pro kl�� <see cref="System.DateTime"/> a hodnoty <see cref="DateInfo"/>.
	/// </summary>
	public class DateInfoDictionary : DictionaryBase
	{
		#region Constructor
		/// <summary>
		/// Vytvo�� pr�zdnou instanci <see cref="DateInfoDictionary"/>
		/// </summary>
		public DateInfoDictionary()
		{
		}
		#endregion

		#region Indexer
		/// <summary>
		/// Indexer p�es kl��e <see cref="System.DateTime"/>.
		/// </summary>
		public DateInfo this[DateTime key]
		{
			get
			{
				return (DateInfo)Dictionary[key];
			}
			set
			{
				Dictionary[key] = value;
			}
		}
		#endregion

		#region Add
		/// <summary>
		/// P�id� <see cref="DateInfo"/> do slovn�ku.<br/>
		/// Kl��em je <see cref="DateInfo.Date"/>.
		/// </summary>
		/// <param name="value">Prvek, kter� m� b�t p�id�n do slovn�ku.</param>
		public void Add(DateInfo value)
		{
			Dictionary.Add(value.Date, value);
		}
		#endregion

		#region Constains
		/// <summary>
		/// Zjist�, zda-li je ve slovn�ku po�adovan� den.
		/// </summary>
		/// <param name="key">zji��ovan� den</param>
		public bool Contains(DateTime key)
		{
			return Dictionary.Contains(key);
		}
		#endregion
	}
}

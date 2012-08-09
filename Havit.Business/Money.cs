using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// T��da reprezentuj�c� pen�n� ��stky s m�nou.
	/// </summary>
	[Serializable]
	public class Money<TCurrency>
		where TCurrency: class
	{		
		#region Amount
		/// <summary>
		/// ��stka v jednotk�ch m�ny. Ud�v� se hodnota v z�kladn�
		/// jednotce a zlomky m�ny (nap�. hal��e) se zad�vaj� za desetinou te�kou (nap�.
		/// 57.30).
		/// </summary>
		public decimal? Amount
		{
			get { return _amount; }
			set
			{
				bool changed = (_amount == value);
				_amount = value;

				if (changed && (ValueChanged != null))
				{
					ValueChanged(this, EventArgs.Empty);
				}
			}
		}
		private decimal? _amount;
		#endregion

		#region Currency
		/// <summary>
		/// M�na ��stky.
		/// </summary>
		public TCurrency Currency
		{
			get { return _currency; }
			set
			{
				bool changed = (_currency == value);
				_currency = value;

				if (changed && (ValueChanged != null))
				{
					ValueChanged(this, EventArgs.Empty);
				}
			}
		}
		private TCurrency _currency;
		#endregion

		public event EventHandler ValueChanged;

		#region Constructors
		/// <summary>
		/// Inicializuje t��du money s pr�zdn�mi hodnotami (Amount i Currency jsou null).
		/// </summary>
		public Money(): this(null, null)
		{
		}

		/// <summary>
		/// Inicializuje t��du money zadan�mi hodnotami.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="currency"></param>
		public Money(decimal? amount, TCurrency currency)
		{
			this._amount = amount;
			this._currency = currency;
		}
		#endregion

		#region Equals, oper�tory == a !=
		public virtual bool Equals(Money<TCurrency> money)
		{
			if ((money == null) || (this.GetType() != money.GetType()))
			{
				return false;
			}
			return (this.Amount == money.Amount) && (this.Currency == money.Currency);
		}

		public override bool Equals(object obj)
		{
			if (obj is Money<TCurrency>)
			{
				Money<TCurrency> money = obj as Money<TCurrency>;
				return this.Equals(money);
			}
			return false;
		}

		public static bool operator ==(Money<TCurrency> objA, Money<TCurrency> objB)
		{
			return Object.Equals(objA, objB);
		}
		public static bool operator !=(Money<TCurrency> objA, Money<TCurrency> objB)
		{
			return !Object.Equals(objA, objB);
		}
		#endregion

		#region GetHashCode
		/// <summary>
		/// HashCode je slo�en jako XOR hash k�d� amount a currency, pokud tyto hodnoty nejsou null.
		/// </summary>
		public override int GetHashCode()
		{
			int result = 0;

			if (_amount != null)
			{
				result ^= _amount.GetHashCode();
			}

			if (_currency != null)
			{
				result ^= _currency.GetHashCode();
			}

			return result;
		}
		#endregion

		#region AssertSameCurrencies
		/// <summary>
		/// Porovn� m�ny zadan� v parametrech. Pokud se li��, je vyhozena v�jimka.
		/// </summary>
		protected static void AssertSameCurrencies(TCurrency currency1, TCurrency currency2)
		{			
			if (currency1 != currency2)
			{
				throw new InvalidOperationException("Assertion failed: Currencies are not same.");
			}
		}
		#endregion

		#region AssertNotNull
		/// <summary>
		/// Vyvol� v�jimku, pokud m� parametr hodnotu null.
		/// </summary>
		private static void AssertNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName, String.Format("Assertion failed: Value ({0}) is null.", parameterName));
			}
		}
		#endregion

		#region Oper�tory porovn�n� <, >, <=, >=
		/// <summary>
		/// Porovn� se ��stka. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static bool operator >(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			AssertSameCurrencies(money1.Currency, money2.Currency);
			return money1.Amount > money2.Amount;
		}

		/// <summary>
		/// Porovn� se ��stka. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static bool operator <(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			AssertSameCurrencies(money1.Currency, money2.Currency);
			return money1.Amount < money2.Amount;
		}

		/// <summary>
		/// Porovn� se ��stka. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static bool operator >=(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			return (money1 > money2) || (money1 == money2);
		}

		/// <summary>
		/// Porovn� se ��stka. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static bool operator <=(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			return (money1 < money2) || (money1 == money2);
		}
		#endregion

		#region Oper�tory +, -, *, /
		/// <summary>
		/// Se�te dv� hodnoty Money. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static Money<TCurrency> operator +(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			return SumMoney<Money<TCurrency>>(money1, money2);
		}

		/// <summary>
		/// Ode�te dv� hodnoty Money. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		/// <returns></returns>
		public static Money<TCurrency> operator -(Money<TCurrency> money1, Money<TCurrency> money2)
		{
			return SubtractMoney<Money<TCurrency>>(money1, money2);
		}

		/// <summary>
		/// Vyn�sob� hodnotu Money konstantou typu decimal.
		/// </summary>
		public static Money<TCurrency> operator *(Money<TCurrency> money, decimal multiplicand)
		{
			return MultipleMoney<Money<TCurrency>>(money, multiplicand);
		}

		/// <summary>
		/// Vyn�sob� hodnotu Money konstantou typu int.
		/// </summary>
		public static Money<TCurrency> operator *(Money<TCurrency> money, int multiplicand)
		{
			return MultipleMoney<Money<TCurrency>>(money, multiplicand);
		}
		
		
		/// <summary>
		/// Vyd�l� hodnotu Money konstantou typu decimal.
		/// </summary>
		public static Money<TCurrency> operator /(Money<TCurrency> money, decimal multiplicand)
		{
			return DivideMoney<Money<TCurrency>>(money, multiplicand);
		}

		/// <summary>
		/// Vyd�l� hodnotu Money konstantou typu int.
		/// </summary>
		public static Money<TCurrency> operator /(Money<TCurrency> money, int multiplicand)
		{
			return DivideMoney<Money<TCurrency>>(money, multiplicand);
		}
		
		/// <summary>
		/// Vypo�te pod�l ��stek. Nap�. pro v�po�et pom�ru ��stek, mar�e, apod.
		/// </summary>
		public static decimal operator /(Money<TCurrency> dividend, Money<TCurrency> divisor)
		{
			return DivideMoney(dividend, divisor);
		}

		#endregion

		#region SumMoney
		/// <summary>
		/// Se�te dv� hodnoty Money. Pokud se neshoduje m�na, operace vyvol� v�jimku.	
		/// </summary>
		public static TResult SumMoney<TResult>(Money<TCurrency> money1, Money<TCurrency> money2)
			where TResult : Money<TCurrency>, new()
		{
			AssertSameCurrencies(money1.Currency, money2.Currency);
			AssertNotNull(money1, "money1");
			AssertNotNull(money1.Amount, "money1.Amount");
			AssertNotNull(money2, "money2");
			AssertNotNull(money2.Amount, "money2.Amount");

			TResult result = new TResult();
			result.Amount = money1.Amount + money2.Amount;
			result.Currency = money1.Currency;
			return result;
		}
		#endregion

		#region SubtractMoney
		/// <summary>
		/// Ode�te m�ny (od��t� se money2 od money1). Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static TResult SubtractMoney<TResult>(Money<TCurrency> money1, Money<TCurrency> money2)
			where TResult : Money<TCurrency>, new()
		{
			AssertSameCurrencies(money1.Currency, money2.Currency);
			AssertNotNull(money1, "money1");
			AssertNotNull(money1.Amount, "money1.Amount");
			AssertNotNull(money2, "money2");
			AssertNotNull(money2.Amount, "money2.Amount");

			TResult result = new TResult();
			result.Amount = money1.Amount - money2.Amount;
			result.Currency = money1.Currency;
			return result;
		}
		#endregion

		#region MultipleMoney
		/// <summary>
		/// Vyn�sob� ��stku konstantou.
		/// </summary>
		public static TResult MultipleMoney<TResult>(Money<TCurrency> money, decimal multiplicand)
			where TResult : Money<TCurrency>, new()
		{
			AssertNotNull(money, "money");
			AssertNotNull(money.Amount, "money.Amount");

			TResult result = new TResult();
			result.Amount = money.Amount * multiplicand;
			result.Currency = money.Currency;
			return result;
		}
		#endregion

		#region DivideMoney
		/// <summary>
		/// Vyd�l� ��stku konstantou.
		/// </summary>
		public static TResult DivideMoney<TResult>(Money<TCurrency> money, decimal divisor)
			where TResult : Money<TCurrency>, new()
		{
			AssertNotNull(money, "money");
			AssertNotNull(money.Amount, "money.Amount");

			TResult result = new TResult();
			result.Amount = money.Amount / divisor;
			result.Currency = money.Currency;
			return result;
		}

		/// <summary>
		/// Vypo�te pod�l ��stek. Nap�. pro v�po�et pom�ru ��stek, mar�e, apod.
		/// </summary>
		public static decimal DivideMoney(Money<TCurrency> dividend, Money<TCurrency> divisor)			
		{
			AssertNotNull(dividend, "dividend");
			AssertNotNull(dividend.Amount, "dividend.Amount");

			AssertNotNull(divisor, "divisor");
			AssertNotNull(divisor.Amount, "divisor.Amount");			
			
			AssertSameCurrencies(dividend.Currency, divisor.Currency);

			decimal result = dividend.Amount.Value / divisor.Amount.Value;
			return result;
		}

		#endregion
	}	
}

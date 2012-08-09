using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// T��da reprezentuj�c� ��stku a m�nu.
	/// T��da je ur�ena ke zd�d�n�, potomkem by m�la b�t projektov� t��da Money.
	/// </summary>
	public abstract class MoneyImplementationBase<TCurrency, TResult>: Money<TCurrency>
		where TCurrency: class
		where TResult : MoneyImplementationBase<TCurrency, TResult>, new()
	{
		#region Oper�tory +, -, *, /
		/// <summary>
		/// Se�te dv� hodnoty Money. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		public static TResult operator +(MoneyImplementationBase<TCurrency, TResult> money1, MoneyImplementationBase<TCurrency, TResult> money2)
		{
			return SumMoney<TResult>(money1, money2);
		}

		/// <summary>
		/// Ode�te dv� hodnoty Money. Pokud se neshoduje m�na, operace vyvol� v�jimku.
		/// </summary>
		/// <returns></returns>
		public static TResult operator -(MoneyImplementationBase<TCurrency, TResult> money1, MoneyImplementationBase<TCurrency, TResult> money2)
		{
			return SubtractMoney<TResult>(money1, money2);
		}

		/// <summary>
		/// Vyn�sob� hodnotu Money konstantou typu decimal.
		/// </summary>
		public static TResult operator *(MoneyImplementationBase<TCurrency, TResult> money, decimal multiplicand)
		{
			return MultipleMoney<TResult>(money, multiplicand);
		}

		/// <summary>
		/// Vyn�sob� hodnotu Money konstantou typu int.
		/// </summary>
		public static TResult operator *(MoneyImplementationBase<TCurrency, TResult> money, int multiplicand)
		{
			return MultipleMoney<TResult>(money, multiplicand);
		}


		/// <summary>
		/// Vyd�l� hodnotu Money konstantou typu decimal.
		/// </summary>
		public static TResult operator /(MoneyImplementationBase<TCurrency, TResult> money, decimal multiplicand)
		{
			return DivideMoney<TResult>(money, multiplicand);
		}

		/// <summary>
		/// Vyd�l� hodnotu Money konstantou typu int.
		/// </summary>
		public static TResult operator /(MoneyImplementationBase<TCurrency, TResult> money, int multiplicand)
		{
			return DivideMoney<TResult>(money, multiplicand);
		}
		#endregion
		
	}
}

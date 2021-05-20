﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Havit.Collections;
using Havit.Diagnostics.Contracts;

namespace Havit.Linq
{
	/// <summary>
	/// Extension metody pro IQueryable&lt;T&gt;.
	/// </summary>
	public static class QueryableExt
	{
		/// <summary>
		/// Volitelně rozšíří dotaz dle source o podmínku dle predicate, pokud condition je true.
		/// Pokud je condition false, vrátí nezměněný dotaz.
		/// </summary>
		/// <typeparam name="TSource">Typ objektu v source.</typeparam>
		/// <param name="source">Rozšiřovaný dotaz.</param>
		/// <param name="condition">Podmínka určující, zda má být dotaz rozšířen o podmínku.</param>
		/// <param name="predicate">Podmínka, která volitelně rozšiřuje dotaz.</param>
		/// <returns>Dotaz volitelně rozšířený o podmínku.</returns>
		public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
		{
			return condition
				? source.Where(predicate)
				: source;
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending/descending order according to a key and direction.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <param name="direction">Direction of the order.</param>
		/// <returns> An System.Linq.IOrderedQueryable`1 whose elements are sorted according to a key and direction.</returns>
		public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortDirection direction)
		{
			if (direction == SortDirection.Descending)
			{
				return source.OrderByDescending(keySelector);
			}
			else
			{
				return source.OrderBy(keySelector);
			}
		}

		/// <summary>
		/// Performs a subsequent ordering of the elements in a sequence in ascending/descending order according to a key and direction.
		/// </summary>
		/// <typeparam name="TSource">An System.Linq.IOrderedQueryable`1 that contains elements to sort.</typeparam>
		/// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <param name="direction">Direction of the order.</param>
		/// <returns>An System.Linq.IOrderedQueryable`1 whose elements are sorted according to a key and direction.</returns>
		public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortDirection direction)
		{
			if (direction == SortDirection.Descending)
			{
				return source.ThenByDescending(keySelector);
			}
			else
			{
				return source.ThenBy(keySelector);
			}
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending/descending order according to the sort items.
		/// Sort items expressions are mapped to expressions via a mapping functions.
		/// When sortItems is null or empty, the method returns unchanged sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="sortItems">Sorting items to apply on the source sequence.</param>
		/// <param name="mappingFunc">Mapping function from sort item expression to an expression tree.</param>
		/// <returns> An System.Linq.IQueryable`1 whose elements are sorted according to the sortItems.</returns>
		public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, IEnumerable<SortItem> sortItems, Func<string, Expression<Func<TSource, object>>> mappingFunc)
		{
			Contract.Requires<ArgumentNullException>(source != null, nameof(source));
			Contract.Requires<ArgumentNullException>(mappingFunc != null, nameof(mappingFunc));

			IOrderedQueryable<TSource> sourceWithOrdering = null;
			if (sortItems != null)
			{
				foreach (SortItem sortItem in sortItems)
				{
					var expression = mappingFunc(sortItem.Expression);
					sourceWithOrdering = sourceWithOrdering?.ThenBy(expression, sortItem.Direction)
						?? source.OrderBy(expression, sortItem.Direction);
				}
			}
			return sourceWithOrdering ?? source;
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending/descending order according to the sort items.
		/// Sort items expressions are mapped to expressions via a mapping function.
		/// When sortItems is null or empty, the method returns unchanged sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">A sequence of values to order.</param>
		/// <param name="sortItems">Sorting items to apply on the source sequence.</param>
		/// <param name="mappingFunc">Mapping function from sort item expression to a list of expression trees (<code>OrderBy</code> + <code>ThenBy</code>s).</param>
		/// <returns> An System.Linq.IQueryable`1 whose elements are sorted according to the sortItems.</returns>
		public static IQueryable<TSource> OrderByMultiple<TSource>(this IQueryable<TSource> source, IEnumerable<SortItem> sortItems, Func<string, List<Expression<Func<TSource, object>>>> mappingFunc)
		{
			Contract.Requires<ArgumentNullException>(source != null, nameof(source));
			Contract.Requires<ArgumentNullException>(mappingFunc != null, nameof(mappingFunc));

			IOrderedQueryable<TSource> sourceWithOrdering = null;
			if (sortItems != null)
			{
				foreach (SortItem sortItem in sortItems)
				{
					var expressions = mappingFunc(sortItem.Expression);
					foreach (var expression in expressions)
					{
						sourceWithOrdering = sourceWithOrdering?.ThenBy(expression, sortItem.Direction)
							?? source.OrderBy(expression, sortItem.Direction);
					}
				}
			}
			return sourceWithOrdering ?? source;
		}
	}
}

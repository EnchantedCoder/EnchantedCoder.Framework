using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nku testuj�c� referen�n� hodnotu (ciz� kl��).
	/// </summary>
	public static class ReferenceCondition
	{
		#region CreateEquals
		/// <summary>
		/// Vytvo�� podm�nku na rovnost reference.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, int? id)
		{

			if (id == BusinessObjectBase.NoID)
			{
				throw new ArgumentException("ID objektu nesm� m�t hodnotu BusinessObjectBase.NoID.", "id");
			}

			if (id == null)
			{
				return NullCondition.CreateIsNull(operand);
			}
			else
			{
				return NumberCondition.CreateEquals(operand, id.Value);
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku na rovnost reference.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, BusinessObjectBase businessObject)
		{
			if (businessObject == null)
			{
				return CreateEquals(operand, (int?)null);
			}

			if (businessObject.IsNew)
			{
				throw new ArgumentException("Nelze vyhled�vat podle nov�ho neulo�en�ho objektu.", "businessObject");
			}

			return CreateEquals(operand, businessObject.ID);
		}

		/// <summary>
		/// Vytvo�� podm�nku na rovnost reference.
		/// </summary>
		public static Condition CreateEquals(IOperand operand1, IOperand operand2)
		{
			return NumberCondition.CreateEquals(operand1, operand2);
		}
		
		#endregion

		#region CreateNotEquals
		/// <summary>
		/// Vytvo�� podm�nku na nerovnost reference.
		/// </summary>
		public static Condition CreateNotEquals(IOperand operand, int? id)
		{

			if (id == BusinessObjectBase.NoID)
			{
				throw new ArgumentException("ID objektu nesm� m�t hodnotu BusinessObjectBase.NoID.", "id");
			}

			if (id == null)
			{
				return NullCondition.CreateIsNotNull(operand);
			}
			else
			{
				return NumberCondition.Create(operand, ComparisonOperator.NotEquals, id.Value);
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku na nerovnost reference.
		/// </summary>
		public static Condition CreateNotEquals(IOperand operand, BusinessObjectBase businessObject)
		{
			if (businessObject == null)
			{
				return CreateNotEquals(operand, (int?)null);
			}

			if (businessObject.IsNew)
			{
				throw new ArgumentException("Nelze vyhled�vat podle nov�ho neulo�en�ho objektu.", "businessObject");
			}

			return CreateNotEquals(operand, businessObject.ID);
		}

		/// <summary>
		/// Vytvo�� podm�nku na nerovnost reference.
		/// </summary>
		public static Condition CreateNotEquals(IOperand operand1, IOperand operand2)
		{
			return NumberCondition.Create(operand1, ComparisonOperator.NotEquals, operand2);
		}
		
		#endregion

		#region CreateIn
		/// <summary>
		/// Vytvo�� podm�nku existence hodnoty v poli ID objekt�.
		/// </summary>
		public static Condition CreateIn(IOperand operand, int[] ids)
		{
			return CreateIn(operand, ids, MatchListMode.IntArray);
		}

		/// <summary>
		/// Vytvo�� podm�nku existence hodnoty v poli ID objekt�.
		/// </summary>
		/// <param name="operand">Testovan� operand.</param>
		/// <param name="ids">Seznam hodnot. Ov��ue se, �e hodnota operandu je mezi t�mito hodnotami.</param>
		/// <param name="mode">Zp�sob tvo�en� dotazu.</param>
		/// <returns>Podm�nka testuj�c� existenci hodnoty v poli ID objekt�.</returns>
		public static Condition CreateIn(IOperand operand, int[] ids, MatchListMode mode)
		{
			switch (mode)
			{
				case MatchListMode.IntArray:
					if (ids.Length < 2000)
					{
						return new BinaryCondition("{0} IN (SELECT Value FROM dbo.IntArrayToTable({1}))", operand, SqlInt32ArrayOperand.Create(ids));
					}
					else
					{
						OrCondition result = new OrCondition();
						int startIndex = 0;
						while (startIndex < ids.Length)
						{
							int length = Math.Min(ids.Length - startIndex, 1999);
							
							int[] subarray = new int[length];
							Array.Copy(ids, startIndex, subarray, 0, length);
							result.Conditions.Add(ReferenceCondition.CreateIn(operand, subarray));
							startIndex += length;
						}
						return result;
					}
				case MatchListMode.CommaSeparatedList:
					if (ids.Length == 0)
					{
						return StaticCondition.CreateFalse();
					}
					else
					{
						string condition = String.Format("{{0}} IN ({0})",
							String.Join(",", Array.ConvertAll<int, string>(ids,
								delegate(int item)
								{
									return item.ToString();
								}))
							);
						return new UnaryCondition(condition, operand);
					}
				default:
					throw new InvalidOperationException("Nezn�m� hodnota MatchListMode.");
			}
		}
		#endregion

		#region CreateNotIn
		/// <summary>
		/// Vytvo�� podm�nku neexistence hodnoty v poli ID objekt�.
		/// </summary>
		public static Condition CreateNotIn(IOperand operand, int[] ids)
		{
			return CreateNotIn(operand, ids, MatchListMode.IntArray);
		}

		/// <summary>
		/// Vytvo�� podm�nku neexistence hodnoty v poli ID objekt�.
		/// </summary>
		/// <param name="operand">Testovan� operand.</param>
		/// <param name="ids">Seznam hodnot. Ov��ue se, �e hodnota operandu nen� mezi t�mito hodnotami.</param>
		/// <param name="mode">Zp�sob tvo�en� dotazu.</param>
		/// <returns>Podm�nka testuj�c� neexistenci hodnoty v poli ID objekt�.</returns>
		public static Condition CreateNotIn(IOperand operand, int[] ids, MatchListMode mode)
		{
			switch (mode)
			{
				case MatchListMode.IntArray:
					if (ids.Length < 2000)
					{
						return new BinaryCondition("{0} NOT IN (SELECT Value FROM dbo.IntArrayToTable({1}))", operand, SqlInt32ArrayOperand.Create(ids));
					}
					else
					{
						OrCondition result = new OrCondition();
						int startIndex = 0;
						while (startIndex < ids.Length)
						{
							int length = Math.Min(ids.Length - startIndex, 1999);

							int[] subarray = new int[length];
							Array.Copy(ids, startIndex, subarray, 0, length);
							result.Conditions.Add(ReferenceCondition.CreateNotIn(operand, subarray));
							startIndex += length;
						}
						return result;
					}
				case MatchListMode.CommaSeparatedList:
					if (ids.Length == 0)
					{
						return StaticCondition.CreateTrue();
					}
					else
					{
						string condition = String.Format("{{0}} NOT IN ({0})",
							String.Join(",", Array.ConvertAll<int, string>(ids,
								delegate(int item)
								{
									return item.ToString();
								}))
							);
						return new UnaryCondition(condition, operand);
					}
				default:
					throw new InvalidOperationException("Nezn�m� hodnota MatchListMode.");
			}
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Conditions
{
	/// <summary>
	/// Pomocn�k pro pr�ci s v��tem ComparisonOperator.
	/// </summary>
	public static class ComparisonOperatorHelper
	{
		/// <summary>
		/// P�evede comparison oper�tor na �et�zec, nap�. Equals na "=", NotEquals na "&lt;&gt;", apod.
		/// </summary>
		public static string GetOperatorText(ComparisonOperator comparsionOperator)
		{
			switch (comparsionOperator)
			{
				case ComparisonOperator.Equals:
					return "=";
				case ComparisonOperator.NotEquals:
					return "<>";
				case ComparisonOperator.GreaterOrEquals:
					return ">=";
				case ComparisonOperator.Greater:
					return ">";
				case ComparisonOperator.Lower:
					return "<";
				case ComparisonOperator.LowerOrEquals:
					return "<=";
				default:
					throw new ArgumentException("Nezn�m� hodnota ComparisonOperator.", "comparsionOperator");
			}
		}

	}
}

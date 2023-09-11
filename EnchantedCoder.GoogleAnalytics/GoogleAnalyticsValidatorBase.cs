using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.GoogleAnalytics.ValueValidators;

namespace EnchantedCoder.GoogleAnalytics
{
	internal abstract class GoogleAnalyticsValidatorBase<TModel>
	{
		protected virtual IEnumerable<GoogleAnalyticsValidationResult> ValidateInternal(TModel model)
		{
			foreach (var property in model.GetType().GetProperties())
			{
				if (!ValueRequiredValidator.Validate(property.GetValue(model), property))
				{
					yield return new GoogleAnalyticsValidationResult
					{
						MemberName = property.Name,
						Message = "Value is required"
					};
				}
			}
		}
	}
}

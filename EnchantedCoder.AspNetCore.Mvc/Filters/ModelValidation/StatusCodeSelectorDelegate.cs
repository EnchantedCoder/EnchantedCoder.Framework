using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantedCoder.AspNetCore.Mvc.Filters.ModelValidation
{
	/// <summary>
	/// Určí stavový kódu odpovědi.
	/// </summary>
	public delegate int StatusCodeSelectorDelegate(ModelStateDictionary modelStateDictionary);
}
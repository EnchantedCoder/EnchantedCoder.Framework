using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantedCoder.AspNetCore.Mvc.Filters.ModelValidation
{
	/// <summary>
	/// Vrací odpověď v případě nevalidního ModelState.
	/// </summary>
	public delegate object ResultSelectorDelegate(int statusCode, ModelStateDictionary modelStateDictionary);
}
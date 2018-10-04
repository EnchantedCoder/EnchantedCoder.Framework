using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Havit.GoPay.DataObjects
{
	/// <summary>
	/// B�zov� t��da reprezentuj�c� po�adavek pro GoPay API
	/// </summary>
	public abstract class GoPayRequestBase
	{
		/// <summary>
		/// Access token pro ov��en� autorizace po�adavku
		/// </summary>
		[JsonIgnore]
		public string AccessToken { get; }

		/// <summary>
		/// Nastaven� access tokenu pro ov��en� autorizace po�adavku
		/// </summary>
		/// <param name="accessToken">Access token pro ov��en� autorizace po�adavku</param>
		protected GoPayRequestBase(string accessToken)
		{
			Contract.Requires(!String.IsNullOrEmpty(accessToken));
			AccessToken = accessToken;
		}
	}
}
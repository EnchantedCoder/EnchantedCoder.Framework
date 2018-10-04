using System;

namespace Havit.Data.TransientErrorHandling
{
	/// <summary>
	/// Z�sk�v� informaci o tom, zda m� b�t pokus o proveden� dan� akce v p��pad� ne�sp�chu opakov�n.
	/// </summary>
	internal interface IRetryPolicy
	{
		/// <summary>
		/// Vrac� informaci o tom, jestli m� b�t proveden dal�� pokus a s jak�m odstupem.
		/// </summary>
		RetryPolicyInfo GetRetryPolicyInfo(int attemptNumber, Exception exception);
	}
}
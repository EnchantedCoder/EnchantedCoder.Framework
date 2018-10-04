using Newtonsoft.Json;

namespace Havit.GoPay.DataObjects
{
	/// <summary>
	/// N�vratov� a notifika�n� URL
	/// </summary>
	public class GoPayCallback
	{
		/// <summary>
		/// URL adresa pro n�vrat na eshop
		/// </summary>
		[JsonProperty("return_url")]
		public string ReturnUrl { get; set; }

		/// <summary>
		/// URL adresa pro odesl�n� asynchronn� notifikace v p��pad� zm�ny stavu platby
		/// </summary>
		[JsonProperty("notification_url")]
		public string NotificationUrl { get; set; }
	}
}
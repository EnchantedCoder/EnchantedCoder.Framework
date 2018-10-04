using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Havit.GoPay.Codebooks;
using Havit.GoPay.DataObjects;
using Havit.GoPay.DataObjects.Errors;
using Newtonsoft.Json;

namespace Havit.GoPay
{
	/// <summary>
	/// GoPay client
	/// </summary>
	public class GoPayClient : IGoPayClient
	{
		internal const string TokenUrl = "oauth2/token";
		internal const string PaymentUrl = "payments/payment";
		internal const string PaymentUrlWithIdFormat = "payments/payment/{0}";
		internal const string RefundPaymentUrlFormat = "payments/payment/{0}/refund";
		internal const string CreateRecurrentPaymentUrlFormat = "payments/payment/{0}/create-recurrence";
		internal const string CancelRecurrentPaymentUrlFormat = "payments/payment/{0}/void-recurrence";
		internal const string CancelPreauthorizedPaymentUrlFormat = "payments/payment/{0}/void-authorization";
		internal const string CapturePaymentUrlFormat = "payments/payment/{0}/capture";
		internal const string AllowedPaymentMethodsUrlFormat = "eshops/eshop/{0}/payment-instruments";

		private readonly HttpClient httpClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="GoPayClient" /> class with <see cref="HttpClient" />.
		/// </summary>
		/// <param name="httpClient">Http client</param>
		/// <exception cref="ArgumentNullException">Thrown when instance of the <see cref="HttpClient" /> (or its <see cref="Uri">BaseAddress</see>) is missing.</exception>
		public GoPayClient(HttpClient httpClient)
		{
			if (httpClient == null)
			{
				throw new ArgumentNullException(nameof(httpClient));
			}

			if (httpClient.BaseAddress == null)
			{
				throw new InvalidOperationException("HttpClient nem� nastavenu BaseAddress.");
			}

			this.httpClient = httpClient;

			if (!this.httpClient.BaseAddress.AbsoluteUri.EndsWith("/"))
			{
				this.httpClient.BaseAddress = new Uri(this.httpClient.BaseAddress.AbsoluteUri + '/');
			}
		}

		/// <summary>
		/// Z�sk�n� p��stupov�ho tokenu
		/// </summary>
		/// <param name="clientId">Identifik�tor klienta</param>
		/// <param name="clientSecret">Tajn� k�d klienta</param>
		/// <param name="scope">Scope</param>
		/// <returns>GoPayResponse</returns>
		public virtual GoPayResponse GetToken(string clientId, string clientSecret, GoPayPaymentScope scope)
		{
			string stringScope;
			switch (scope)
			{
				case GoPayPaymentScope.PaymentCreate:
					stringScope = "payment-create";
					break;
				case GoPayPaymentScope.PaymentAll:
					stringScope = "payment-all";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
			}

			return SendPost(TokenUrl, clientId, clientSecret, new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "client_credentials"),
				new KeyValuePair<string, string>("scope", stringScope)
			}));
		}

		/// <summary>
		/// Vytvo�en� platby
		/// </summary>
		/// <param name="request">Request</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse CreatePayment(GoPayRequest request)
		{
			string jsonRequest = JsonConvert.SerializeObject(request);
			return SendPost(PaymentUrl, request.AccessToken, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
		}

		/// <summary>
		/// Z�sk�n� platby
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse GetPayment(long paymentId, string accessToken)
		{
			return SendGet(String.Format(PaymentUrlWithIdFormat, paymentId), accessToken);
		}

		/// <summary>
		/// Vr�cen� platby
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="amount">��stka</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse RefundPayment(long paymentId, long amount, string accessToken)
		{
			return SendPost(String.Format(RefundPaymentUrlFormat, paymentId), accessToken, new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("amount", amount.ToString())
			}));
		}

		/// <summary>
		/// CreateRecurrentPaymentOnDemand
		/// </summary>
		/// <param name="onDemandPaymentId">onDemandPaymentId</param>
		/// <param name="request">request</param>
		/// <returns>GoPayResponse</returns>
		public virtual GoPayResponse CreateRecurrentPaymentOnDemand(long onDemandPaymentId, GoPayRequest request)
		{
			string jsonRequest = JsonConvert.SerializeObject(request);
			return SendPost(String.Format(CreateRecurrentPaymentUrlFormat, onDemandPaymentId), request.AccessToken, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
		}

		/// <summary>
		/// CancelRecurrentPayment
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse CancelRecurrentPayment(long paymentId, string accessToken)
		{
			return SendPost(String.Format(CancelRecurrentPaymentUrlFormat, paymentId), accessToken);
		}

		/// <summary>
		/// CancelPreauthorizedPayment
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse CancelPreauthorizedPayment(long paymentId, string accessToken)
		{
			return SendPost(String.Format(CancelPreauthorizedPaymentUrlFormat, paymentId), accessToken);
		}

		/// <summary>
		/// CapturePayment
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse CapturePayment(long paymentId, string accessToken)
		{
			return SendPost(String.Format(CapturePaymentUrlFormat, paymentId), accessToken);
		}

		/// <summary>
		/// CapturePaymentAsync
		/// </summary>
		/// <param name="paymentId">Identifik�tor platby</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public async Task<GoPayResponse> CapturePaymentAsync(long paymentId, string accessToken)
		{
			return await SendPostAsync(String.Format(CapturePaymentUrlFormat, paymentId), accessToken);
		}

		/// <summary>
		/// GetAllowedPaymentMethods
		/// </summary>
		/// <param name="goId">goId</param>
		/// <param name="accessToken">P��stupov� token</param>
		/// <returns>GoPayResponse</returns>
		public GoPayResponse GetAllowedPaymentMethods(long goId, string accessToken)
		{
			return SendGet(String.Format(AllowedPaymentMethodsUrlFormat, goId), accessToken);
		}

		private GoPayResponse SendGet(string apiPartialUrl, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			return SendGetInternal(apiPartialUrl);
		}

		private GoPayResponse SendPost(string apiPartialUrl, string clientId, string clientSecret, HttpContent content = null)
		{
			string authorizationHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret));
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeaderValue);
			return SendPostInternal(apiPartialUrl, content);
		}

		private GoPayResponse SendPost(string apiPartialUrl, string accessToken, HttpContent content = null)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			return SendPostInternal(apiPartialUrl, content);
		}

		private async Task<GoPayResponse> SendPostAsync(string apiPartialUrl, string accessToken, HttpContent content = null)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			return await SendPostInternalAsync(apiPartialUrl, content);
		}

		private GoPayResponse SendGetInternal(string apiPartialUrl)
		{
			HttpResponseMessage message = httpClient.GetAsync(apiPartialUrl).Result;
			GoPayResponse response = ProcessResponse(message);
			return response;
		}

		private GoPayResponse SendPostInternal(string apiPartialUrl, HttpContent content = null)
		{
			HttpResponseMessage message = httpClient.PostAsync(apiPartialUrl, content).Result;
			GoPayResponse response = ProcessResponse(message);
			return response;
		}

		private async Task<GoPayResponse> SendPostInternalAsync(string apiPartialUrl, HttpContent content = null)
		{
			HttpResponseMessage message = await httpClient.PostAsync(apiPartialUrl, content);
			GoPayResponse response = await ProcessResponseAsync(message);
			return response;
		}

		private static GoPayResponse ProcessResponse(HttpResponseMessage result)
		{
			GoPayResponse response;
			using (Stream responseStream = result.Content.ReadAsStreamAsync().Result)
			using (TextReader textReader = new StreamReader(responseStream))
			{
				string resultStringValue = textReader.ReadToEnd();

				try
				{
					response = JsonConvert.DeserializeObject<GoPayResponse>(resultStringValue);
				}
				catch (Exception ex)
				{
					throw new GoPayResponseException($"Unable to process GoPay response: {resultStringValue}", ex);
				}
				
			}
			return response;
		}

		private static async Task<GoPayResponse> ProcessResponseAsync(HttpResponseMessage result)
		{
			GoPayResponse response;
			using (Stream responseStream = await result.Content.ReadAsStreamAsync())
			using (TextReader textReader = new StreamReader(responseStream))
			{
				string resultStringValue = textReader.ReadToEnd();

				try
				{
					response = JsonConvert.DeserializeObject<GoPayResponse>(resultStringValue);
				}
				catch (Exception ex)
				{
					throw new GoPayResponseException($"Unable to process GoPay response: {resultStringValue}", ex);
				}

			}
			return response;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			httpClient?.Dispose();
		}
	}
}
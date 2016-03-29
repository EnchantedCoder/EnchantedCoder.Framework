﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Management;

namespace Havit.Web.Management
{
	/// <summary>
	/// EventProvider pro healtmonitoring pro odeslání chyb pomocí Smtp serveru (třída SmtpClient) inspirovaný System.Web.Management.SimpleMailWebEventProvider.
	/// Na rozdíl od System.Web.Management.SimpleMailWebEventProvider nepodporuje bufferování zpráv.
	/// Konfigurace je obdobná, podporovány jsou následující hodnoty:
	/// <list type="bullet">
	///		<item>
	///			<term>to</term>
	///			<description>Adresát emailu.</description>
	///		</item>
	///		<item>
	///			<term>from</term>
	///			<description>Odesílatem emailu.</description>
	///		</item>
	///		<item>
	///			<term>smtpServer</term>
	///			<description>Smtp server k odeslání zprávy.</description>
	///		</item>
	///		<item>
	///			<term>smtpUsername</term>
	///			<description>Username smtp serveru (není-li nastaveno, nepoužije se).</description>
	///		</item>
	///		<item>
	///			<term>smtpPassword</term>
	///			<description>Heslo smtp serveru.</description>
	///		</item>
	///		<item>
	///			<term>smtpEnableSsl</term>
	///			<description>Indikuje, zda se má použít SSL při připojení k smtp serveru. Výchozí hodnota je false.</description>
	///		</item>
	///		<item>
	///			<term>subjectPrefix</term>
	///			<description>Prefix do předmětu zasílaného emailu.</description>
	///		</item>
	/// </list>
	/// </summary>
	/// <exception cref="ConfigurationErrorsException">Při výskytu neznámé konfigurační hodnoty.</exception>
	/// <seealso cref="System.Web.Management.WebEventProvider" />
	public class SmtpWebEventProvider : WebEventProvider
	{
		private string _from;
		private string _to;
		private string _smtpServer;
		private string _smtpUsername;
		private string _smtpPassword;
		private bool? _smtpEnableSsl = false;
		private string _subjectPrefix;

		private int _mailCounter = 0;

		/// <summary>
		/// Načte hodnoty z konfigurace do privátních fieldů, zkontroluje neexistenci neznámé hodnoty v konfiguraci.
		/// </summary>
		public override void Initialize(string name, NameValueCollection config)
		{
			ProviderUtil.GetAndRemoveStringAttribute(config, "from", name, ref _from);
			ProviderUtil.GetAndRemoveStringAttribute(config, "to", name, ref _to);
			ProviderUtil.GetAndRemoveStringAttribute(config, "smtpServer", name, ref _smtpServer);
			ProviderUtil.GetAndRemoveStringAttribute(config, "smtpUsername", name, ref _smtpUsername);
			ProviderUtil.GetAndRemoveStringAttribute(config, "smtpPassword", name, ref _smtpPassword);			
			ProviderUtil.GetAndRemoveBooleanAttribute(config, "smtpEnableSsl", name, ref _smtpEnableSsl);
			ProviderUtil.GetAndRemoveStringAttribute(config, "subjectPrefix", name, ref _subjectPrefix);

			base.Initialize(name, config);

			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		/// <summary>
		/// Zpracuje událost - odešle ji emailem.
		/// </summary>
		public override void ProcessEvent(WebBaseEvent raisedEvent)
		{
			SendMail(raisedEvent);
		}

		/// <summary>
		/// Odešle událost emailem.
		/// </summary>
		/// <param name="raisedEvent">The raised event.</param>
		protected void SendMail(WebBaseEvent raisedEvent)
		{
			MailMessage mailMessage = GetMailMessage(raisedEvent);
			var smtpClient = GetSmtpClient();

			smtpClient.Send(mailMessage);
		}

		/// <summary>
		/// Vrací kompletně nakonfigurovanou instanci SmtpClient pro odeslání emailu.
		/// </summary>
		protected virtual SmtpClient GetSmtpClient()
		{
			SmtpClient smtpClient = new SmtpClient();
			if (!String.IsNullOrEmpty(_smtpServer))
			{
				smtpClient.Host = _smtpServer;
			}

			if (!String.IsNullOrEmpty(_smtpUsername))
			{
				smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
			}

			smtpClient.EnableSsl = _smtpEnableSsl.GetValueOrDefault(false);
			return smtpClient;
		}

		/// <summary>
		/// Vrací kompletně nakonfigurovanou MailMessage k odeslání.
		/// </summary>
		protected virtual MailMessage GetMailMessage(WebBaseEvent raisedEvent)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.BodyTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

			if (!String.IsNullOrEmpty(_to))
			{
				foreach (string _toAddress in _to.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					mailMessage.To.Add(_toAddress.Trim());
				}
			}

			if (!String.IsNullOrEmpty(_from))
			{
				mailMessage.From = new MailAddress(_from);
			}

			mailMessage.Subject = GetMailMessageSubject(raisedEvent);
			mailMessage.Body = GetMailMessageBody(raisedEvent);
			return mailMessage;
		}

		/// <summary>
		/// Vrací předmět pro odesílaný email.
		/// </summary>
		protected virtual string GetMailMessageSubject(WebBaseEvent raisedEvent)
		{
			string message = (raisedEvent is WebRequestErrorEventExt) ? ((WebRequestErrorEventExt)raisedEvent).ErrorException.Message : raisedEvent.Message;
			string prefix = _subjectPrefix.Trim().TrimEnd(':');
			int counter = Interlocked.Increment(ref _mailCounter);

			return String.Format("{0} (#{1})", String.IsNullOrEmpty(prefix) ? message : (prefix + ": " + message), counter);
		}

		/// <summary>
		/// Vrací body pro odesílaný email.
		/// </summary>
		protected virtual string GetMailMessageBody(WebBaseEvent raisedEvent)
		{
			return raisedEvent.ToString(true, true);
		}

		/// <summary>
		/// Performs tasks associated with shutting down the provider.
		/// </summary>
		/// <remarks>Implementace nedělá nic.</remarks>
		public override void Shutdown()
		{
			// NOOP
		}

		/// <summary>
		/// Moves the events from the provider's buffer into the event log.
		/// </summary>
		/// <remarks>Implementace nedělá nic.</remarks>
		public override void Flush()
		{
			// NOOP
		}
	}
}

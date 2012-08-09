using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Security;
using System.Web;
using System.Security.Principal;

namespace Havit.Web.Security
{
	/// <summary>
	/// Poskytuje statick� metody pro snadnou implementaci FormAuthentication, kdy jsou do role ukl�d�ny do ticketu jako userData.
	/// </summary>
	/// <remarks>
	/// Implementov�no v�hradn� pro cookies-authentizaci. Nepodporuje cookieless!
	/// </remarks>
	public static class FormsRolesAuthentication
	{
		#region Timeout
		/// <summary>
		/// Timeout pro authentication-ticket (web.config: system.web/authentication/forms/timeout).
		/// </summary>
		/// <remarks>
		/// Jako jedna z m�la konfigura�n�ch parametr� nen� p��stupn� p�es <see cref="System.Web.Security.FormsAuthentication"/>.
		/// </remarks>
		public static int Timeout
		{
			get
			{
				if (_timeout == null)
				{
					AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
					_timeout = (int)authenticationSection.Forms.Timeout.TotalMinutes; // pokud nen� konfigurov�no, vrac� default
				}
				return (int)_timeout;
			}
		}
		private static int? _timeout;
		#endregion

		#region ApplyAuthenticationTicket
		/// <summary>
		/// Aplikuje autentiza�n� ticket, tj. vyt�hne z n�j informace o p�ihl�en�m u�ivateli
		/// a jeho rol�ch a napln� jimi objekt User.
		/// </summary>
		/// <remarks>
		/// Vyt�hne z authentication-ticketu role, vytvo�� z n�j identity, spoj� to v principal a ten nastav� jako aktu�ln�ho u�ivatele.
		/// </remarks>
		/// <exception cref="ArgumentNullException">pokud je <c>ticket</c> null</exception>
		/// <param name="ticket">authentication-ticket</param>
		public static void ApplyAuthenticationTicket(FormsAuthenticationTicket ticket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("ticket");
			}
			
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new InvalidOperationException("HttpContext.Current not available");
			}

			string[] roles = ticket.UserData.Split(',');
			for (int i = 0; i < roles.Length; i++)
			{
				roles[i] = roles[i].Trim();
			}

			FormsIdentity identity = new FormsIdentity(ticket);
			GenericPrincipal principal = new GenericPrincipal(identity, roles);
			context.User = principal;
		}

		/// <summary>
		/// Aplikuje p��padn� autentiza�n� ticket, tj. vyt�hne z n�j informace o p�ihl�en�m u�ivateli
		/// a jeho rol�ch a napln� jimi objekt User.
		/// </summary>
		/// <remarks>
		/// Autentiza�n� ticket se pokou�� zjistit ve form� cookie a decryptovat. V p��pad� nalezen� ho aplikuje.
		/// </remarks>
		public static void ApplyAuthenticationTicket()
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new InvalidOperationException("HttpContext.Current not available");
			}

			HttpCookie authenticationCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authenticationCookie != null)
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authenticationCookie.Value);
				ApplyAuthenticationTicket(ticket);
			}
		}
		#endregion

		#region GetAuthTicket
		/// <summary>
		/// Vytvo�� autentiza�n� ticket pro forms-authentication s ukl�d�n�m rol� do userData.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistent"><c>true</c>, pokud se m� b�t ticket persistentn�; jinak <c>false</c></param>
		/// <param name="cookiePath">cookie-path pro autentiza�n� ticket</param>
		/// <returns>autentiza�n� ticket na z�klad� p�edan�ch argument�</returns>
		public static FormsAuthenticationTicket GetAuthTicket(string username, string[] roles, bool createPersistent, string cookiePath)
		{
			FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
				2,											// version
				username,									// name
				DateTime.Now,								// issueDate
				DateTime.Now.AddMinutes((double)Timeout),	// expiration
				createPersistent,						// isPersistent
				String.Join(",", roles),					// userData
				cookiePath);								// cookiePath
			return authTicket;
		}
		#endregion

		#region GetAuthCookie
		/// <summary>
		/// Vytvo�� authentiza�n� cookie pro forms-authentication s ukl�d�n�m rol� do userData.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistentCookie"><c>true</c>, pokud se m� vytvo�it trval� cookie, kter� p�e�ije session browseru; jinak <c>false</c></param>
		/// <param name="cookiePath">cookie-path pro autentiza�n� ticket</param>
		/// <returns></returns>
		public static HttpCookie GetAuthCookie(string username, string[] roles, bool createPersistentCookie, string cookiePath)
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new InvalidOperationException("HttpContext.Current not available");
			}

			if (username == null)
			{
				username = String.Empty;
			}

			if (String.IsNullOrEmpty(cookiePath))
			{
				cookiePath = FormsAuthentication.FormsCookiePath;
			}

			FormsAuthenticationTicket authTicket = GetAuthTicket(username, roles, createPersistentCookie, cookiePath);

			string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
			if (String.IsNullOrEmpty(encryptedTicket))
			{
				throw new HttpException("Unable to encrypt cookie for authentication ticket");
			}

			HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
			authCookie.HttpOnly = true;
			authCookie.Path = cookiePath;
			authCookie.Secure = FormsAuthentication.RequireSSL;
			if (FormsAuthentication.CookieDomain != null)
			{
				authCookie.Domain = FormsAuthentication.CookieDomain;
			}
			if (authTicket.IsPersistent)
			{
				authCookie.Expires = authTicket.Expiration;
			}

			return authCookie;
		}
		#endregion

		#region RedirectFromLoginPage
		/// <summary>
		/// Redirektuje autentizovan�ho u�ivatele zp�t na p�vodn� URL (nebo default URL).
		/// Sou��st� response je autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistentCookie"><c>true</c>, pokud se m� vytvo�it trval� cookie, kter� p�e�ije session browseru; jinak <c>false</c></param>
		/// <param name="cookiePath">cookie-path pro autentiza�n� ticket</param>
		/// <param name="redirectUrl">URL, na kter� m� b�t provedeno p�esm�rov�n�</param>
		public static void RedirectFromLoginPage(string username, string[] roles, bool createPersistentCookie, string cookiePath, string redirectUrl)
		{
			if (username != null)
			{
				HttpContext context = HttpContext.Current;
				if (context == null)
				{
					throw new InvalidOperationException("HttpContext.Current not available");
				}

				if (String.IsNullOrEmpty(redirectUrl))
				{
					redirectUrl = FormsAuthentication.GetRedirectUrl(username, createPersistentCookie);
				}

				AddAuthCookie(username, roles, createPersistentCookie, cookiePath);
				context.Response.Redirect(redirectUrl, false);
			}
		}

		/// <summary>
		/// Redirektuje autentizovan�ho u�ivatele zp�t na p�vodn� URL (nebo default URL).
		/// Sou��st� response je autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistentCookie"><c>true</c>, pokud se m� vytvo�it trval� cookie, kter� p�e�ije session browseru; jinak <c>false</c></param>
		public static void RedirectFromLoginPage(string username, string[] roles, bool createPersistentCookie)
		{
			RedirectFromLoginPage(username, roles, createPersistentCookie, null, null);
		}

		/// <summary>
		/// Redirektuje autentizovan�ho u�ivatele zp�t na p�vodn� URL (nebo default URL).
		/// Sou��st� response je autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem, bez persistence.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		public static void RedirectFromLoginPage(string username, string[] roles)
		{
			RedirectFromLoginPage(username, roles, false, null, null);
		}
		#endregion

		#region AddAuthCookie
		/// <summary>
		/// P�id� do Response autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistentCookie"><c>true</c>, pokud se m� vytvo�it trval� cookie, kter� p�e�ije session browseru; jinak <c>false</c></param>
		/// <param name="cookiePath">cookie-path pro autentiza�n� ticket</param>
		/// <returns>autnetiza�n� cookie, kter� byla vytvo�ena a p�id�na do Response</returns>
		public static HttpCookie AddAuthCookie(string username, string[] roles, bool createPersistentCookie, string cookiePath)
		{
			if (username != null)
			{
				HttpContext context = HttpContext.Current;
				if (context == null)
				{
					throw new InvalidOperationException("HttpContext.Current not available");
				}

				HttpCookie authCookie = GetAuthCookie(username, roles, createPersistentCookie, cookiePath);
				context.Response.Cookies.Add(authCookie);
				
				return authCookie;
			}
			return null;
		}

		/// <summary>
		/// P�id� do Response autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem. Cookie nen� persistentn�.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <returns>autentiza�n� cookie, kter� byla vytvo�ena a p�id�na do Response</returns>
		public static HttpCookie AddAuthCookie(string username, string[] roles)
		{
			return AddAuthCookie(username, roles, false, null);
		}

		/// <summary>
		/// P�id� do Response autentiza�n� cookie s p��slu�n�m autentiza�n�m ticketem.
		/// </summary>
		/// <param name="username">p�ihla�ovac� jm�no u�ivatele</param>
		/// <param name="roles">role, kter� u�ivateli p��slu��</param>
		/// <param name="createPersistentCookie"><c>true</c>, pokud se m� vytvo�it trval� cookie, kter� p�e�ije session browseru; jinak <c>false</c></param>
		/// <returns>autnetiza�n� cookie, kter� byla vytvo�ena a p�id�na do Response</returns>
		public static HttpCookie AddAuthCookie(string username, string[] roles, bool createPersistentCookie)
		{
			return AddAuthCookie(username, roles, createPersistentCookie, null);
		}
		#endregion
	}
}

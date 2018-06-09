using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebProj.Auth
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class BasicAuthenticationFilter : AuthorizationFilterAttribute
	{
		bool Active = true;

		public BasicAuthenticationFilter()
		{ }

		public BasicAuthenticationFilter(bool active)
		{
			Active = active;
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (Active)
			{
				var identity = ParseAuthorizationHeader(actionContext);
				if (identity == null)
				{
					Challenge(actionContext);
					return;
				}

				var principal = new GenericPrincipal(identity, null);

				Thread.CurrentPrincipal = principal;

				if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
				{
					//Challenge(actionContext);
					return;
				}

				// inside of ASP.NET this is required
				if (HttpContext.Current != null)
					HttpContext.Current.User = principal;

				base.OnAuthorization(actionContext);
			}
		}

		protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return false;

			return true;
		}

		protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
		{
			string authHeader = null;
			var auth = actionContext.Request.Headers.Authorization;
			if (auth != null && auth.Scheme == "Basic")
				authHeader = auth.Parameter;

			if (string.IsNullOrEmpty(authHeader))
				return null;

			authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

			var tokens = authHeader.Split(':');
			if (tokens.Length < 2)
				return null;

			return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
		}

		void Challenge(HttpActionContext actionContext)
		{
			var host = actionContext.Request.RequestUri.DnsSafeHost;
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
		}
	}

	public class BasicAuthenticationIdentity : GenericIdentity
	{

		public BasicAuthenticationIdentity(string name, string password) : base(name, "Basic")
		{
			this.Password = password;
		}

		public string Password { get; set; }
		public long UserId { get; set; }
	}
}
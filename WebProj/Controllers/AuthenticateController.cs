using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebProj.Auth;
using WebProj.BLL.BusinessLogic.Token.Services;

namespace WebProj.Controllers
{
	[MyBasicAuthenticationFilter(true)]
	public class AuthenticateController : ApiController
    {
		public ITokenService TokenService { get; set; }

		public AuthenticateController()
		{
			TokenService = (ITokenService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ITokenService));
		}

		[Route("login")]
		[Route("authenticate")]
		[Route("get/token")]
		public HttpResponseMessage Authenticate()
		{
			HttpResponseMessage responseMessage = null;
			if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
			{
				var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
				if (basicAuthenticationIdentity != null)
				{
					responseMessage = GetAuthToken(basicAuthenticationIdentity.UserId);
				}
			}

			return responseMessage;
		}

		private HttpResponseMessage GetAuthToken(long userId)
		{
			TokenService.AssingToken(userId);
			var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
			response.Headers.Add("Token", userId.ToString());
			response.Headers.Add("Access-Control-Expose-Headers", "Token");

			return response;
		}
	}
}

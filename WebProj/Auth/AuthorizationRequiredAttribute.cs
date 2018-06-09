using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebProj.BLL.BusinessLogic.Token.Services;

namespace WebProj.Auth
{
	public class AuthorizationRequiredAttribute : ActionFilterAttribute
	{
		private const string Token = "Token";

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var requestScope = actionContext.Request.GetDependencyScope();
			var service = requestScope.GetService(typeof(ITokenService)) as ITokenService;

			if (actionContext.Request.Headers.Contains(Token))
			{
				string tokenValue = actionContext.Request.Headers.GetValues(Token).First();
				long token;
				Int64.TryParse(tokenValue, out token);
				if (service != null && !service.ValidateToken(token))
				{
					var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
					actionContext.Response = responseMessage;
				}
			}
			else
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}

			base.OnActionExecuting(actionContext);
		}
	}
}
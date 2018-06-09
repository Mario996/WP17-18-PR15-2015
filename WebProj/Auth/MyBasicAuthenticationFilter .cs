using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using WebProj.BLL.BusinessLogic.Account.Services;
using WebProj.BLL.BusinessLogic.Token.Services;
using WebProj.BLL.Models.ViewModel;
using WebProj.Common.Models;

namespace WebProj.Auth
{
	public class MyBasicAuthenticationFilter : BasicAuthenticationFilter
	{
		public MyBasicAuthenticationFilter()
		{ }

		public MyBasicAuthenticationFilter(bool active) : base(active)
		{ }


		protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
		{
			bool retVal = false;
			var requestScope = actionContext.Request.GetDependencyScope();
			var resolveService = requestScope.GetService(typeof(IAccountService));
			var resolveService2 = requestScope.GetService(typeof(ITokenService));
			string report = string.Empty;
			var service = resolveService as IAccountService;

			actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
			if (service != null)
			{
				AppUserViewModel user = null;
				

				var userId = service.Authenticate(username, password, out report, out user);
				if (userId > 0)
				{
					var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
					if (basicAuthenticationIdentity != null)
					{
						basicAuthenticationIdentity.UserId = userId;
					}
					actionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
					actionContext.Response.Headers.Add("user", JsonConvert.SerializeObject(user));
					retVal =  true;
					(resolveService2 as ITokenService).AssingToken(userId);
				}
				
			}
			
            actionContext.Response.Headers.Add("report", report);
			return retVal;
		}

		//internal bool Login(string username, string password, out string report, out AppUserViewModel user)
		//{
		//	bool retVal = false;
		//	report = string.Empty;
		//	user = null;
		//	//if (DataManager.DataManager.Instance.ValidateUserLoginData(username, password, out report, out user))
		//	//{
		//	//	BasicAuthenticationIdentity identity = new BasicAuthenticationIdentity(username, password);
		//	//	var principal = new GenericPrincipal(identity, null);

		//	//	Thread.CurrentPrincipal = principal;

		//	//	if (HttpContext.Current != null)
		//	//		HttpContext.Current.User = principal;

		//	//	retVal = true;
		//	//}
		//	return retVal;
		//}

		internal void Logout(long id)
		{
			Thread.CurrentPrincipal = null;

			if (HttpContext.Current != null)
				HttpContext.Current.User = null;
		}
	}
}

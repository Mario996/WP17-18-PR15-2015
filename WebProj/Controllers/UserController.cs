using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Services;
using WebProj.Auth;
using WebProj.Common;
using WebProj.Common.Models;
using WebProj.BLL.Models.ViewModel;
using WebProj.BLL.BusinessLogic.User.Services;
using System.Text;
using WebProj.BLL.BusinessLogic.Token.Services;

namespace WebProj.Controllers
{

	public class UserController : ApiController
	{
		public IUserService UserService { get; set; }

		List<string> genders = new List<string>();
		public UserController()
		{
			foreach (var t in Enum.GetValues(typeof(Gender)))
			{
				genders.Add(t.ToString());
			}
			UserService = (IUserService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUserService));
		}

		[MyBasicAuthenticationFilter(false)]
		[HttpGet]
		public IHttpActionResult GetGenders()
		{
			return Ok(JsonConvert.SerializeObject(genders));
		}

		[AuthorizationRequired]
		[HttpPost]
		public IHttpActionResult GetUserData(long id)
		{
			User user = UserService.GetUserById(id);
			object retVal = new { User = user };
			if (user is Driver)
			{
				Driver d = user as Driver;
				Location l = DataManager.DataManager.Instance.GetById<Location>(d.Location);
                if (l != null)
				{
					Address a = DataManager.DataManager.Instance.GetById<Address>(l.Address);
					if (a != null)
					{
						retVal = new { User = user, Location = l, Address = a };
					}
				}
			}

			string rsp = JsonConvert.SerializeObject(retVal);
			return Ok(rsp);
		}


		[MyBasicAuthenticationFilter(false)]
		[HttpPost]
		public IHttpActionResult Login(JObject inputData)
		{
			JToken t;
			string username = string.Empty;
			if (inputData.TryGetValue("username", out t))
			{
				username = t.ToString();
			}
			string password = string.Empty;
			if (inputData.TryGetValue("password", out t))
			{
				password = t.ToString();
			}

			MyBasicAuthenticationFilter auth = new MyBasicAuthenticationFilter(true);

			string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

			this.ActionContext.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", token);
			auth.OnAuthorization(this.ActionContext);

			if (this.ActionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
			{
				return new NotFoundWithMessageResult(GetHeaderItem("report"));
			}

			string json = GetHeaderItem("user");
			HttpContext.Current.Response.Headers.Add("loggedUser", json);
			//HttpContext.Current.Response.Headers.Add("token", token);
			return Ok();
		}

		private string GetHeaderItem(string headerItem)
		{
			string retVal = string.Empty;
			IEnumerable<string> values;
			if (this.ActionContext.Response.Headers.TryGetValues(headerItem, out values))
			{
				retVal = values.First();
			}
			return retVal;
		}

		[HttpPost]
		public IHttpActionResult Logout(long id)
		{
			MyBasicAuthenticationFilter auth = new MyBasicAuthenticationFilter();
			auth.Logout(id);
			var requestScope = this.ActionContext.Request.GetDependencyScope();
			var resolveService = requestScope.GetService(typeof(ITokenService));
			(resolveService as ITokenService).Kill(id);
			return Ok();
		}


		[MyBasicAuthenticationFilter(false)]
		[HttpPost]
		public IHttpActionResult RegisterNewUser(JObject inputData)
		{
			JToken t;
			string username = string.Empty;
			if (inputData.TryGetValue("username", out t))
			{
				username = t.ToString();
			}
			string password = string.Empty;
			if (inputData.TryGetValue("password", out t))
			{
				password = t.ToString();
			}
			string firstName = string.Empty;
			if (inputData.TryGetValue("firstName", out t))
			{
				firstName = t.ToString();
			}
			string lastName = string.Empty;
			if (inputData.TryGetValue("lastName", out t))
			{
				lastName = t.ToString();
			}
			string gender = string.Empty;
			Gender genderEnum = Gender.MALE;
			if (inputData.TryGetValue("gender", out t))
			{
				gender = t.ToString();
				Enum.TryParse(gender, out genderEnum);
			}
			string jmbg = string.Empty;
			if (inputData.TryGetValue("jmbg", out t))
			{
				jmbg = t.ToString();
			}
			string phoneNumber = string.Empty;
			if (inputData.TryGetValue("phoneNumber", out t))
			{
				phoneNumber = t.ToString();
			}
			string emailAddress = string.Empty;
			if (inputData.TryGetValue("emailAddress", out t))
			{
				emailAddress = t.ToString();
			}
			UserType role = UserType.CUSTOMER;
			if (inputData.TryGetValue("role", out t))
			{
				Enum.TryParse(t.ToString(), out role);
			}

			User user = CreateUser(username, password, firstName, lastName, genderEnum, jmbg, phoneNumber, emailAddress, role);
			string report;
			if (!UserService.RegisterUser(user, out report))
			{
				return new NotFoundWithMessageResult(report);
			}

			return Ok();
		}

		private User CreateUser(string username, string password, string firstName, string lastName, Gender genderEnum, string jmbg, string phoneNumber, string emailAddress, UserType role)
		{
			User user;
			if (role == UserType.DRIVER)
			{
				user = new Driver(username, password, firstName, lastName, genderEnum, jmbg, phoneNumber, emailAddress, role);
			}
			else
			{
				user = new User(username, password, firstName, lastName, genderEnum, jmbg, phoneNumber, emailAddress, role);
			}

			return user;
		}

		[AuthorizationRequired]
		[HttpPut]
		public IHttpActionResult UpdateUser(long id, JObject inputData)
		{
			IHttpActionResult retVal = Ok();

			JToken t;
			string password = string.Empty;
			if (inputData.TryGetValue("password", out t))
			{
				password = t.ToString();
			}
			string firstName = string.Empty;
			if (inputData.TryGetValue("firstName", out t))
			{
				firstName = t.ToString();
			}
			string lastName = string.Empty;
			if (inputData.TryGetValue("lastName", out t))
			{
				lastName = t.ToString();
			}
			string gender = string.Empty;
			Gender genderEnum = Gender.MALE;
			if (inputData.TryGetValue("gender", out t))
			{
				gender = t.ToString();
				Enum.TryParse(gender, out genderEnum);
			}
			string jmbg = string.Empty;
			if (inputData.TryGetValue("jmbg", out t))
			{
				jmbg = t.ToString();
			}
			string phoneNumber = string.Empty;
			if (inputData.TryGetValue("phoneNumber", out t))
			{
				phoneNumber = t.ToString();
			}
			string emailAddress = string.Empty;
			if (inputData.TryGetValue("emailAddress", out t))
			{
				emailAddress = t.ToString();
			}
			string s = string.Empty;
			bool blocked = false;
			if (inputData.TryGetValue("blocked", out t))
			{
				s = t.ToString();
				Boolean.TryParse(s, out blocked);
			}

			User user = UserService.GetUserById(id);

			user.Password = password;
			user.FirstName = firstName;
			user.LastName = lastName;
			user.Gender = genderEnum;
			user.Idnumber = jmbg;
			user.PhoneNumber = phoneNumber;
			user.EmailAddress = emailAddress;
			user.Blocked = blocked;

			UserService.UpdateUser(user);

			return retVal;
		}
	}
}

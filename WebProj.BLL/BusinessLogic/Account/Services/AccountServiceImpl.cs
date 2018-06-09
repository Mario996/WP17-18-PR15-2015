using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Account.Services
{
	public class AccountServiceImpl : IAccountService
	{
		public long Authenticate(string username, string password, out string report, out AppUserViewModel user)
		{
			Model.User loggedInUser;
			bool retVal = DataManager.DataManager.Instance.ValidateUserLoginData(username, password, out report, out loggedInUser);
			user = retVal ? new AppUserViewModel(loggedInUser) : null;
			return retVal ? user.Id : 0;
		}
	}
}

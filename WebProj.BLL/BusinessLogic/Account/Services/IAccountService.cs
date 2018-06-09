using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Account.Services
{
	public interface IAccountService
	{
		long Authenticate(string username, string password, out string report, out AppUserViewModel user);
	}
}

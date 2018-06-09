using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.User.Services
{
	public interface IUserService
	{
		bool RegisterUser(Model.User user, out string report);
		Model.User GetUserById(long id);

		void UpdateUser(Model.User user);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.User.Services
{
	public class UserServiceImpl : IUserService
	{
		public Model.User GetUserById(long id)
		{
			Model.User user;
			if ((EntityType)(id >> 48) == EntityType.USER)
			{
				user = DataManager.DataManager.Instance.GetById<Model.User>(id);
			}
			else
			{
				user = DataManager.DataManager.Instance.GetById<Model.Driver>(id);
			}

			return user;
		}

		public bool RegisterUser(Model.User user, out string report)
		{
			bool retVal = false;
			report = "Username must be unique";
            if (user.UserRole == UserType.DRIVER)
			{
				if (DataManager.DataManager.Instance.ValidateInputData<Model.Driver>(user as Model.Driver))
				{
					DataManager.DataManager.Instance.AddNewEntity<Model.Driver>(user as Model.Driver);
					retVal = true;
				}
			}
			else
			{
				if (DataManager.DataManager.Instance.ValidateInputData<Model.User>(user))
				{
					DataManager.DataManager.Instance.AddNewEntity<Model.User>(user);
					retVal = true;
				}
			}
			return retVal;
		}

		public void UpdateUser(Model.User user)
		{
			long id = user.Id;
			if ((EntityType)(id >> 48) == EntityType.USER)
			{
				DataManager.DataManager.Instance.UpdateEntity<Model.User>(user);
			}
			else
			{
				DataManager.DataManager.Instance.UpdateEntity<Model.Driver>(user as Model.Driver);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Token.Services
{
	public class TokenServiceImpl : ITokenService
	{
		List<long> activeTokens = new List<long>();

		public void AssingToken(long userId)
		{
			activeTokens.Add(userId);
		}

		public bool Kill(long userId)
		{
			bool retVal = false;
			if (activeTokens.Contains(userId))
			{
				activeTokens.Remove(userId);
				retVal = true;
			}
			return retVal;
		}

		public bool ValidateToken(long userId)
		{
			Model.User user;
			if ((EntityType)(userId >> 48) == EntityType.USER)
			{
				user = DataManager.DataManager.Instance.GetById<Model.User>(userId);
			}
			else
			{
				user = DataManager.DataManager.Instance.GetById<Model.Driver>(userId);
			}

			return user != null && activeTokens.Contains(user.Id);
		}
	}
}

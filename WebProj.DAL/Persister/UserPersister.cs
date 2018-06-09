using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class UserPersister : Persister<User>
	{
		public UserPersister() : base("users.txt")
		{
		}
		protected override User InstantiateEntity(string[] splited)
		{
			return new User(splited);
		}

		protected override bool ValidateInput(User entity)
		{
			foreach (var user in Collection.Values)
			{
				if (user.UserName.CompareTo(entity.UserName) == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}

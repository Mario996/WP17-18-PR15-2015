using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class DriverPersister : Persister<Driver>
	{
		public DriverPersister() : base("drivers.txt")
		{
		}
		protected override Driver InstantiateEntity(string[] splited)
		{
			return new Driver(splited);
		}

		protected override bool ValidateInput(Driver entity)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class AddressPersister : Persister<Address>
	{
		public AddressPersister() :base("addresses.txt")
		{
			//this.path = "addresses.txt";
		}
		protected override Address InstantiateEntity(string[] splited)
		{
			return new Address(splited);
		}

		protected override bool ValidateInput(Address entity)
		{
			throw new NotImplementedException();
		}
	}
}
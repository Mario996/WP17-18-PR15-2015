using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class FarePersister : Persister<Fare>
	{
		public FarePersister() : base("fares.txt")
		{
		}
		protected override Fare InstantiateEntity(string[] splited)
		{
			return new Fare(splited);
		}

		protected override bool ValidateInput(Fare entity)
		{
			return entity.DateOfDrive > DateTime.Now && entity.StartLocation != 0; 
		}
	}
}
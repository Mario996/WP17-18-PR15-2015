using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class LocationPersister : Persister<Location>
	{
		public LocationPersister() : base("locations.txt")
		{
			//this.path = "locations.txt";
		}

		protected override Location InstantiateEntity(string[] splited)
		{
			return new Location(splited);
		}

		protected override bool ValidateInput(Location entity)
		{
			throw new NotImplementedException();
		}
	}
}

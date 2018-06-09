using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public class VehiclePersister : Persister<Vehicle>
	{
		public VehiclePersister() : base("vehicles.txt")
		{
		}
		protected override Vehicle InstantiateEntity(string[] splited)
		{
			return new Vehicle(splited);
		}

		protected override bool ValidateInput(Vehicle entity)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using WebProj.Common.Models;

namespace WebProj.BLL.Models.ViewModel
{
	public class VehicleViewModel : ViewModel
	{
		public string Driver { get; set; }
		public int YearOfProduction { get; set; }
		public string Licence { get; set; }
		public int TaxiId{ get; set; }
		public VehicleType VehicleType{ get; set; }
		public string VehicleTypeString { get; set; }

		public VehicleViewModel(Vehicle vehicle)
		{
			YearOfProduction = vehicle.YearOfProduction;
			Licence = vehicle.Registration;
			TaxiId = vehicle.TaxiId;
			VehicleType = vehicle.VehicleType;
			VehicleTypeString = VehicleType.ToString();
			var driver = DataManager.DataManager.Instance.GetById<Driver>(vehicle.Driver);

			Driver = $"{driver.FirstName} {driver.LastName}";
		}
	}
}

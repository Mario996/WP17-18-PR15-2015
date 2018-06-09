using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Driver.Service
{
	public class DriverService : IDriverService
	{
		public IEnumerable<Common.Models.Driver> GetFreeDrivers()
		{
			return DataManager.DataManager.Instance.GetAllByType<Model.Driver>().Where(d => !d.Busy);
		}

		public bool RegisterDriver(string username, string password, string firstName, string lastName, Gender genderEnum, string jmbg, string phoneNumber, string emailAddress, UserType role, int year, string plate, int taxiId, VehicleType type, out string report)
		{
			bool retVal = false;
			report = string.Empty;
			try
			{
				Model.Driver driver = new Model.Driver(username, password, firstName, lastName, genderEnum, jmbg, phoneNumber, emailAddress, role);
				Model.Vehicle vehicle = new Model.Vehicle(year, plate, taxiId, type);
				driver.Vehicle = vehicle.Id;
				vehicle.Driver = driver.Id;
				if (DataManager.DataManager.Instance.ValidateInputData<Model.Driver>(driver))
				{
					DataManager.DataManager.Instance.AddNewEntity<Model.Driver>(driver);
					DataManager.DataManager.Instance.AddNewEntity<Model.Vehicle>(vehicle);
					retVal = true;
				}
				//[CKP] dodaj poruku o losem user nameu
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}

			return retVal;
		}

		public bool UpdateDriverLocation(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long id, out string report)
		{
			bool retVal = false;
			report = string.Empty;
			try
			{
				var driver = DataManager.DataManager.Instance.GetById<Model.Driver>(id);
				var location = DataManager.DataManager.Instance.GetRelatedEntity<Model.Driver, Model.Location>(id, "Location");
				if (location == null)
				{
					Model.Location loc = new Model.Location(locX, locY);
					Model.Address addr = new Model.Address(addrStreet, addrNumber, addrCity, addrPostalCode);
					loc.Address = addr.Id;
					driver.Location = loc.Id;

					DataManager.DataManager.Instance.AddNewEntity<Model.Location>(loc);
					DataManager.DataManager.Instance.AddNewEntity<Model.Address>(addr);
					DataManager.DataManager.Instance.UpdateEntity<Model.Driver>(driver);

				}
				else
				{
					location.X = locX;
					location.Y = locY;
					var address = DataManager.DataManager.Instance.GetById<Model.Address>(location.Address);
					if (address != null)
					{
						address.Street = addrStreet;
						address.Number = addrNumber;
						address.City = addrCity;
						address.PostalCode = addrPostalCode;
						DataManager.DataManager.Instance.UpdateEntity<Model.Address>(address);
					}
					else
					{
						address = new Model.Address(addrStreet, addrNumber, addrCity, addrPostalCode);
						location.Address = address.Id;
						DataManager.DataManager.Instance.AddNewEntity<Model.Address>(address);
					}

					DataManager.DataManager.Instance.UpdateEntity<Model.Location>(location);
				}
				retVal = true;
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;

		}
	}
}

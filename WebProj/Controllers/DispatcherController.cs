using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebProj.Common;
using WebProj.Common.Models;
using WebProj.BLL.Models.ViewModel;
using WebProj.Auth;
using WebProj.BLL.BusinessLogic.Driver.Service;
using WebProj.BLL.BusinessLogic.Vehicle.Service;
using WebProj.BLL.BusinessLogic.Fare.Sevices;

namespace WebProj.Controllers
{
	[AuthorizationRequired]
	public class DispatcherController : ApiController
	{
		List<string> vehicleTypes = new List<string>();
		List<string> fareCustomerStatuses = new List<string>(2) { FareStatus.ON_HOLD.ToString(), FareStatus.CANCELED.ToString() };
		List<string> fareStatuses = new List<string>();
		IDriverService DriverService;
		IVehicleService VehicleService;
		IFareService FareService;

		public DispatcherController()
		{
			foreach (var t in Enum.GetValues(typeof(VehicleType)))
			{
				vehicleTypes.Add(t.ToString());
			}
			foreach (var t in Enum.GetValues(typeof(FareStatus)))
			{
				fareStatuses.Add(t.ToString());
			}

			DriverService = (IDriverService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDriverService));
			VehicleService = (IVehicleService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IVehicleService));
			FareService = (IFareService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFareService));
		}
		[HttpPost]
		public IHttpActionResult RegisterDriver(JObject inputData)
		{
			JToken t;
			string username = string.Empty;
			if (inputData.TryGetValue("username", out t))
			{
				username = t.ToString();
			}
			string password = string.Empty;
			if (inputData.TryGetValue("password", out t))
			{
				password = t.ToString();
			}
			string firstName = string.Empty;
			if (inputData.TryGetValue("firstName", out t))
			{
				firstName = t.ToString();
			}
			string lastName = string.Empty;
			if (inputData.TryGetValue("lastName", out t))
			{
				lastName = t.ToString();
			}
			Gender genderEnum = Gender.MALE;
			if (inputData.TryGetValue("gender", out t))
			{
				Enum.TryParse(t.ToString(), out genderEnum);
			}
			string jmbg = string.Empty;
			if (inputData.TryGetValue("jmbg", out t))
			{
				jmbg = t.ToString();
			}
			string phoneNumber = string.Empty;
			if (inputData.TryGetValue("phoneNumber", out t))
			{
				phoneNumber = t.ToString();
			}
			string emailAddress = string.Empty;
			if (inputData.TryGetValue("emailAddress", out t))
			{
				emailAddress = t.ToString();
			}
			UserType role = UserType.CUSTOMER;
			if (inputData.TryGetValue("role", out t))
			{
				Enum.TryParse(t.ToString(), out role);
			}
			int year = -1;
			if (inputData.TryGetValue("year", out t))
			{
				Int32.TryParse(t.ToString(), out year);
			}
			string plate = string.Empty;
			if (inputData.TryGetValue("plate", out t))
			{
				plate = t.ToString();
			}
			int taxiId = -1;
			if (inputData.TryGetValue("taxiId", out t))
			{
				Int32.TryParse(t.ToString(), out taxiId);
			}
			VehicleType type = VehicleType.PASSENGER_CAR;
			if (inputData.TryGetValue("type", out t))
			{
				Enum.TryParse(t.ToString(), out type);
			}
			string report = string.Empty;
			if (!DriverService.RegisterDriver(username, password, firstName, lastName, genderEnum, jmbg, phoneNumber, emailAddress, role, year, plate, taxiId, type, out report))
			{
				return new NotFoundWithMessageResult(report);
			}

			return Ok();
		}

		[HttpPost]
		public IHttpActionResult CreateFare(long id, JObject inputData)
		{
			JToken t;
			double locX = 0;
			if (inputData.TryGetValue("locX", out t))
			{
				Double.TryParse(t.ToString(), out locX);
			}
			double locY = 0;
			if (inputData.TryGetValue("locY", out t))
			{
				Double.TryParse(t.ToString(), out locY);
			}
			string addrStreet = string.Empty;
			if (inputData.TryGetValue("addrStreet", out t))
			{
				addrStreet = t.ToString();
			}
			int addrNumber = 0;
			if (inputData.TryGetValue("addrNumber", out t))
			{
				Int32.TryParse(t.ToString(), out addrNumber);
			}
			string addrCity = string.Empty;
			if (inputData.TryGetValue("addrCity", out t))
			{
				addrCity = t.ToString();
			}
			int addrPostalCode = 0;
			if (inputData.TryGetValue("addrPostalCode", out t))
			{
				Int32.TryParse(t.ToString(), out addrPostalCode);
			}

			int type = -1;
			if (inputData.TryGetValue("vehicleType", out t))
			{
				Int32.TryParse(t.ToString(), out type);
			}

			DateTime date = DateTime.Now;
			if (inputData.TryGetValue("date", out t))
			{
				DateTime.TryParse(t.ToString(), out date);
			}

			long driverId = 0;
			if (inputData.TryGetValue("driver", out t))
			{
				Int64.TryParse(t.ToString(), out driverId);
			}

			string report;
			if (!FareService.CreateDriverFare(locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode, id, type, date,driverId, out report))
			{
				return new NotFoundWithMessageResult(report);
			}

			return Ok();
		}

		public IHttpActionResult GetVehicleTypes()
		{
			return Ok(JsonConvert.SerializeObject(vehicleTypes));
		}
		public IHttpActionResult GetFareCustomerStatuses()
		{
			return Ok(JsonConvert.SerializeObject(fareCustomerStatuses));
		}
		public IHttpActionResult GetFareStatuses()
		{
			return Ok(JsonConvert.SerializeObject(fareStatuses));
		}

		public IEnumerable<AppUserViewModel> GetUsers()
		{
			var users = DataManager.DataManager.Instance.GetAllByType<User>();
			var drivers = DataManager.DataManager.Instance.GetAllByType<Driver>().Cast<User>();
			users.AddRange(drivers);
			List<AppUserViewModel> retVal = new List<AppUserViewModel>();
			foreach(var u in users)
			{
				retVal.Add(new AppUserViewModel(u));
			}
			return retVal;
		}

		public IEnumerable<VehicleViewModel> GetVehicles()
		{
			return VehicleService.GetAllVehicles();
		}

		public IEnumerable<FareViewModel> GetFares()
		{
			var models = DataManager.DataManager.Instance.GetAllByType<Fare>();
			List<FareViewModel> retVal = new List<FareViewModel>(models.Count);
			foreach (var m in models)
			{
				retVal.Add(new FareViewModel(m));
			}
			return retVal;
		}
		[HttpPost]
		public IHttpActionResult GetFare(long id)
		{
			var fareModel = DataManager.DataManager.Instance.GetById<Fare>(id);
			FareViewModel retVal = new FareViewModel(fareModel);

			return Ok(JsonConvert.SerializeObject(retVal));
		}

		public IEnumerable<Driver> GetFreeDrivers()
		{
			return DriverService.GetFreeDrivers();
		}

		public IEnumerable<FareViewModel> GetDispatcherFares(long id)
		{
			return FareService.GetDispatcherFares(id);
		}

		public IEnumerable<Driver> GetDrivers()
		{
			return DataManager.DataManager.Instance.GetAllByType<Driver>();
		}

		//public IHttpActionResult CreateFare(long id, JObject inputData)
		//{
		//	return Ok();
		//}

		[HttpPost]
		public IHttpActionResult GetUserDetails(long id)
		{
			string rspMsg;
			if ((EntityType)(id >> 48) == EntityType.USER)
			{
				var u = DataManager.DataManager.Instance.GetById<User>(id);
				rspMsg = JsonConvert.SerializeObject(u);
			}
			else
			{
				var u = DataManager.DataManager.Instance.GetById<Driver>(id);
				rspMsg = JsonConvert.SerializeObject(u);
			}

			return Ok(rspMsg);
		}

		public IHttpActionResult UpdateDispatcherFare(long id, JObject inputData)
		{
			JToken t;
			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}
			long driver= 0;
			if (inputData.TryGetValue("driver", out t))
			{
				Int64.TryParse(t.ToString(), out driver);
			}

			long fareid = 0;
			if (inputData.TryGetValue("fareid", out t))
			{
				Int64.TryParse(t.ToString(), out fareid);
			}

			string report = string.Empty;
			if (!FareService.UpdateDispatcherFare(id, status,driver, fareid, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		[HttpPost]
		public IHttpActionResult AddVehicle(JObject inputData)
		{
			JToken t;
			long driver = -1;
			if (inputData.TryGetValue("driver", out t))
			{
				Int64.TryParse(t.ToString(), out driver);
			}
			int year = -1;
			if (inputData.TryGetValue("year", out t))
			{
				Int32.TryParse(t.ToString(), out year);
			}

			int taxiId = -1;
			if (inputData.TryGetValue("taxiId", out t))
			{
				Int32.TryParse(t.ToString(), out taxiId);
			}
			string plate = string.Empty;
			if (inputData.TryGetValue("plate", out t))
			{
				plate = t.ToString();
			}

			VehicleType type = VehicleType.PASSENGER_CAR;
			if (inputData.TryGetValue("type", out t))
			{
				Enum.TryParse(t.ToString(), out type);
			}

			Vehicle v = new Vehicle(year, plate, taxiId, type);
			var d = DataManager.DataManager.Instance.GetById<Driver>(driver);
			if (d != null)
			{
				d.Vehicle = v.Id;
				v.Driver = driver;
				DataManager.DataManager.Instance.UpdateEntity<Driver>(d);
			}

			DataManager.DataManager.Instance.AddNewEntity<Vehicle>(v);

			return Ok();
		}
	}
}

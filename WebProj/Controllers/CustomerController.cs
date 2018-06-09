using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebProj.BLL.BusinessLogic.Fare.Sevices;
using WebProj.BLL.Models.ViewModel;
using WebProj.Common;
using WebProj.Common.Models;

namespace WebProj.Controllers
{
    public class CustomerController : ApiController
    {
		IFareService FareService;

		public CustomerController()
		{
			FareService = (IFareService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFareService));
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

			VehicleType type = 0;
			if (inputData.TryGetValue("vehicleType", out t))
			{
				Enum.TryParse(t.ToString(), out type);
			}

			DateTime date = DateTime.Now;
			if (inputData.TryGetValue("date", out t))
			{
				DateTime.TryParse(t.ToString(), out date);
			}

			string report;
			if(!FareService.CreateCustomerFare(locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode, id, (int)type, date, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		public IEnumerable<FareViewModel> GetCustomerFares(long id)
		{
			return FareService.GetUserFares(id);
		}

		public IHttpActionResult UpdateCustomerFareRate(long id, JObject inputData)
		{
			JToken t;
			string comment = string.Empty;
			if (inputData.TryGetValue("comment", out t))
			{
				comment = t.ToString();
			}
			int rate = 0;
			if (inputData.TryGetValue("rate", out t))
			{
				Int32.TryParse(t.ToString(), out rate);
			}

			string report;
			if(!FareService.CommnetFare(id, comment, rate, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		[HttpPost]
		public IEnumerable<FareViewModel> GetFilterCustomerFares(long id, JObject inputData)
		{
			JToken t;
			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}
			return FareService.GetFilerCustomerFares(id, status);
		}

		public IHttpActionResult UpdateCustomerFare(long id, JObject inputData)
		{
			JToken t;
			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}

			string  comment = string.Empty;
			if (inputData.TryGetValue("comment", out t))
			{
				comment = t.ToString();
			}
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

			VehicleType type = 0;
			if (inputData.TryGetValue("vehicleType", out t))
			{
				Enum.TryParse(t.ToString(), out type);
			}

			DateTime date = DateTime.Now;
			if (inputData.TryGetValue("date", out t))
			{
				DateTime.TryParse(t.ToString(), out date);
			}

			string report = string.Empty;
			if (!FareService.UpdateCustomerFare(id, locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode, (int)type, date,status, comment, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}
	}
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebProj.BLL.BusinessLogic.Driver.Service;
using WebProj.BLL.BusinessLogic.Fare.Sevices;
using WebProj.BLL.Models.ViewModel;
using WebProj.Common;
using WebProj.Common.Models;

namespace WebProj.Controllers
{
	public class DriverController : ApiController
	{
		IFareService FareService;
		IDriverService DriverService;
        public DriverController()
		{
			FareService = (IFareService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFareService));
			DriverService = (IDriverService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDriverService));
		}

		[HttpPut]
		public IHttpActionResult UpdateLocation(long id, JObject inputData)
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

			string report = string.Empty;
			if(!DriverService.UpdateDriverLocation(locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode, id, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		public IEnumerable<FareViewModel> GetDriverFares(long id)
		{
			return FareService.GetUserFares(id);
		}

		public IEnumerable<FareViewModel> GetUnassignedFares(long id)
		{
			return FareService.GetUnassignedFares();
		}


		[HttpPost]
		public IEnumerable<FareViewModel> GetFilterDriverFares(long id, JObject inputData)
		{
			JToken t;
			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}
			return FareService.GetFilterDriverFares(id, status);
		}

		[HttpPost]
		public IHttpActionResult TakeFare(long id, JObject inputData)
		{
			JToken t;
			long fareId = -1;
			if (inputData.TryGetValue("fareId", out t))
			{
				Int64.TryParse(t.ToString(), out fareId);
			}

			string report = string.Empty;
			if (!FareService.TakeFare(fareId, id, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		public IHttpActionResult UpdateDriverFareFailed(long id, JObject inputData)
		{
			JToken t;
			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}
			string comment = string.Empty;
			if (inputData.TryGetValue("comment", out t))
			{
				comment = t.ToString();
			}
			string report = string.Empty;
			if (!FareService.UpdateDispatcherFareFailded(id, status, comment, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}

		[HttpPost]
		public IHttpActionResult UpdateDriverFare(long id, JObject inputData)
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

			double price = 0;
			if (inputData.TryGetValue("price", out t))
			{
				Double.TryParse(t.ToString(), out price);
			}

			FareStatus status = 0;
			if (inputData.TryGetValue("status", out t))
			{
				Enum.TryParse(t.ToString(), out status);
			}

			string report = string.Empty;
			if (!FareService.UpdateDispatcherFare(id, locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode, price, status, out report))
			{
				return new NotFoundWithMessageResult(report);
			}
			return Ok();
		}
	}
}

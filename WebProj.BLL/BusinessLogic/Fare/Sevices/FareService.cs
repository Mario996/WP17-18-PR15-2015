using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;
using WebProj.Common;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Fare.Sevices
{
	public class FareService : IFareService
	{
		public bool CreateCustomerFare(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long customer, int type, DateTime date, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				Model.Location loc = new Model.Location(locX, locY);
				Model.Address addr = new Model.Address(addrStreet, addrNumber, addrCity, addrPostalCode);
				loc.Address = addr.Id;
				Model.Fare fare = new Model.Fare(loc.Id, customer, type, date);
				var user = DataManager.DataManager.Instance.GetById<Model.User>(customer);

				if (DataManager.DataManager.Instance.ValidateInputData<Model.Fare>(fare))
				{
					user.Fares.Add(fare.Id);
					DataManager.DataManager.Instance.AddNewEntity<Model.Location>(loc);
					DataManager.DataManager.Instance.AddNewEntity<Model.Address>(addr);
					DataManager.DataManager.Instance.AddNewEntity<Model.Fare>(fare);
					DataManager.DataManager.Instance.UpdateEntity<Model.User>(user);
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				report = ex.Message;
			}
			return retVal;
		}

		// ne treba driverId
		public IEnumerable<FareViewModel> GetUnassignedFares()
		{
			List<FareViewModel> retVal = new List<FareViewModel>();
			var fareModels = DataManager.DataManager.Instance.GetAllByType<Model.Fare>().Where(fare => fare.FareStatus == FareStatus.ON_HOLD);

			foreach (var f in fareModels)
			{
				retVal.Add(new FareViewModel(f));
			}
			return retVal;
		}

		public IEnumerable<FareViewModel> GetUserFares(long customerId)
		{
			List<long> fareIds = GetRelatedFaresIds(customerId);
			var fareModels = DataManager.DataManager.Instance.GetAllByType<Model.Fare>().Where(f => fareIds.Contains(f.Id));
			List<FareViewModel> retVal = new List<FareViewModel>();
			foreach (var f in fareModels)
			{
				retVal.Add(new FareViewModel(f));
			}
			return retVal;
		}

		List<long> GetRelatedFaresIds(long id)
		{
			EntityType t = (EntityType)(id >> 48);
			if (t == EntityType.USER)
			{
				return new List<long>(DataManager.DataManager.Instance.GetById<Model.User>(id).Fares);
			}
			if (t == EntityType.DRIVER)
			{
				return new List<long>(DataManager.DataManager.Instance.GetById<Model.Driver>(id).Fares);
			}
			return new List<long>();
		}

		public bool TakeFare(long fareId, long driverId, out string report)
		{
			bool retVal = false;
			report = string.Empty;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(fareId);
				var driver = DataManager.DataManager.Instance.GetById<Model.Driver>(driverId);
				driver.Busy = true;
				fare.FareStatus = FareStatus.ACCEPTED;
				fare.Driver = driverId;
				driver.Fares.Add(fare.Id);
				DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
				DataManager.DataManager.Instance.UpdateEntity<Model.Driver>(driver);
				retVal = true;

			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public bool UpdateCustomerFare(long id,
			double locX, double locY,
			string addrStreet, int addrNumber, string addrCity, int addrPostalCode,
			int type, DateTime date, FareStatus status, string comment, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(id);
				if (fare != null)
				{
					fare.FareStatus = status;
					fare.DesiredVehicleType = (VehicleType)type;
					fare.DateOfDrive = date;
					Model.Comment fareComment;
					if (fare.Comment != 0)
					{
						fareComment = DataManager.DataManager.Instance.GetById<Model.Comment>(fare.Comment);
						if (fareComment != null)
						{
							fareComment.Description = comment;
							fareComment.DateOfPublish = DateTime.Now;
							DataManager.DataManager.Instance.UpdateEntity<Model.Comment>(fareComment);
						}
					}
					else
					{
						fareComment = new Model.Comment(comment);
						fareComment.Fare = fare.Id;
						fare.Comment = fareComment.Id;
						DataManager.DataManager.Instance.AddNewEntity<Model.Comment>(fareComment);
						DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					}

					var location = DataManager.DataManager.Instance.GetById<Model.Location>(fare.StartLocation);
					if (location != null)
					{
						location.X = locX;
						location.Y = locY;
						DataManager.DataManager.Instance.UpdateEntity<Model.Location>(location);
						var address = DataManager.DataManager.Instance.GetById<Model.Address>(location.Address);
						if (address != null)
						{
							address.Street = addrStreet;
							address.Number = addrNumber;
							address.PostalCode = addrPostalCode;
							address.City = addrCity;
							DataManager.DataManager.Instance.UpdateEntity<Model.Address>(address);
						}
					}
					DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public bool UpdateDispatcherFare(long id, double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, double price, FareStatus status, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(id);
				if (fare != null)
				{
					fare.FareStatus = status;
					fare.Amount = price;
					Model.Location fareFinishLocation = new Model.Location(locX, locY);
					Model.Address fareFinishAddress = new Model.Address(addrStreet, addrNumber, addrCity, addrPostalCode);
					fareFinishLocation.Address = fareFinishAddress.Id;
					fare.FinishLocation = fareFinishLocation.Id;
					DataManager.DataManager.Instance.AddNewEntity<Model.Location>(fareFinishLocation);
					DataManager.DataManager.Instance.AddNewEntity<Model.Address>(fareFinishAddress);
					DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public bool CreateDriverFare(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long dispatherId, int vehicleType, DateTime date, long driverId, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				Model.Location loc = new Model.Location(locX, locY);
				Model.Address addr = new Model.Address(addrStreet, addrNumber, addrCity, addrPostalCode);
				loc.Address = addr.Id;
				Model.Fare fare = new Model.Fare();
				fare.StartLocation = loc.Id;
				fare.Dispather = dispatherId;
				if (vehicleType > 0)
				{
					fare.DesiredVehicleType = (VehicleType)vehicleType;
				}
				fare.DateOfDrive = date;
				fare.Driver = driverId;
				fare.FareStatus = FareStatus.FORMED;

				var driver = DataManager.DataManager.Instance.GetById<Model.Driver>(driverId);

				if (DataManager.DataManager.Instance.ValidateInputData<Model.Fare>(fare))
				{
					driver.Fares.Add(fare.Id);
					driver.Busy = true;
					DataManager.DataManager.Instance.AddNewEntity<Model.Location>(loc);
					DataManager.DataManager.Instance.AddNewEntity<Model.Address>(addr);
					DataManager.DataManager.Instance.AddNewEntity<Model.Fare>(fare);
					DataManager.DataManager.Instance.UpdateEntity<Model.Driver>(driver);
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				report = ex.Message;
			}
			return retVal;
		}

		public bool UpdateDispatcherFare(long id, FareStatus status, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(id);
				if (fare != null)
				{
					fare.FareStatus = status;
					DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public bool CommnetFare(long id, string comment, int rate, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(id);
				var commentModel = DataManager.DataManager.Instance.GetRelatedEntity<Model.Fare, Model.Comment>(id, "Comment");
				if (commentModel != null)
				{
					commentModel.Description = comment;
					commentModel.Mark = rate;
					commentModel.DateOfPublish = DateTime.Now;

					DataManager.DataManager.Instance.UpdateEntity<Model.Comment>(commentModel);
					retVal = true;
				}
				else
				{
					commentModel = new Common.Models.Comment();
					commentModel.Description = comment;
					commentModel.Mark = rate;
					commentModel.DateOfPublish = DateTime.Now;
					commentModel.Fare = id;
					fare.Comment = commentModel.Id;
					DataManager.DataManager.Instance.AddNewEntity<Model.Comment>(commentModel);
					DataManager.DataManager.Instance.UpdateEntity<Model.Fare >(fare);
					retVal = true;
				}

			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public IEnumerable<FareViewModel> GetFilerCustomerFares(long id, FareStatus status)
		{
			var fareModels = DataManager.DataManager.Instance.GetAllByType<Model.Fare>().Where(f => f.Customer == id && f.FareStatus == status);
			List<FareViewModel> retVal = new List<FareViewModel>();
			foreach (var model in fareModels)
			{
				retVal.Add(new FareViewModel(model));
			}

			return retVal;
		}

		public IEnumerable<FareViewModel> GetFilterDriverFares(long id, FareStatus status)
		{
			var fareModels = DataManager.DataManager.Instance.GetAllByType<Model.Fare>().Where(f => f.Driver == id && f.FareStatus == status);
			List<FareViewModel> retVal = new List<FareViewModel>();
			foreach (var model in fareModels)
			{
				retVal.Add(new FareViewModel(model));
			}

			return retVal;
		}

		public IEnumerable<FareViewModel> GetDispatcherFares(long id)
		{
			var fareModels = DataManager.DataManager.Instance.GetAllByType<Model.Fare>().Where(f => f.Dispather == id);
			List<FareViewModel> retVal = new List<FareViewModel>();
			foreach (var f in fareModels)
			{
				retVal.Add(new FareViewModel(f));
			}
			return retVal;
		}

		public bool UpdateDispatcherFare(long id, FareStatus status, long driver, long fareid, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(fareid);
				var dispatcher = DataManager.DataManager.Instance.GetById<Model.User>(id);
				var driverModel = DataManager.DataManager.Instance.GetById<Model.Driver>(driver);

				if (fare != null && dispatcher != null && driverModel != null)
				{
					fare.Driver = driver;
					fare.Dispather = id;
					fare.FareStatus = status;
					dispatcher.Fares.Add(fareid);
					driverModel.Fares.Add(fareid);

					DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					DataManager.DataManager.Instance.UpdateEntity<Model.User>(dispatcher);
					DataManager.DataManager.Instance.UpdateEntity<Model.Driver>(driverModel);
				}
				retVal = false;
			}
			catch (Exception ex)
			{
				report = ex.Message;
				retVal = false;
			}
			return retVal;
		}

		public bool UpdateDispatcherFareFailded(long id, FareStatus status, string comment, out string report)
		{
			report = string.Empty;
			bool retVal = false;
			try
			{
				var fare = DataManager.DataManager.Instance.GetById<Model.Fare>(id);

				if (fare != null)
				{
					fare.FareStatus = status;

					Model.Comment fareComment;
					if (fare.Comment != 0)
					{
						fareComment = DataManager.DataManager.Instance.GetById<Model.Comment>(fare.Comment);
						if (fareComment != null)
						{
							fareComment.Description = comment;
							fareComment.DateOfPublish = DateTime.Now;
							DataManager.DataManager.Instance.UpdateEntity<Model.Comment>(fareComment);
						}
					}
					else
					{
						fareComment = new Model.Comment(comment);
						fareComment.Fare = fare.Id;
						fare.Comment = fareComment.Id;
						DataManager.DataManager.Instance.AddNewEntity<Model.Comment>(fareComment);
						//DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
					}


					DataManager.DataManager.Instance.UpdateEntity<Model.Fare>(fare);
				}
				retVal = false;
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

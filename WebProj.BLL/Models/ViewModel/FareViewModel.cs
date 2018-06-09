using System;
using WebProj.Common;
using WebProj.Common.Models;

namespace WebProj.BLL.Models.ViewModel
{
	public class FareViewModel : ViewModel
	{
		public string CustomerFirstName { get; set; }
		public string CustomerLastName { get; set; }

		public string Customer { get; set; }

		public string DispatcherFirstName { get; set; }
		public string DispatcherLastName { get; set; }
		public string Dispatcher { get; set; }

		public string DriverFirstName { get; set; }
		public string DriverLastName { get; set; }
		public string Driver { get; set; }

		public string StartLocation { get; set; }
		public string FinishLocation { get; set; }

		public DateTime DateOfDrive { get; set; }
		public string DateTimeString
		{
			get { return DateOfDrive.ToString("yyyy-MM-ddThh:mm"); }
		}

		public VehicleType DesiredVehicleType { get; set; }

		public string VehicleType { get { return DesiredVehicleType == 0 ? "N/A" : DesiredVehicleType.ToString(); } }
		public FareStatus Status { get; set; }

		public string StatusString { get { return Status.ToString(); } }

		public double Price { get; set; }
		public string Comment { get; set; }

		public Comment CommentObject { get; set; }
		public Location StartLocationObject { get; private set; }
		public Address StartAddressObject { get; private set; }

		public FareViewModel(Fare fare) : base(fare.Id)
		{
			var customer = DataManager.DataManager.Instance.GetById<User>(fare.Customer);
			Customer = "N/A";
			if (customer != null)
			{
				CustomerFirstName = customer.FirstName;
				CustomerLastName = customer.LastName;
				Customer = $"{CustomerFirstName} {CustomerLastName}";
			}

			var driver = DataManager.DataManager.Instance.GetById<Driver>(fare.Driver);
			Driver = "N/A";
			if (driver != null)
			{
				DriverFirstName = driver.FirstName;
				DriverLastName = driver.LastName;
				Driver = $"{DriverFirstName} {DriverLastName}";
			}

			var dispather = DataManager.DataManager.Instance.GetById<User>(fare.Dispather);
			Dispatcher = "N/A";
			if (dispather != null)
			{
				DispatcherFirstName = dispather.FirstName;
				DispatcherLastName = dispather.LastName;
				Dispatcher = $"{DispatcherFirstName} {DispatcherLastName}";
			}

			DesiredVehicleType = fare.DesiredVehicleType;

			DateOfDrive = fare.DateOfDrive;

			var startaddress = DataManager.DataManager.Instance.GetRelatedEntity<Location, Address>(fare.StartLocation, "Address");
			if (startaddress != null)
			{
				StartLocation = $"{startaddress.Street} {startaddress.Number}, {startaddress.PostalCode}, {startaddress.City}";
			}
			StartLocationObject = DataManager.DataManager.Instance.GetById<Location>(fare.StartLocation);
			StartAddressObject = DataManager.DataManager.Instance.GetById<Address>(StartLocationObject.Address);

			var finishLocation = DataManager.DataManager.Instance.GetRelatedEntity<Location, Address>(fare.FinishLocation, "Address");
			FinishLocation = "N/A";
            if (finishLocation != null)
			{
				FinishLocation = $"{finishLocation.Street} {finishLocation.Number}, {finishLocation.PostalCode}, {finishLocation.City}";
			}

			Status = fare.FareStatus;

			Price = fare.Amount;

			var comment = DataManager.DataManager.Instance.GetById<Comment>(fare.Comment);
			if (comment != null)
			{
				Comment = comment.Description;
				CommentObject = comment;
			}

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Vehicle : Entity
	{
		private long driver;
		private int yearOfProduction;
		private string registration;
		private int taxiId;
		private VehicleType vehicleType;

		public int YearOfProduction
		{
			get
			{
				return yearOfProduction;
			}

			set
			{
				yearOfProduction = value;
			}
		}

		public string Registration
		{
			get
			{
				return registration;
			}

			set
			{
				registration = value;
			}
		}

		public int TaxiId
		{
			get
			{
				return taxiId;
			}

			set
			{
				taxiId = value;
			}
		}

		public VehicleType VehicleType
		{
			get
			{
				return vehicleType;
			}

			set
			{
				vehicleType = value;
			}
		}

		public long Driver
		{
			get
			{
				return driver;
			}

			set
			{
				driver = value;
			}
		}

		public Vehicle(int yearOfProduction, string registration, int taxiId, VehicleType vehicleType) : base(EntityType.VEHICLE)
		{
			this.YearOfProduction = yearOfProduction;
			this.Registration = registration;
			this.TaxiId = taxiId;
			this.VehicleType = vehicleType;
		}

		public Vehicle(Vehicle other) : base(other)
		{
			this.YearOfProduction = other.YearOfProduction;
			this.Registration = other.Registration;
			this.TaxiId = other.TaxiId;
			this.VehicleType = other.VehicleType;
		}

		public Vehicle(string[] splited)
		{
			long loadedId;
			Int64.TryParse(splited[0], out loadedId);
			this.Id = loadedId;
			int year;
			Int32.TryParse(splited[1], out year);
			this.YearOfProduction = year;
			this.Registration = splited[2];
			int taxiId;
			Int32.TryParse(splited[3], out taxiId);
			this.TaxiId = taxiId;
			VehicleType vt;
			Enum.TryParse(splited[4], out vt);
			this.VehicleType = vt;
			long driverId;
			Int64.TryParse(splited[5], out driverId);
			this.driver = driverId;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{YearOfProduction}{SEPARATOR}");
			sb.Append($"{Registration}{SEPARATOR}");
			sb.Append($"{TaxiId}{SEPARATOR}");
			sb.Append($"{VehicleType}{SEPARATOR}");
			sb.Append($"{Driver}{SEPARATOR}");
			return sb.ToString();
		}
	}
}

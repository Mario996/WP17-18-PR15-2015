using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Fare : Entity
	{
		private DateTime dateOfDrive;
		private long startLocation;
		private VehicleType desiredVehicleType;
		private long customer;
		private long finishLocation;
		private long dispather;
		private long driver;
		private double amount;
		private long comment;
		private FareStatus fareStatus;
		private int type;
		private DateTime date;

		public DateTime DateOfDrive
		{
			get
			{
				return dateOfDrive;
			}

			set
			{
				dateOfDrive = value;
			}
		}

		public long StartLocation
		{
			get
			{
				return startLocation;
			}

			set
			{
				startLocation = value;
			}
		}

		public VehicleType DesiredVehicleType
		{
			get
			{
				return desiredVehicleType;
			}

			set
			{
				desiredVehicleType = value;
			}
		}

		public long Customer
		{
			get
			{
				return customer;
			}

			set
			{
				customer = value;
			}
		}

		public long FinishLocation
		{
			get
			{
				return finishLocation;
			}

			set
			{
				finishLocation = value;
			}
		}

		public long Dispather
		{
			get
			{
				return dispather;
			}

			set
			{
				dispather = value;
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

		public double Amount
		{
			get
			{
				return amount;
			}

			set
			{
				amount = value;
			}
		}

		public long Comment
		{
			get
			{
				return comment;
			}

			set
			{
				comment = value;
			}
		}

		public FareStatus FareStatus
		{
			get
			{
				return fareStatus;
			}

			set
			{
				fareStatus = value;
			}
		}

		public Fare() : base(EntityType.FARE)
		{
		}
		public Fare(string[] splited)
		{
			long tempLong;
			Int64.TryParse(splited[0], out tempLong);
			this.Id = tempLong;
			Int64.TryParse(splited[1], out tempLong);
			dateOfDrive = DateTime.FromFileTimeUtc(tempLong);
			Int64.TryParse(splited[2], out startLocation);
			Enum.TryParse(splited[3], out desiredVehicleType);
			Int64.TryParse(splited[4], out customer);
			Int64.TryParse(splited[5], out finishLocation);
			Int64.TryParse(splited[6], out dispather);
			Int64.TryParse(splited[7], out driver);
			Double.TryParse(splited[8], out amount);
			Int64.TryParse(splited[9], out comment);
			Enum.TryParse(splited[10], out fareStatus);
		}

		public Fare(long locId, long customerId, int type, DateTime dateOfDrive) : base(EntityType.FARE)
		{
			this.startLocation = locId;
			this.customer = customerId;
			if (type > 0)
			{
				desiredVehicleType = (VehicleType)type;
			}
			fareStatus = FareStatus.ON_HOLD;
			this.dateOfDrive = dateOfDrive;
        }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{DateOfDrive.ToFileTimeUtc()}{SEPARATOR}");
			sb.Append($"{StartLocation}{SEPARATOR}");
			sb.Append($"{DesiredVehicleType}{SEPARATOR}");
			sb.Append($"{Customer}{SEPARATOR}");
			sb.Append($"{FinishLocation}{SEPARATOR}");
			sb.Append($"{Dispather}{SEPARATOR}");
			sb.Append($"{Driver}{SEPARATOR}");
			sb.Append($"{Amount}{SEPARATOR}");
			sb.Append($"{Comment}{SEPARATOR}");
			sb.Append($"{FareStatus}{SEPARATOR}");
			return sb.ToString();
		}
	}
}

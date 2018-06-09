using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Driver : User
	{
		private long location = -1;
		private long vehicle;
		private bool busy = false;

		public Driver() : base(EntityType.DRIVER)
		{
		}

		public Driver(string[] splited) : base(splited)
		{
			long locId;
			Int64.TryParse(splited[12], out locId);
			this.Location = locId;
			long vehicleId;
			Int64.TryParse(splited[13], out vehicleId);
			this.Vehicle = vehicleId;
			Boolean.TryParse(splited[14], out busy);
		}

		public Driver(Driver other) : base(other)
		{
			this.Location = other.Location;
			this.Vehicle = other.Vehicle;
		}

		public Driver(string username, string password,
			string firstName, string lastName, Gender gender, string idNumber, string phoneNumber,
			string emailAddress, UserType userRole) : base(username, password, firstName, lastName, gender, idNumber, phoneNumber, emailAddress, userRole, EntityType.DRIVER)
		{
		}

		public long Location
		{
			get
			{
				return location;
			}

			set
			{
				location = value;
			}
		}

		public long Vehicle
		{
			get
			{
				return vehicle;
			}

			set
			{
				vehicle = value;
			}
		}

		public bool Busy
		{
			get
			{
				return busy;
			}

			set
			{
				busy = value;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{Location}{SEPARATOR}");
			sb.Append($"{Vehicle}{SEPARATOR}");
			sb.Append($"{Busy}{SEPARATOR}");
			return sb.ToString();
		}
	}
}

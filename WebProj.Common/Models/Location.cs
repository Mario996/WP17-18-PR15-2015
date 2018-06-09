using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Location : Entity
	{
		private double x;
		private double y;
		private long address;

		public Location(Location other) : base(other)
		{
			this.x = other.x;
			this.y = other.y;
		}

		public Location() : base(EntityType.LOCATION)
		{
		}

		public Location(string[] splited)
		{
			long loadedId;
			Int64.TryParse(splited[0], out loadedId);
			this.Id = loadedId;
			double temp;
			Double.TryParse(splited[1], out temp);
			this.x = temp;
			Double.TryParse(splited[2], out temp);
			this.y = temp;
			long address;
			Int64.TryParse(splited[3], out address);
			this.Address = address;
		}

		public Location(double locX, double locY) : this()
		{
			this.x = locX;
			this.y = locY;
		}

		public double X
		{
			get
			{
				return x;
			}

			set
			{
				x = value;
			}
		}

		public double Y
		{
			get
			{
				return y;
			}

			set
			{
				y = value;
			}
		}

		public long Address
		{
			get
			{
				return address;
			}

			set
			{
				address = value;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{x}{SEPARATOR}");
			sb.Append($"{y}{SEPARATOR}");
			sb.Append($"{address}{SEPARATOR}");
			return sb.ToString();
		}
	}
}

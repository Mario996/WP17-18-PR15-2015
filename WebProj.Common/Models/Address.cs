using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Address : Entity
	{
		private string street;
		private int number;
		private string city;
		private int postalCode;

		public Address(Address other) : base(other)
		{
			this.street = other.street;
			this.number = other.number;
			this.city = other.city;
			this.postalCode = other.postalCode;
		}

		public Address() : base(EntityType.ADDRESS)
		{
		}

		public Address(string[] splited)
		{
			long loadedId;
			Int64.TryParse(splited[0], out loadedId);
			this.Id = loadedId;
			this.street = splited[1];
			int number;
			Int32.TryParse(splited[2], out number);
			this.number = number;
			this.city = splited[3];
			int postalCode;
			Int32.TryParse(splited[4], out postalCode);
			this.postalCode = postalCode;
		}

		public Address(string addrStreet, int addrNumber, string addrCity, int addrPostalCode) : this()
		{
			this.street = addrStreet;
			this.number = addrNumber;
			this.city = addrCity;
			this.postalCode= addrPostalCode;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{street}{SEPARATOR}");
			sb.Append($"{number}{SEPARATOR}");
			sb.Append($"{city}{SEPARATOR}");
			sb.Append($"{postalCode}{SEPARATOR}");
			return sb.ToString();
		}

		public string Street
		{
			get
			{
				return street;
			}

			set
			{
				street = value;
			}
		}

		public int Number
		{
			get
			{
				return number;
			}

			set
			{
				number = value;
			}
		}

		public string City
		{
			get
			{
				return city;
			}

			set
			{
				city = value;
			}
		}

		public int PostalCode
		{
			get
			{
				return postalCode;
			}

			set
			{
				postalCode = value;
			}
		}
	}
}

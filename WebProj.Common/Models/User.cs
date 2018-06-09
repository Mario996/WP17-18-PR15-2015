using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebProj.Common;
using System.Runtime.Serialization;
using WebProj.Common.Utils;

namespace WebProj.Common.Models
{
	public class User : Entity
	{
		private string userName;
		private string password;
		private string firstName;
		private string lastName;
		private Gender gender;
		private string idnumber;
		private string phoneNumber;
		private string emailAddress;
		private UserType userRole;
		List<long> fares;
		private bool blocked = false;

		public string GenderString
		{
			get
			{
				return gender.ToString();
			}
		}

		#region properties
		public string UserName
		{
			get
			{
				return userName;
			}

			set
			{
				userName = value;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}

			set
			{
				password = value;
			}
		}

		public string FirstName
		{
			get
			{
				return firstName;
			}

			set
			{
				firstName = value;
			}
		}

		public string LastName
		{
			get
			{
				return lastName;
			}

			set
			{
				lastName = value;
			}
		}

		public Gender Gender
		{
			get
			{
				return gender;
			}

			set
			{
				gender = value;
			}
		}

		public string Idnumber
		{
			get
			{
				return idnumber;
			}

			set
			{
				idnumber = value;
			}
		}

		public string PhoneNumber
		{
			get
			{
				return phoneNumber;
			}

			set
			{
				phoneNumber = value;
			}
		}

		public string EmailAddress
		{
			get
			{
				return emailAddress;
			}

			set
			{
				emailAddress = value;
			}
		}

		public UserType UserRole
		{
			get
			{
				return userRole;
			}

			set
			{
				userRole = value;
			}
		}

		public bool Blocked
		{
			get
			{
				return blocked;
			}

			set
			{
				blocked = value;
			}
		}

		public List<long> Fares
		{
			get
			{
				return fares;
			}

			set
			{
				fares = value;
			}
		}
		#endregion

		public User(User other) : base(other)
		{
			this.userName = other.UserName;
			this.password = other.Password;
			this.firstName = other.FirstName;
			this.lastName = other.LastName;
			this.gender = other.Gender;
			this.idnumber = other.Idnumber;
			this.phoneNumber = other.PhoneNumber;
			this.emailAddress = other.EmailAddress;
			this.userRole = other.UserRole;
			this.Fares = new List<long>(other.Fares);
		}


		#region ctor
		public User(string username, string password, string firstName, string lastName,
			Gender gender, string idNumber, string phoneNumber, string emailAddress, UserType userRole)
			: base(EntityType.USER)
		{
			this.userName = username;
			this.password = password;
			this.firstName = firstName;
			this.lastName = lastName;
			this.gender = gender;
			this.idnumber = idNumber;
			this.phoneNumber = phoneNumber;
			this.emailAddress = emailAddress;
			this.userRole = userRole;
			Fares = new List<long>();
		}

		public User() : base(EntityType.USER)
		{

		}

		public User(EntityType type) : base(type)
		{

		}

		public User(string[] splited)
		{
			long loadedId;
			Int64.TryParse(splited[0], out loadedId);
			this.Id = loadedId;
			this.userName = splited[1];
			this.password = splited[2];
			this.firstName = splited[3];
			this.lastName = splited[4];
			Gender g;
			Enum.TryParse(splited[5], out g);
			this.gender = g;
			this.idnumber = splited[6];
			this.phoneNumber = splited[7];
			this.emailAddress = splited[8];
			UserType role;
			Enum.TryParse(splited[9], out role);
			this.userRole = role;
			string[] faresString = splited[10].Split(',');
			Fares = new List<long>(faresString.Length);
			foreach (string fare in faresString)
			{
				long fareId;
				Int64.TryParse(fare, out fareId);
				Fares.Add(fareId);
			}
			Boolean.TryParse(splited[11], out blocked);
		}

		public User(string username, string password, string firstName, string lastName, Gender gender, string idNumber, string phoneNumber, string emailAddress, UserType userRole, EntityType type) 
		{
			this.Id = IdHelper.Instance.GenerateId(type);
			this.userName = username;
			this.password = password;
			this.firstName = firstName;
			this.lastName = lastName;
			this.gender = gender;
			this.idnumber = idNumber;
			this.phoneNumber = phoneNumber;
			this.emailAddress = emailAddress;
			this.userRole = userRole;
			Fares = new List<long>();
		}
		#endregion

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			sb.Append($"{userName}{SEPARATOR}");
			sb.Append($"{password}{SEPARATOR}");
			sb.Append($"{firstName}{SEPARATOR}");
			sb.Append($"{lastName}{SEPARATOR}");
			sb.Append($"{gender}{SEPARATOR}");
			sb.Append($"{idnumber}{SEPARATOR}");
			sb.Append($"{phoneNumber}{SEPARATOR}");
			sb.Append($"{emailAddress}{SEPARATOR}");
			sb.Append($"{userRole}{SEPARATOR}");
			sb.Append($"{string.Join(",", Fares)}{SEPARATOR}");
			sb.Append($"{blocked}{SEPARATOR}");
			return sb.ToString();
		}
	}
}
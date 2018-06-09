using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Models
{
	public class Comment : Entity
	{
		string description;
		DateTime dateOfPublish;
		long user;
		long fare;
		int mark;

		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}

		public DateTime DateOfPublish
		{
			get
			{
				return dateOfPublish;
			}

			set
			{
				dateOfPublish = value;
			}
		}

		public long User
		{
			get
			{
				return user;
			}

			set
			{
				user = value;
			}
		}

		public long Fare
		{
			get
			{
				return fare;
			}

			set
			{
				fare = value;
			}
		}

		public int Mark
		{
			get
			{
				return mark;
			}

			set
			{
				mark = value;
			}
		}

		public Comment() : base(EntityType.COMMENT)
		{
			//DataManager.DataManager.Instance.DajMiNekoSmece();

		}

		public Comment(string[] splited)
		{
			long tempLong;
			Int64.TryParse(splited[0], out tempLong);
			this.Id = tempLong;
			description = splited[1];
			Int64.TryParse(splited[2], out tempLong);
			dateOfPublish = DateTime.FromFileTimeUtc(tempLong);
			Int64.TryParse(splited[3], out user);
			Int64.TryParse(splited[4], out fare);
			Int32.TryParse(splited[5], out mark);
		}

		public Comment(string comment) : base(EntityType.COMMENT)
		{
			this.description = comment;
			this.dateOfPublish = DateTime.Now;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(base.ToString());
			
			sb.Append($"{description}{SEPARATOR}");
			sb.Append($"{dateOfPublish.ToFileTimeUtc()}{SEPARATOR}");
			sb.Append($"{user}{SEPARATOR}");
			sb.Append($"{fare}{SEPARATOR}");
			sb.Append($"{mark}{SEPARATOR}");
			return sb.ToString();
		}
	}
}

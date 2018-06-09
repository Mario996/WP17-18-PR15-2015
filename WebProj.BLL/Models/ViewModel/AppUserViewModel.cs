using WebProj.Common;
using WebProj.Common.Models;

namespace WebProj.BLL.Models.ViewModel
{
	public class AppUserViewModel : ViewModel
	{
		public string UserName { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public bool IsAdmin { get; private set; }
		public bool IsDriver { get; private set; }
		public bool IsCustomer { get; private set; }
		public UserType Role { get; private set; }

		public string UserRole { get { return Role.ToString(); } }

		public AppUserViewModel(User user) : base(user.Id)
		{
			UserName = user.UserName;
			FirstName = user.FirstName;
			LastName = user.LastName;
			IsAdmin = user.UserRole == UserType.DISPATCHER;
			IsDriver = user.UserRole == UserType.DRIVER;
			IsCustomer = user.UserRole == UserType.CUSTOMER;
			Role = user.UserRole;
		}
	}
}
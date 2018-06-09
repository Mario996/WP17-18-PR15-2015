using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using Model = WebProj.Common.Models;
namespace WebProj.BLL.BusinessLogic.Driver.Service
{
	public interface IDriverService
	{
		IEnumerable<Model.Driver> GetFreeDrivers();
		bool RegisterDriver(string username, string password, string firstName, string lastName, Gender genderEnum, string jmbg, string phoneNumber, string emailAddress, UserType role, int year, string plate, int taxiId, VehicleType type, out string report);
		bool UpdateDriverLocation(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long id, out string report);
	}
}

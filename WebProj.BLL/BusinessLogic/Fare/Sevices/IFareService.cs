using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;
using WebProj.Common;

namespace WebProj.BLL.BusinessLogic.Fare.Sevices
{
	public interface IFareService
	{
		IEnumerable<FareViewModel> GetUserFares(long customerId);
		bool UpdateCustomerFare(long id, double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, int type, DateTime date, FareStatus status, string comment, out string report);
		bool CreateCustomerFare(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long customer, int type, DateTime date, out string report);
		IEnumerable<FareViewModel> GetUnassignedFares();

		bool TakeFare(long fareId, long driverId, out string report);
		bool UpdateDispatcherFare(long id, double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, double price, FareStatus status, out string report);
		bool CreateDriverFare(double locX, double locY, string addrStreet, int addrNumber, string addrCity, int addrPostalCode, long dispatherId, int type, DateTime date, long driverId, out string report);
		bool UpdateDispatcherFare(long id, FareStatus status, out string report);

		bool UpdateDispatcherFare(long id, FareStatus status,long driver,long fareid, out string report);
		bool CommnetFare(long id, string comment, int rate, out string report);
		IEnumerable<FareViewModel> GetFilerCustomerFares(long id, FareStatus status);
		IEnumerable<FareViewModel> GetFilterDriverFares(long id, FareStatus status);

		IEnumerable<FareViewModel> GetDispatcherFares(long id);
		bool UpdateDispatcherFareFailded(long id, FareStatus status, string comment, out string report);
	}
}

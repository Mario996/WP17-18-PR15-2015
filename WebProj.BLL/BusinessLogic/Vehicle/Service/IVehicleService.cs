using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;


namespace WebProj.BLL.BusinessLogic.Vehicle.Service
{
	public interface IVehicleService
	{
		IEnumerable<VehicleViewModel> GetAllVehicles();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.BLL.Models.ViewModel;
using Model = WebProj.Common.Models;

namespace WebProj.BLL.BusinessLogic.Vehicle.Service
{
	public class VehicleServiceImpl : IVehicleService
	{
		public IEnumerable<VehicleViewModel> GetAllVehicles()
		{
			var models = DataManager.DataManager.Instance.GetAllByType<Model.Vehicle>();
			List<VehicleViewModel> retVal = new List<VehicleViewModel>();
			foreach(var m in models)
			{
				retVal.Add(new VehicleViewModel(m));
			}
			return retVal;
		}
	}
}

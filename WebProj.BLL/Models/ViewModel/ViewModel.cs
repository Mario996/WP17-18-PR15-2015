using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProj.BLL.Models.ViewModel
{
	public class ViewModel
	{
		public ViewModel()
		{
		}

		public ViewModel(long id)
		{
			Id = id;
		}

		public long Id { get; set; }
	}
}

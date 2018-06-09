using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProj.BLL.BusinessLogic.Token.Services
{
	public interface ITokenService
	{
		bool ValidateToken(long userId);

		bool Kill(long userId);

		void AssingToken(long userId);
	}
}

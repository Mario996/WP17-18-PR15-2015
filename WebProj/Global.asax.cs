using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebProj.Auth;
using WebProj.BLL.BusinessLogic.Account.Services;
using WebProj.BLL.BusinessLogic.Driver.Service;
using WebProj.BLL.BusinessLogic.Fare.Sevices;
using WebProj.BLL.BusinessLogic.Token.Services;
using WebProj.BLL.BusinessLogic.User.Services;
using WebProj.BLL.BusinessLogic.Vehicle.Service;

namespace WebProj
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
			//GlobalConfiguration.Configuration.Filters.Add(new MyBasicAuthenticationFilter());


			var builder = new ContainerBuilder();
			builder.RegisterType<UserServiceImpl>().As<IUserService>().SingleInstance();
			builder.RegisterType<AccountServiceImpl>().As<IAccountService>().SingleInstance();
			builder.RegisterType<TokenServiceImpl>().As<ITokenService>().SingleInstance();
			builder.RegisterType<FareService>().As<IFareService>().SingleInstance();
			builder.RegisterType<DriverService>().As<IDriverService>().SingleInstance();
			builder.RegisterType<VehicleServiceImpl>().As<IVehicleService>().SingleInstance();

			var container = builder.Build();
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
    }
}

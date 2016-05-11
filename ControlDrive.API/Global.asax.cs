using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ControlDrive.Core.App_Start;
using System.Globalization;

namespace ControlDrive.Core
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Culture = new CultureInfo(string.Empty)
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalDigits = 5
                }
            };

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofacWebApiConfig.Initialize(GlobalConfiguration.Configuration);


        }
    }
}

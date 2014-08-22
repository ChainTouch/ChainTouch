using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using System.Threading;

using Weitm.Modules.Membership;
using Weitm.Modules.Membership.Models;

namespace Weitm.Applications.ChainTouchPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static WeitmMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError();
            //if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            //{
            //    Response.Redirect("~/Error/PageNotFound/");
            //}
        }
    }
}
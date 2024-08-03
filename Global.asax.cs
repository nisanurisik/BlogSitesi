using BlogSitesi.Filters;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSitesi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SessionCheckAttribute());
        }
    }
}

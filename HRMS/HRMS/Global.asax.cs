using HRMS.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Disable database initializer (useful in production)
            Database.SetInitializer<ApplicationDbContext>(null);

            // Register MVC areas, routes, and filters
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Optionally register global filters
            // FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}

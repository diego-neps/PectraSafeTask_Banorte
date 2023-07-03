using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PectraForms.WebApplication
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFiltersConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}

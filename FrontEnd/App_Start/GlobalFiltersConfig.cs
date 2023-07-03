using PectraForms.WebApplication.Attributes;
using System.Web.Mvc;

namespace PectraForms.WebApplication
{
    public class GlobalFiltersConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new XFrameOptionsAllowFromConfigFilter());
        }
    }
}
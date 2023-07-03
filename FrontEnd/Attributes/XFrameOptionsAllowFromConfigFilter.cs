using System.Configuration;
using System.Web.Mvc;

namespace PectraForms.WebApplication.Attributes
{
    public class XFrameOptionsAllowFromConfigFilter : ActionFilterAttribute
    {
        private const string APP_SETTINGS_KEY = "XFrameOptionsAllowFrom";
        private const string HEADER_NAME = "x-frame-options";
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var allowFrom = ConfigurationManager.AppSettings[APP_SETTINGS_KEY];
            if (allowFrom != null)
            {
                allowFrom = allowFrom.Trim();
                if (!string.IsNullOrEmpty(allowFrom))
                {
                    allowFrom = string.Format("allow-from {0}", allowFrom);
                    filterContext.HttpContext.Response.Headers.Remove(HEADER_NAME);
                    filterContext.HttpContext.Response.AddHeader(HEADER_NAME, allowFrom);
                }
            }
        }
    }
}
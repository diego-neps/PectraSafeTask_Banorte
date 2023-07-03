using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace PectraForms.WebApplication.BackEnd.API.Filters
{
    public class GlobalExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var msg = actionExecutedContext.Exception.GetBaseException().Message;
            Log.Error(msg, actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }

    }
}
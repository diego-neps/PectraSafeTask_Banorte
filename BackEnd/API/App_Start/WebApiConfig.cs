using Microsoft.Owin.Security.OAuth;
using PectraForms.WebApplication.BackEnd.API.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PectraForms.WebApplication.BackEnd.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // Filtro de Excepciones
            config.Filters.Add(new GlobalExceptionFilterAttribute());
            // Configuración de logging
            log4net.Config.XmlConfigurator.Configure();

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",                
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

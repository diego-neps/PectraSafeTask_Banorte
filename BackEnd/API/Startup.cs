using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using PectraForms.WebApplication.BackEnd.API.Provider;

[assembly: OwinStartup(typeof(PectraForms.WebApplication.BackEnd.API.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace PectraForms.WebApplication.BackEnd.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Para obtener más información acerca de cómo configurar su aplicación, visite http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

    }
}

using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PectraForms.WebApplication.BackEnd.API.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PectraForms.WebApplication.BackEnd.API
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static OAuthAuthorizationServerOptions OAuthOptionsLogin { get; private set; }

        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new PectraAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true
            };

            OAuthOptionsLogin = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/login"),
                Provider = new PectraAuthProviderLogin(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true
            };
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.Use<AuthenticationMiddleware>();

            app.UseOAuthAuthorizationServer(OAuthOptions);

            app.UseOAuthAuthorizationServer(OAuthOptionsLogin);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
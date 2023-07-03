using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PectraForms.WebApplication.BackEnd.API.Helpers;
using PectraForms.WebApplication.BackEnd.API.PectraGDReference;
using System.Net;
using System.ServiceModel;
using System.Configuration;

namespace PectraForms.WebApplication.BackEnd.API.Provider
{
    public class PectraAuthProviderLogin : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    string szError = string.Empty;
                    User usr = new User();

                    if (ValidateUserOnPectra(context.UserName, context.Password, ref szError, usr))
                    {
                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Name, context.UserName),
                            new Claim("SessionToken", usr.SessionToken),
                            new Claim("DefaultLanguage", usr.DefaultLanguage),
                            new Claim("DefaultOperationalUnit", usr.DefaultOperationalUnit),
                            new Claim("DefaultOrganization", usr.DefaultOrganization),
                            new Claim("DefaultProfile", usr.DefaultProfile)
                        };
                        ClaimsIdentity oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                        context.Validated(new AuthenticationTicket(oAutIdentity, new AuthenticationProperties() { }));
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Error de validación en Pectra. " + szError);
                        context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
                    }
                }
                catch (Exception ex)
                {
                    context.SetError("Exception validando en Pectra", ex.Message);
                }
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }


        private class User
        {
            public string SessionToken { get; set; }
            public string DefaultLanguage { get; set; }
            public string DefaultOperationalUnit { get; set; }
            public string DefaultOrganization { get; set; }
            public string DefaultProfile { get; set; }
        }


        /// <summary>
        /// Validates the user on pectra.
        /// </summary>
        /// <param name="username">Usr id.</param>
        /// <param name="password">Password.</param>
        /// <param name="pszError">Retorna el posible mensaje de error.</param>
        /// <param name="pUser">Retorna el token de sesión y los datos del usuario.</param>
        /// <returns></returns>
        private bool ValidateUserOnPectra(string username, string password, ref string pszError, User pUser)
        {
            bool bRet = false;
            try
            {
                LoginResponse oRes = null;

                bool bWindowsAuthentication = false;
                bool.TryParse(ConfigurationManager.AppSettings["WindowsAuthentication"], out bWindowsAuthentication);

                if (bWindowsAuthentication)
                {
                    LoginWindowsSecurityRequest oReq = new LoginWindowsSecurityRequest();
                    new ClientProxyHelper<IGDService>().Use(serviceProxy =>
                    {
                        oRes = serviceProxy.LoginWindowsSecurity(oReq);
                    }, "WSHttpBinding_IGDService1", username, password);
                }
                else
                {
                    LoginRequest oReq = new LoginRequest() { LanguageId = "Spanish", UsrId = username, UsrPwd = password };
                    new ClientProxyHelper<IGDService>().Use(serviceProxy =>
                    {
                        oRes = serviceProxy.Login(oReq);
                    }, "WSHttpBinding_IGDService");
                }

                if (oRes != null)
                {
                    bRet = true;
                    pUser.SessionToken = oRes.SessionToken;
                    pUser.DefaultLanguage = oRes.User.DefaultLanguage.LanguageId;
                    pUser.DefaultOperationalUnit = oRes.User.DefaultOperationalUnit.Id;
                    pUser.DefaultOrganization = oRes.User.DefaultOrganization.Id;
                    pUser.DefaultProfile = oRes.User.DefaultProfile.Id;
                }
            }
            catch (FaultException fex)
            {
                bRet = false;
                pszError = fex.Message;
            }
            return bRet;
        }

    }
}
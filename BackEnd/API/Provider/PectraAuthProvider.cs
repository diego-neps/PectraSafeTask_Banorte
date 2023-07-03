using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using PectraForms.WebApplication.BackEnd.API.Helpers;
using PectraForms.WebApplication.BackEnd.API.PectraBPReference;
using PectraForms.WebApplication.BackEnd.API.PectraOBPIReference;
using System.Net;
using System.Web.Http;

namespace PectraForms.WebApplication.BackEnd.API.Provider
{
    public class PectraAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                string guid = context.OwinContext.Get<string>("guid");
                string trxid = context.OwinContext.Get<string>("trxid");
                string username = string.Empty;
                try
                {
                    if (ValidateUserOnPectra(trxid, guid, ref username))
                    {
                        var claims = new List<Claim>() {
                                new Claim(ClaimTypes.Name, username)
                            };
                        ClaimsIdentity oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                        context.Validated(new AuthenticationTicket(oAutIdentity, new AuthenticationProperties() { }));
                    }
                    else
                    {
                        context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
                        context.SetError("invalid_grant", "TrxId o GUID incorrectos.");
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
            if (context.ClientId == null)
            {
                string[] aGuid = context.Parameters.Where(f => f.Key == "guid").Select(f => f.Value).SingleOrDefault();
                if (aGuid != null && aGuid.Length > 0)
                {
                    context.OwinContext.Set<string>("guid", aGuid[0]);
                    string[] aTrxId = context.Parameters.Where(f => f.Key == "trxid").Select(f => f.Value).SingleOrDefault();
                    if (aGuid != null && aGuid.Length > 0)
                    {
                        context.OwinContext.Set<string>("trxid", aTrxId[0]);
                        context.Validated();
                    }
                    else
                    {
                        context.SetError("invalid_client", "Missing trxid parameter.");
                        context.Rejected();
                    }
                }
                else
                {
                    context.SetError("invalid_client", "Missing guid parameter.");
                    context.Rejected();
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Valida que exista el package y la sesión, además que el usuario del package corresponda con el de la sesión y que la misma no esté vencida.
        /// </summary>
        /// <param name="pTrxId">TRX id.</param>
        /// <param name="pGUID">GUID identificador de sesión.</param>
        /// <param name="pUsrId">User id del package.</param>
        /// <returns></returns>
        private bool ValidateUserOnPectra(string pTrxId, string pGUID, ref string pUsrId)
        {
            bool bRet = false;
            PackageGetOneResponse oResPkg = null;

            PackageGetOneSecureRequest oReqPkg = new PackageGetOneSecureRequest();
            oReqPkg.LanguageId = "Spanish";
            oReqPkg.TrxId = Guid.Parse(pTrxId);

            new ClientProxyHelper<IBPService>().Use(serviceProxy =>
            {
                oResPkg = serviceProxy.PackageGetOneSecure(oReqPkg);
            }, "WSHttpBinding_IBPService");

            if (oResPkg != null && oResPkg.Package != null)
            {
                pUsrId = oResPkg.Package.UsrId;
                ValidateSessionResponse oRes = null;
                new ClientProxyHelper<IPectraOBPIService>().Use(serviceProxy =>
                {
                    oRes = serviceProxy.ValidateSession(new ValidateSessionRequest() { UsrId = oResPkg.Package.UsrId, GUID = pGUID });
                }, "BasicHttpBinding_IPectraOBPIService");
                bRet = oRes.Result;
            }
            return bRet;
        }

    }

}
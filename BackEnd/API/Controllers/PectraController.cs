using PectraForms.WebApplication.BackEnd.API.Helpers;
using PectraForms.WebApplication.BackEnd.API.PectraBPReference;
using PectraForms.WebApplication.BackEnd.API.PectraGDReference;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Xml.Linq;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    [Authorize]
    public class PectraController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetTreeView(int PngId, string OrgId, string UoId, string ProfId)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string SessionToken, DefaultLanguage;
            ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

            GetTreeViewRequest oGetTreeViewRequest = new GetTreeViewRequest()
            {
                SessionToken = SessionToken,
                UsrId = identity.Identity.Name,
                LanguageId = DefaultLanguage,
                OrgId = OrgId,
                UoId = UoId,
                ProfId = ProfId
            };

            try
            {
                GetTreeViewResponse oGetTreeViewResponse = new GetTreeViewResponse();

                new ClientProxyHelper<IBPService>().Use(
                        serviceProxy => { oGetTreeViewResponse = serviceProxy.GetTreeView(oGetTreeViewRequest); },
                                                    "WSHttpBinding_IBPService");

                return Ok(oGetTreeViewResponse.TreeView.BusinessProcesses.Where(x => x.PngId == PngId));
            }
            catch (InvalidSessionException)
            {
                return Unauthorized();
            }
            catch (Exception Exc)
            {
                return InternalServerError(new Exception(Exc.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult GetFunctionDef(int FuncId)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string DefaultLanguage = identity.Claims.Where(c => c.Type == "DefaultLanguage").Select(c => c.Value).SingleOrDefault();
            string SessionToken = identity.Claims.Where(c => c.Type == "SessionToken").Select(c => c.Value).SingleOrDefault();

            GetFunctionRequest oGetFunctionRequest = new GetFunctionRequest() {
                FunctionId = FuncId,
                LanguageId = DefaultLanguage,
                SessionToken = SessionToken,
                UsrId = identity.Identity.Name
            };

            try
            {
                GetFunctionResponse oGetFunctionResponse = new GetFunctionResponse();

                new ClientProxyHelper<IBPService>().Use(
                        serviceProxy => { oGetFunctionResponse = serviceProxy.GetFunction(oGetFunctionRequest); },
                                                    "WSHttpBinding_IBPService");

                XDocument doc = null;
                if (oGetFunctionResponse != null && oGetFunctionResponse.Function != null && oGetFunctionResponse.Function.Definition != null)
                {
                    doc = XDocument.Parse(oGetFunctionResponse.Function.Definition);

                    var cDataNodes = doc.DescendantNodes().OfType<XCData>().ToArray();
                    foreach (var cDataNode in cDataNodes)
                        cDataNode.ReplaceWith(new XText(cDataNode));
                }

                return Ok(doc);
            }
            catch (InvalidSessionException)
            {
                return Unauthorized();
            }
            catch (Exception Exc)
            {
                return InternalServerError(new Exception(Exc.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult GetNewTrxId(string OrgId, string UoId, string ProfId, int PngId, int VersionId, int SubProId, int ActId, int FunctionId, int InsId = 0, bool Blocked = true)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string SessionToken, DefaultLanguage;
            ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

            GetNewTrxIdRequest oGetNewTrxIdReq = new GetNewTrxIdRequest()
            {
                SessionToken = SessionToken,
                UsrId = identity.Identity.Name,
                LanguageId = DefaultLanguage,
                OrgId = OrgId,
                UoId = UoId,
                ProfId = ProfId,
                PngId = PngId,
                VersionId = VersionId,
                SubProId = SubProId,
                ActId = ActId,
                FunctionId = FunctionId,
                PkgId = 1, // PkgType = Com, PkgDescription = Local
                InsId = InsId,
                Blocked = Blocked
            };

            try
            {
                GetNewTrxIdSecureResponse oGetNewTrxIdSecureRes = null;

                new ClientProxyHelper<IBPService>().Use(
                    serviceProxy => { oGetNewTrxIdSecureRes = serviceProxy.GetNewTrxIdSecure(oGetNewTrxIdReq); },
                    "WSHttpBinding_IBPService");

                string szTrxId = oGetNewTrxIdSecureRes.TrxId.ToString();

                return Ok(szTrxId);
            }
            catch (InvalidSessionException)
            {
                return Unauthorized();
            }
            catch (Exception Exc)
            {
                return InternalServerError(new Exception(Exc.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult GetInbox(string OrgId, string UoId, string ProfId, int PngId, int VersionId, int ActId, string NodeKey, int Top = 10)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string SessionToken, DefaultLanguage;
            ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

            GetInboxRequest oGetInboxRequest = new GetInboxRequest()
            {
                SessionToken = SessionToken,
                UsrId = identity.Identity.Name,
                LanguageId = DefaultLanguage,
                OrgId = OrgId,
                UoId = UoId,
                ProfId = ProfId,
                PngId = PngId,
                VersionId = VersionId,
                // El SubProId se fuerza a cero porque en los menú items siempre queda en cero, independientemente
                // del q le corresponda según la actividad.
                SubProId = 0,
                ActId = ActId,
                NodeKey = NodeKey,
                ISPAState = 1,
                ISPADate = null,
                OverrideExistingFilters = true,
                InsId = 0,
                InsWhere = "-1",
                InsOrder = 1,
                PaginateCurrentPage = 0,
                PaginateRowsByPage = Top,
                LastPage = false,
                InstanceFilters = null,
                AdvancedFilters = null
            };

            try
            {
                GetInboxResponse oGetInboxResponse = new GetInboxResponse();

                new ClientProxyHelper<IBPService>().Use(
                        serviceProxy => { oGetInboxResponse = serviceProxy.GetInbox(oGetInboxRequest); },
                                                    "WSHttpBinding_IBPService");

                return Ok(oGetInboxResponse.Inbox);
            }
            catch (InvalidSessionException)
            {
                return Unauthorized();
            }
            catch (Exception Exc)
            {
                return InternalServerError(new Exception(Exc.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult GetOrgUoProf(int PngId, int VersionId)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string SessionToken, DefaultLanguage;
            ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

            GetOrgUoProfRequest oGetOrgUoProfRequest = new GetOrgUoProfRequest()
            {
                SessionToken = SessionToken,
                UsrId = identity.Identity.Name,
                PngId = PngId,
                VersionId = VersionId
            };

            try
            {
                GetOrgUoProfResponse oGetOrgUoProfResponse = new GetOrgUoProfResponse();

                new ClientProxyHelper<IGDService>().Use(
                    serviceProxy => { oGetOrgUoProfResponse = serviceProxy.GetOrgUoProf(oGetOrgUoProfRequest); },
                    "WSHttpBinding_IGDService");

                return Ok(oGetOrgUoProfResponse.OrgUoProf.Organizations);
            }
            catch (InvalidSessionException)
            {
                return Unauthorized();
            }
            catch (Exception Exc)
            {
                return InternalServerError(new Exception(Exc.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult UnlockInstance(string trxId, string ProfId)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string SessionToken, DefaultLanguage;
            ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);
            try
            {
                long iRet = PectraOBPIHelper.InstanceLockUnLockBP(trxId, false, SessionToken, ProfId, DefaultLanguage);
                return Ok("");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public static void ExtractDataFromClaims(ClaimsPrincipal pIdentity, out string pszSessionToken, out string pszDefaultLanguage)
        {
            pszSessionToken = pIdentity.Claims.Where(c => c.Type == "SessionToken").Select(c => c.Value).SingleOrDefault();
            pszDefaultLanguage = pIdentity.Claims.Where(c => c.Type == "DefaultLanguage").Select(c => c.Value).SingleOrDefault();
        }
    }
}

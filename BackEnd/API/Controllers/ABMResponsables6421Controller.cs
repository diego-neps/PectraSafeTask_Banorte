using Newtonsoft.Json;
using PectraForms.WebApplication.BackEnd.API.db;
using PectraForms.WebApplication.BackEnd.API.Helpers;
using PectraForms.WebApplication.BackEnd.API.PectraBPReference;
using PectraForms.WebApplication.BackEnd.BusinessEntities;
using PectraForms.WebApplication.BackEnd.DataAccessComponents;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Transactions;
using System.Web.Http;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    [Authorize]
    public partial class ABMResponsables6421Controller : ApiController
    {
        /// <summary>
        /// Definición de la vista asociada.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetViewDefinition()
        {
            ExpandoObject oForm = DbContext.GetFormDefinition(6421);
            return Ok(oForm);
        }

        [HttpGet]
        public IHttpActionResult Get(string trxId)
        {
            Dictionary<string, object> dicConditionValues;
            dynamic oViewModel = new ExpandoObject();
            oViewModel.TrxId = trxId;

            // Pectra Attributes
            oViewModel.PectraAttributes = new PectraAttributesBE();
            // Grilla grilla1
            List<ResponsableBE> lstResponsable = new List<ResponsableBE>();
            List<ResponsableBE> lstResponsableDAC = ResponsableDAC.GetAll();    
            foreach (ResponsableBE item in lstResponsableDAC)
            {
                ResponsableBE p = new ResponsableBE() {

                    IdResponsable = item.IdResponsable,
                    NomResponsable = item.NomResponsable,

                };
                lstResponsable.Add(p);
            } 
            oViewModel.ResponsableList = JsonConvert.SerializeObject(lstResponsable);                
            return Ok(oViewModel);
        }

        [HttpPost]
        public IHttpActionResult Cancel([FromBody]string trxId)
        {
            // Unlock instance
            PectraOBPIHelper.InstanceLockUnLock(trxId, false);

            // Clear package
            PectraOBPIHelper.PackageDelOne(trxId);

            return Ok();
        }
        public class ABMResponsables6421Model
        {
            public PectraAttributesBE PectraAttributes { get; set; }
            public string ResponsableList { get; set; }
        }

        public class ABMResponsables6421ModelWithTrxId : ABMResponsables6421Model
        {
            public string TrxId { get; set; }
        }

        public class ABMResponsables6421ModelWithoutTrxId : ABMResponsables6421Model
        {
            public int PngId { get; set; }
            public int VersionId { get; set; }
            public int SubProId { get; set; }
            public int ActId { get; set; }
            public int FunctionId { get; set; }
            public string OrgId { get; set; }
            public string UoId { get; set; }
            public string ProfId { get; set; }
        }

        [HttpPost]
        public IHttpActionResult PostWithActivityEnd(ABMResponsables6421ModelWithTrxId model)
        {
            return this._Post(model, true);
        }

        [HttpPost]
        public IHttpActionResult PostExtWithActivityEnd(ABMResponsables6421ModelWithoutTrxId model)
        {
            return this._Post(model, true);
        }

        [HttpPost]
        public IHttpActionResult PostWithoutActivityEnd(ABMResponsables6421ModelWithTrxId model)
        {
            return this._Post(model, false);
        }

        [HttpPost]
        public IHttpActionResult PostExtWithoutActivityEnd(ABMResponsables6421ModelWithoutTrxId model)
        {
            return this._Post(model, false);
        }

        private IHttpActionResult _Post(ABMResponsables6421Model model, bool bActivityEnd)
        {
            Dictionary<string, object> dicUpdateValues;
            Dictionary<string, object> dicConditionValues;
            string szAttributeValue = string.Empty;
            string szSetAttributes = string.Empty;

            using (TransactionScope transaction = new TransactionScope())
            {
                // Grillas ABM
                if(model.ResponsableList != null)
				{
					List<ResponsableBE> lstResponsable = JsonConvert.DeserializeObject<List<ResponsableBE>>(model.ResponsableList, new JsonSerializerSettings { Culture = Constants.Culture });
					// Elimina registros de base de datos.
					List<object> lstIdsResponsable0 = new List<object>();

	                List<ResponsableBE> dbResponsableResponsable = ResponsableDAC.GetAll();
						foreach (var item0 in dbResponsableResponsable)
					{
						if (!lstResponsable.Exists(x => x.IdResponsable == item0.IdResponsable))
							lstIdsResponsable0.Add(item0.IdResponsable);
					}
             
					foreach (var item0 in lstResponsable )
					{
						// Lista de Ids a eliminar de la tabla en base de datos.
						List<object> lstIdsResponsable1 = new List<object>();
						var parentValue1 = item0.IdResponsable;
             
						if (item0.IdResponsable == 0)
						{
							var p = new ResponsableBE()
							{
	                            NomResponsable = item0.NomResponsable, 
	                        };
	                        ResponsableDAC.Insert(p);
							parentValue1 = p.IdResponsable;
							item0.IdResponsable = p.IdResponsable;
						}
						else
						{
							Dictionary<string, object> pConditionValues = new Dictionary<string, object>();
							pConditionValues.Add("IdResponsable", item0.IdResponsable);
							Dictionary<string, object> pUpdateValues = new Dictionary<string, object>();
	                        pUpdateValues.Add("NomResponsable", item0.NomResponsable);
	                        ResponsableDAC.Update(pUpdateValues, pConditionValues);
	                    }
	                }

					foreach (var id in lstIdsResponsable0)
					{
						Dictionary<string, object> pConditionValues = new Dictionary<string, object>();
						pConditionValues.Add("IdResponsable", id);
							ResponsableDAC.Delete(id);
					}
				}
                // Pectra
                string szTrxId = string.Empty;
                if (model is ABMResponsables6421ModelWithTrxId)
                {
                    szTrxId = ((ABMResponsables6421ModelWithTrxId)model).TrxId;
                }
                else if (model is ABMResponsables6421ModelWithoutTrxId)
                {
                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    string SessionToken, DefaultLanguage;
                    PectraController.ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

                    ABMResponsables6421ModelWithoutTrxId oModel = model as ABMResponsables6421ModelWithoutTrxId;

                    GetNewTrxIdRequest oGetNewTrxIdReq = new GetNewTrxIdRequest()
                    {
                        SessionToken = SessionToken,
                        UsrId = identity.Identity.Name,
                        LanguageId = DefaultLanguage,
                        OrgId = oModel.OrgId,
                        UoId = oModel.UoId,
                        ProfId = oModel.ProfId,
                        PngId = oModel.PngId,
                        VersionId = oModel.VersionId,
                        SubProId = oModel.SubProId,
                        ActId = oModel.ActId,
                        FunctionId = oModel.FunctionId,
                        PkgId = 1, // PkgType = Com, PkgDescription = Local
                        InsId = 0,
                        Blocked = false
                    };

                    GetNewTrxIdSecureResponse oGetNewTrxIdSecureRes = null;

                    new ClientProxyHelper<IBPService>().Use(
                        serviceProxy => { oGetNewTrxIdSecureRes = serviceProxy.GetNewTrxIdSecure(oGetNewTrxIdReq); },
                        "WSHttpBinding_IBPService");

                    szTrxId = oGetNewTrxIdSecureRes.TrxId.ToString();
                }

                long trxID = long.Parse(szTrxId);

                // ActivityEnd
                if (bActivityEnd)
                    PectraOBPIHelper.ActivityEnd(trxID, szSetAttributes);
                else
                    PectraOBPIHelper.PackageDelOne(szTrxId);
              
                transaction.Complete();
		    } // Fin TransactionScope
              
            return Ok();
        
        }
    }
}

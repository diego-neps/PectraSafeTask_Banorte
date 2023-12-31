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
    public partial class AprobarResoluciondelaTarea6424Controller : ApiController
    {
        /// <summary>
        /// Definición de la vista asociada.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetViewDefinition()
        {
            ExpandoObject oForm = DbContext.GetFormDefinition(6424);
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
            oViewModel.PectraAttributes.DsEstado = PectraOBPIHelper.AttributeGetValue(oViewModel.TrxId, "DsEstado").Trim();
            oViewModel.PectraAttributes.FechaCargaTarea = PectraOBPIHelper.AttributeGetValue(oViewModel.TrxId, "FechaCargaTarea").Trim();
            oViewModel.PectraAttributes.FormularioId = PectraOBPIHelper.AttributeGetValue(oViewModel.TrxId, "FormularioId").Trim();
            oViewModel.PectraAttributes.TipoFormularioAgrupadoId = PectraOBPIHelper.AttributeGetValue(oViewModel.TrxId, "TipoFormularioAgrupadoId").Trim();
            oViewModel.PectraAttributes.TipoFormularioId = PectraOBPIHelper.AttributeGetValue(oViewModel.TrxId, "TipoFormularioId").Trim();
            // Formularios
            oViewModel.Formularios = new FormulariosBE();
            dicConditionValues = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(oViewModel.PectraAttributes.FormularioId) == false)
                dicConditionValues.Add("FormularioId", oViewModel.PectraAttributes.FormularioId);
            if (dicConditionValues.Count > 0)
            {
                List<FormulariosBE> lstRdMapsFormularios = FormulariosDAC.GetByParam(dicConditionValues);
                if (lstRdMapsFormularios != null && lstRdMapsFormularios.Count > 0)
                    oViewModel.Formularios = lstRdMapsFormularios[0];
            }
            // FormularioPdfGeoGestion25
            oViewModel.Formularios.GUID_FormPdf = (string.IsNullOrWhiteSpace(oViewModel.Formularios.GUID_FormPdf) == false) ? oViewModel.Formularios.GUID_FormPdf : Guid.NewGuid().ToString();
            // ArchivosadjuntosFotografias26
            oViewModel.Formularios.GUID_Imagenes = (string.IsNullOrWhiteSpace(oViewModel.Formularios.GUID_Imagenes) == false) ? oViewModel.Formularios.GUID_Imagenes : Guid.NewGuid().ToString();
            // ArchivosadjuntosDocumentos27
            oViewModel.Formularios.GUID_Documentos = (string.IsNullOrWhiteSpace(oViewModel.Formularios.GUID_Documentos) == false) ? oViewModel.Formularios.GUID_Documentos : Guid.NewGuid().ToString();
            // ArchivosadjuntosPlanosAutoCAD30
            oViewModel.Formularios.GUID_Planos = (string.IsNullOrWhiteSpace(oViewModel.Formularios.GUID_Planos) == false) ? oViewModel.Formularios.GUID_Planos : Guid.NewGuid().ToString();
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
        public class AprobarResoluciondelaTarea6424Model
        {
            public PectraAttributesBE PectraAttributes { get; set; }
            public FormulariosBE Formularios { get; set; }
        }

        public class AprobarResoluciondelaTarea6424ModelWithTrxId : AprobarResoluciondelaTarea6424Model
        {
            public string TrxId { get; set; }
        }

        public class AprobarResoluciondelaTarea6424ModelWithoutTrxId : AprobarResoluciondelaTarea6424Model
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
        public IHttpActionResult PostWithActivityEnd(AprobarResoluciondelaTarea6424ModelWithTrxId model)
        {
            return this._Post(model, true);
        }

        [HttpPost]
        public IHttpActionResult PostExtWithActivityEnd(AprobarResoluciondelaTarea6424ModelWithoutTrxId model)
        {
            return this._Post(model, true);
        }

        [HttpPost]
        public IHttpActionResult PostWithoutActivityEnd(AprobarResoluciondelaTarea6424ModelWithTrxId model)
        {
            return this._Post(model, false);
        }

        [HttpPost]
        public IHttpActionResult PostExtWithoutActivityEnd(AprobarResoluciondelaTarea6424ModelWithoutTrxId model)
        {
            return this._Post(model, false);
        }

        private IHttpActionResult _Post(AprobarResoluciondelaTarea6424Model model, bool bActivityEnd)
        {
            Dictionary<string, object> dicUpdateValues;
            Dictionary<string, object> dicConditionValues;
            string szAttributeValue = string.Empty;
            string szSetAttributes = string.Empty;

            using (TransactionScope transaction = new TransactionScope())
            {
                // Formularios
                dicConditionValues = new Dictionary<string, object>();

                if (model is AprobarResoluciondelaTarea6424ModelWithTrxId)
                    szAttributeValue = PectraOBPIHelper.AttributeGetValue(((AprobarResoluciondelaTarea6424ModelWithTrxId)model).TrxId, "FormularioId").Trim();
                if (string.IsNullOrWhiteSpace(szAttributeValue) == false)
                {
                    dicConditionValues.Add("FormularioId", szAttributeValue);
                }
                if (dicConditionValues.Count > 0)
                {
                    // Update
                    dicUpdateValues = new Dictionary<string, object>();
                    dicUpdateValues.Add("AC_Activo", model.Formularios.AC_Activo);
                    dicUpdateValues.Add("AC_Cliente", model.Formularios.AC_Cliente);
                    dicUpdateValues.Add("AC_Direccion", model.Formularios.AC_Direccion);
                    dicUpdateValues.Add("AC_Posicion", model.Formularios.AC_Posicion);
                    dicUpdateValues.Add("ATM_CRCargo", model.Formularios.ATM_CRCargo);
                    dicUpdateValues.Add("ATM_Direccion", model.Formularios.ATM_Direccion);
                    dicUpdateValues.Add("ATM_MarcaDeCajero", model.Formularios.ATM_MarcaDeCajero);
                    dicUpdateValues.Add("ATM_ModeloDeCajero", model.Formularios.ATM_ModeloDeCajero);
                    dicUpdateValues.Add("ATM_NichoDeMercado", model.Formularios.ATM_NichoDeMercado);
                    dicUpdateValues.Add("ATM_NombreATM", model.Formularios.ATM_NombreATM);
                    dicUpdateValues.Add("ATM_Territorio", model.Formularios.ATM_Territorio);
                    dicUpdateValues.Add("ATM_TipoDeUbicacion", model.Formularios.ATM_TipoDeUbicacion);
                    dicUpdateValues.Add("Fecha", model.Formularios.Fecha);
                    //dicUpdateValues.Add("FechaCargaTarea", model.Formularios.FechaCargaTarea);
                    dicUpdateValues.Add("Formulario", model.Formularios.Formulario);
                    dicUpdateValues.Add("GUID_Documentos", model.Formularios.GUID_Documentos);
                    dicUpdateValues.Add("GUID_FormPdf", model.Formularios.GUID_FormPdf);
                    dicUpdateValues.Add("GUID_Imagenes", model.Formularios.GUID_Imagenes);
                    dicUpdateValues.Add("GUID_Planos", model.Formularios.GUID_Planos);
                    dicUpdateValues.Add("Hora", model.Formularios.Hora);
                    dicUpdateValues.Add("IdResponsable", model.Formularios.IdResponsable);
                    dicUpdateValues.Add("Observacion", model.Formularios.Observacion);
                    dicUpdateValues.Add("ObservacionTemp", model.Formularios.ObservacionTemp);
                    dicUpdateValues.Add("SUC_Domicilio", model.Formularios.SUC_Domicilio);
                    dicUpdateValues.Add("SUC_NombreSucursal", model.Formularios.SUC_NombreSucursal);
                    dicUpdateValues.Add("SUC_Regional", model.Formularios.SUC_Regional);
                    dicUpdateValues.Add("SUC_Territorial", model.Formularios.SUC_Territorial);
                    dicUpdateValues.Add("SUC_TipoDeOficina", model.Formularios.SUC_TipoDeOficina);
                    dicUpdateValues.Add("ATM_FolioCitronella", model.Formularios.ATM_FolioCitronella);
                    dicUpdateValues.Add("ATM_IDCajero", model.Formularios.ATM_IDCajero);
                    FormulariosDAC.Update(dicUpdateValues, dicConditionValues);
                }
                else
                {
                    // Insert
                    FormulariosDAC.Insert(model.Formularios);
                }
  
                // Formularios - Pectra
                if (model.Formularios.DsEstado != null)
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("DsEstado", model.Formularios.DsEstado.ToString(), szSetAttributes, null);
                if (model.Formularios.FechaCargaTarea != null)
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("FechaCargaTarea", model.Formularios.FechaCargaTarea.ToString(), szSetAttributes, null);
                szSetAttributes = PectraOBPIHelper.AttributeSetValue("FormularioId", model.Formularios.FormularioId.ToString(), szSetAttributes, null);
                if (model.Formularios.TipoFormularioAgrupadoId != null)
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("TipoFormularioAgrupadoId", model.Formularios.TipoFormularioAgrupadoId.ToString(), szSetAttributes, null);
                if (model.Formularios.TipoFormularioId != null)
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("TipoFormularioId", model.Formularios.TipoFormularioId.ToString(), szSetAttributes, null);
                // Pectra
                string szTrxId = string.Empty;
                if (model is AprobarResoluciondelaTarea6424ModelWithTrxId)
                {
                    szTrxId = ((AprobarResoluciondelaTarea6424ModelWithTrxId)model).TrxId;
                }
                else if (model is AprobarResoluciondelaTarea6424ModelWithoutTrxId)
                {
                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    string SessionToken, DefaultLanguage;
                    PectraController.ExtractDataFromClaims(identity, out SessionToken, out DefaultLanguage);

                    AprobarResoluciondelaTarea6424ModelWithoutTrxId oModel = model as AprobarResoluciondelaTarea6424ModelWithoutTrxId;

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

                long trxId = long.Parse(szTrxId);

                // ActivityEnd
                if (bActivityEnd)
                    PectraOBPIHelper.ActivityEnd(trxId, szSetAttributes);
                else
                    PectraOBPIHelper.PackageDelOne(szTrxId);
              
                transaction.Complete();
		    } // Fin TransactionScope
              
            return Ok();
        
        }
    }
}

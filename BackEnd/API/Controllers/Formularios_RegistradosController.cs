
using PectraForms.WebApplication.BackEnd.API.Models;
using PectraForms.WebApplication.BackEnd.BusinessEntities;

using PectraForms.WebApplication.BackEnd.DataAccessComponents;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;
using PectraForms.WebApplication.BackEnd.API.Models.FormulariosRegistrados;
using System.Linq;
using System;
using PectraForms.WebApplication.BackEnd.API.Helpers;
using log4net.Repository.Hierarchy;
using System.Web.Script.Serialization;
using PectraForms.WebApplication.BackEnd.API.PectraBPReference;
using PectraForms.WebApplication.BackEnd.API.PectraGDReference;
using System.Configuration;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    //[Authorize]
    public partial class Formularios_RegistradosController : ApiController
    {
        /// <summary>
        /// Obtiene registros según filtro.
        /// </summary>
        /// <param name="filtro">Condiciones de búsqueda separadas por coma. Ej.:campo1:valor1,campo2:valor2.</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Formularios_RegistradosBE>))]
        [HttpGet]
        public IHttpActionResult GetByParam(ConditionsRequest filtro)
        {
            var oDicContions = new Dictionary<string, object>();
            if (filtro != null)
            {
                foreach (var item in filtro.conditionValues)
                    oDicContions.Add(item.field, item.value);

                return Ok(Formularios_RegistradosDAC.GetByParam(oDicContions));
            }
            else
                return BadRequest("Incorrect parameters.");
        }

        /// <summary>
        /// Obtiene todos los registros.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Formularios_RegistradosBE>))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(Formularios_RegistradosDAC.GetAll());
        }

        [ResponseType(typeof(List<Formularios_RegistradosBE>))]
        [HttpGet]
        public List<Formularios_RegistradosBE> ObtenerProcesadosNullDeJson()
        {                      
            return Formularios_RegistradosDAC.GetNoProcesados();
        }
        
        [ResponseType(typeof(List<Formularios_RegistradosBE>))]
        [HttpGet]
        public List<TiposFormularioMapeoBE> ObtenerTiposFormularioMapeos(int TipoFormularioId)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("TipoFormularioId", TipoFormularioId);
            return TiposFormularioMapeoDAC.GetByParam(conditions);
        }       
        [ResponseType(typeof(List<Formularios_RegistradosBE>))]
        [HttpGet]
        public void InsertarMapeosAformularios()
        {
            List<Formularios_RegistradosBE> registros = ObtenerProcesadosNullDeJson();

            foreach (var registro in registros)
            {
                string jsonFormData = registro.JsonFormData;
                JObject jsonData = JObject.Parse(jsonFormData);
                string formData = jsonData["form"].ToString();

                List<TiposFormularioMapeoBE> mapeos = ObtenerTiposFormularioMapeos(registro.TipoFormularioId);
                var idForm = jsonData["_id"].ToString();
                Dictionary<string, string> formularioDictionary = new Dictionary<string, string>();


                if (registro.TipoFormularioId == 6)
                {
                    var formDataObject = JsonConvert.DeserializeObject<FormDataSucursal>(formData);
                    

                    foreach (var page in formDataObject.Fields.Pages.Where(x => x.Elements != null))
                    {
                            foreach (var element in page.Elements)
                            {
                                foreach (var mapeo in mapeos)
                                {
                                    try
                                    {
                                        if (mapeo.FieldToLoad == element.FieldToLoad)
                                        {
                                            formularioDictionary.Add(mapeo.FieldToMap, element.Value);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                            }
                    }

                }
                else
                {
                    var formDataObject = JsonConvert.DeserializeObject<FormData>(formData);
                   
                    foreach (var page in formDataObject.Fields.Pages.Where(x => x.Elements != null))
                    {
                        foreach (var element in page.Elements.Where(x => x.Elements != null))
                        {
                            foreach (var element2level in element.Elements)
                            {
                                foreach (var mapeo in mapeos)
                                {
                                    try
                                    {
                                        if (mapeo.FieldToLoad == element2level.FieldToLoad)
                                        {
                                            formularioDictionary.Add(mapeo.FieldToMap, element2level.Value);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                

                var serializeDictionaryformulario = JsonConvert.SerializeObject(formularioDictionary);
                var formularioBE = JsonConvert.DeserializeObject<FormulariosBE>(serializeDictionaryformulario);
                formularioBE.FormId = idForm;
                formularioBE.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                formularioBE.Hora = DateTime.Now.ToString("HH:mm:ss");
                formularioBE.TipoFormularioId = registro.TipoFormularioId;
                formularioBE.FechaCargaTarea = DateTime.Now;

                var tipoformulariobe = TiposFormularioDAC.GetAll().Where(x => x.TipoFormularioId == registro.TipoFormularioId).FirstOrDefault();
                formularioBE.Formulario = tipoformulariobe != null ? tipoformulariobe.Nombre : string.Empty;

                formularioBE.DsEstado = "ASIGNAR-RESPONSABLE";

                FormulariosDAC.Insert(formularioBE);

                //SetearAtributosPectra(formularioBE);

                var dicUpdateConditions = new Dictionary<string, object>();
                dicUpdateConditions.Add("Id", registro.Id);

                var dicUpdateValues = new Dictionary<string, object>();
                dicUpdateValues.Add("Procesado", 1);
                dicUpdateValues.Add("ProcesadoFechaHora", DateTime.Now);

                Formularios_RegistradosDAC.Update(dicUpdateValues, dicUpdateConditions);
            }
        }
        [HttpPost]
        public void SetearAtributosPectra(FormulariosBE formularioBE)
        {
            var szSetAttributes = string.Empty;

            szSetAttributes = PectraOBPIHelper.AttributeSetValue("Fecha", formularioBE.Fecha ?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("Hora", formularioBE.Hora?.ToString() ?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNichoMercado", formularioBE.ATM_NichoDeMercado?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("FormularioId", formularioBE.FormularioId.ToString() ?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMIDCajero", formularioBE.ATM_IDCajero?.ToString() ?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMModeloDeCajero", formularioBE.ATM_ModeloDeCajero?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMMarcaDeCajero", formularioBE.ATM_MarcaDeCajero?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTipoDeOficina", formularioBE.SUC_TipoDeOficina?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCNombreSucursal", formularioBE.SUC_NombreSucursal?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCDomicilio", formularioBE.SUC_Domicilio?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTerritorial", formularioBE.SUC_Territorial?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCRegional", formularioBE.SUC_Regional?.ToString()?? string.Empty, szSetAttributes);
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTipoDeInmueble", formularioBE.SUC_TipoDeInmueble?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ACActivo", formularioBE.AC_Activo?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ACCliente", formularioBE.AC_Cliente?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("ACDireccion", formularioBE.AC_Direccion?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("Formulario", formularioBE.Formulario?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("TipoFomularioAgrupadoId", formularioBE.TipoFormularioAgrupadoId?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("TerritorioId", formularioBE.TerritorioId?.ToString()?? string.Empty, szSetAttributes);                    
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("DsEstado", "ASIGNAR-RESPONSABLE", szSetAttributes);                                     
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("FechaCargaTarea", formularioBE.FechaCargaTarea?.ToString()?? string.Empty, szSetAttributes);                                     
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("NomResponsable", string.Empty, szSetAttributes);                                     
            szSetAttributes = PectraOBPIHelper.AttributeSetValue("UsridSolicitante", string.Empty, szSetAttributes);                    


            var newTrxId = GenerarTrxId();

            PectraOBPIHelper.ActivityEnd(newTrxId, szSetAttributes);
        }
        [HttpGet]
        public long GenerarTrxId()
        {
            long newTrxid = 0; 

            try
            {
                IGDService service = new GDServiceClient("WSHttpBinding_IGDService");
                var usrId = ConfigurationManager.AppSettings["UsrId"];
                var password = ConfigurationManager.AppSettings["UsrPwd"];
                var languageId = ConfigurationManager.AppSettings["languaje"];

                LoginRequest loginRequest = new LoginRequest();
                loginRequest.UsrId = usrId;
                loginRequest.UsrPwd = password;
                loginRequest.LanguageId = languageId;
                LoginResponse loginResponse = service.Login(loginRequest);

                BPServiceClient bpServiceClient = new BPServiceClient("WSHttpBinding_IBPService");
                var request = new GetNewTrxIdRequest();

                request.SessionToken = loginResponse.SessionToken;
                request.UsrId = usrId;
                request.LanguageId = languageId;
                request.OrgId = ConfigurationManager.AppSettings.Get("OrgId");
                request.UoId = ConfigurationManager.AppSettings.Get("UoId");
                request.ProfId = ConfigurationManager.AppSettings.Get("ProfId");
                request.ActId = long.Parse(ConfigurationManager.AppSettings.Get("ActId"));
                request.InsId = long.Parse(ConfigurationManager.AppSettings.Get("InsId"));
                request.FunctionId = long.Parse(ConfigurationManager.AppSettings.Get("FunctionId"));
                request.PkgId = long.Parse(ConfigurationManager.AppSettings.Get("PkgId"));
                request.PngId = long.Parse(ConfigurationManager.AppSettings.Get("PngId"));
                request.SubProId = long.Parse(ConfigurationManager.AppSettings.Get("SubProId"));
                request.VersionId = long.Parse(ConfigurationManager.AppSettings.Get("VersionId"));
                request.Blocked = bool.Parse(ConfigurationManager.AppSettings.Get("Blocked"));

                var response = bpServiceClient.GetNewTrxId(request);

                newTrxid = response.TrxId;
            }
            catch (Exception ex)
            {
                //Logger.Error($"ERR:GenerarTrxId: usrId:{usrId}, languageId:{languageId}, orgId:{orgId}, uoId:{uoId}, profId:{profId}, actId:{actId}, insId:{insId}, fncId:{fncId}, pkgId:{pkgId}, pngId:{pngId}, subProId:{subProId}, version:{version}, false)", ex); throw ex;
            }

            return newTrxid;
        }
    }
}


       

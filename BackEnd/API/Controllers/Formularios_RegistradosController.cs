
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

        //[ResponseType(typeof(List<Formularios_RegistradosBE>))]
        //[HttpGet]
        //public ActionResult ObtenerDatosJson()
        //{   //Obtengo los registros de la tabla Formularios_Registrados
        //    List<Formularios_RegistradosBE> registros = db.Formularios_registrados.ToList();
        //    //Creo una lista para almacenar los datos del json
        //    List<FormulariosBE> datosJson = new List<FormulariosBE>();
        //    //Recorro los registros y extraigo los datos del json
        //    foreach(var registro in registros)
        //    {
        //        FormulariosBE formulariosBE = JsonConvert.DeserializeObject<formulariosBE>(registro.JsonFormData);
        //        //Agrego el objeto FormulariosBE a la lista
        //        datosJson.Add(formulariosBE);
        //    }
        //    return Ok(Formularios_RegistradosDAC.GetAll());
        //}
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
                var formDataObject = JsonConvert.DeserializeObject<FormData>(formData);
                var idForm = jsonData["_id"].ToString();

                Dictionary<string, string> formularioDictionary = new Dictionary<string, string>();

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

                var serializeDictionaryformulario = JsonConvert.SerializeObject(formularioDictionary);
                var formularioBE = JsonConvert.DeserializeObject<FormulariosBE>(serializeDictionaryformulario);
                formularioBE.FormId = idForm;
                formularioBE.Fecha = DateTime.Now;
                formularioBE.Hora = DateTime.Now.ToString("HH:mm:ss");
                formularioBE.Formulario = "no especificado";
                formularioBE.TipoFormularioId = registro.TipoFormularioId;
                FormulariosDAC.Insert(formularioBE);

                SetearAtributosPectra(formularioBE);

                var dicUpdateConditions = new Dictionary<string, object>();
                dicUpdateConditions.Add("Id", registro.Id);

                var dicUpdateValues = new Dictionary<string, object>();
                dicUpdateValues.Add("Procesado", 1);
                dicUpdateValues.Add("Fecha", DateTime.Now);
                dicUpdateValues.Add("Hora", DateTime.Now.ToString("HH:mm:ss"));

                Formularios_RegistradosDAC.Update(dicUpdateValues, dicUpdateConditions);
            }
        }

        public void SetearAtributosPectra(FormulariosBE formularioBE)
        {
            var szSetAttributes = string.Empty;

            switch (formularioBE.TipoFormularioId)
            {
                case 1:
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella != null? formularioBE.ATM_FolioCitronella.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM != null?formularioBE.ATM_NombreATM.ToString() : string.Empty, szSetAttributes);                   
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio != null ?formularioBE.ATM_Territorio.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion != null ?formularioBE.ATM_TipoDeUbicacion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo != null ? formularioBE.ATM_CRCargo.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion!= null ? formularioBE.ATM_Direccion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNichoMercado", formularioBE.ATM_NichoDeMercado != null ? formularioBE.ATM_NichoDeMercado.ToString(): string.Empty, szSetAttributes);
                    
                    break;

                case 2:
                     szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella != null? formularioBE.ATM_FolioCitronella.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM != null?formularioBE.ATM_NombreATM.ToString() : string.Empty, szSetAttributes);                   
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio != null ?formularioBE.ATM_Territorio.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion != null ?formularioBE.ATM_TipoDeUbicacion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo != null ? formularioBE.ATM_CRCargo.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion!= null ? formularioBE.ATM_Direccion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNichoMercado", formularioBE.ATM_NichoDeMercado != null ? formularioBE.ATM_NichoDeMercado.ToString(): string.Empty, szSetAttributes);
                    break;

                case 3:
                     szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella != null? formularioBE.ATM_FolioCitronella.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM != null?formularioBE.ATM_NombreATM.ToString() : string.Empty, szSetAttributes);                   
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio != null ?formularioBE.ATM_Territorio.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion != null ?formularioBE.ATM_TipoDeUbicacion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo != null ? formularioBE.ATM_CRCargo.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion!= null ? formularioBE.ATM_Direccion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNichoMercado", formularioBE.ATM_NichoDeMercado != null ? formularioBE.ATM_NichoDeMercado.ToString(): string.Empty, szSetAttributes);
                    break;

                case 4:
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella != null? formularioBE.ATM_FolioCitronella.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM != null?formularioBE.ATM_NombreATM.ToString() : string.Empty, szSetAttributes);                   
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio != null ?formularioBE.ATM_Territorio.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion != null ?formularioBE.ATM_TipoDeUbicacion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo != null ? formularioBE.ATM_CRCargo.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion!= null ? formularioBE.ATM_Direccion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMIDCajero", formularioBE.ATM_IDCajero != null ? formularioBE.ATM_IDCajero.ToString(): string.Empty, szSetAttributes);
                    break;

                case 5:
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMFolioCitronella", formularioBE.ATM_FolioCitronella != null? formularioBE.ATM_FolioCitronella.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNombreATM", formularioBE.ATM_NombreATM != null?formularioBE.ATM_NombreATM.ToString() : string.Empty, szSetAttributes);                   
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTerritorio", formularioBE.ATM_Territorio != null ?formularioBE.ATM_Territorio.ToString() : string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMTipoDeUbicacion", formularioBE.ATM_TipoDeUbicacion != null ?formularioBE.ATM_TipoDeUbicacion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMCRCargo", formularioBE.ATM_CRCargo != null ? formularioBE.ATM_CRCargo.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMDireccion", formularioBE.ATM_Direccion!= null ? formularioBE.ATM_Direccion.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMNichoMercado", formularioBE.ATM_NichoDeMercado != null ? formularioBE.ATM_NichoDeMercado.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMIDCajero", formularioBE.ATM_IDCajero != null ? formularioBE.ATM_IDCajero.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMModeloDeCajero", formularioBE.ATM_ModeloDeCajero != null ? formularioBE.ATM_ModeloDeCajero.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("ATMMarcaDeCajero", formularioBE.ATM_MarcaDeCajero != null ? formularioBE.ATM_MarcaDeCajero.ToString(): string.Empty, szSetAttributes);
                   break;

                case 6:
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTipoDeOficina", formularioBE.SUC_TipoDeOficina != null ? formularioBE.SUC_TipoDeOficina.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCNombreSucursal", formularioBE.SUC_NombreSucursal != null ? formularioBE.SUC_NombreSucursal.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCDomicilio", formularioBE.SUC_Domicilio != null ? formularioBE.SUC_Domicilio.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTerritorial", formularioBE.SUC_Territorial != null ? formularioBE.SUC_Territorial.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCRegional", formularioBE.SUC_Regional != null ? formularioBE.SUC_Regional.ToString(): string.Empty, szSetAttributes);
                    szSetAttributes = PectraOBPIHelper.AttributeSetValue("SUCTipoDeInmueble", formularioBE.SUC_TipoDeInmueble != null ? formularioBE.SUC_TipoDeInmueble.ToString(): string.Empty, szSetAttributes);                    
                    break;
               
                default:
                    
                    break;
            }
        }
    }   
}


       

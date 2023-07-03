
using PectraForms.WebApplication.BackEnd.API.Models;
using PectraForms.WebApplication.BackEnd.BusinessEntities;

using PectraForms.WebApplication.BackEnd.DataAccessComponents;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

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
                JObject form = JObject.Parse(formData);

                List<TiposFormularioMapeoBE> mapeos = ObtenerTiposFormularioMapeos(registro.TipoFormularioId);
                var serializeRegistro = JsonConvert.SerializeObject(form);
                var registroDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializeRegistro);
                Dictionary<string, string> formularioDictionary = new Dictionary<string, string>();
                foreach (var mapeo in mapeos)
                {
                                       
                    formularioDictionary.Add(mapeo.FieldToMap, registroDictionary[mapeo.FieldToLoad].ToString());                    
                }
                var serializeDictionaryformulario = JsonConvert.SerializeObject(formularioDictionary);
                var formularios = JsonConvert.DeserializeObject<FormulariosBE>(serializeDictionaryformulario);
                FormulariosDAC.Insert(formularios);                                            
            }
        }       
    }
}


       

 
using PectraForms.WebApplication.BackEnd.API.Models;
using PectraForms.WebApplication.BackEnd.BusinessEntities;
using PectraForms.WebApplication.BackEnd.DataAccessComponents;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    [Authorize]
    public partial class BooleBox_ArchivosController : ApiController
    {
        /// <summary>
        /// Obtiene registros según filtro.
        /// </summary>
        /// <param name="filtro">Condiciones de búsqueda separadas por coma. Ej.:campo1:valor1,campo2:valor2.</param>
        /// <returns></returns>
        [ResponseType(typeof(List<BooleBox_ArchivosBE>))]
        [HttpGet]
        public IHttpActionResult GetByParam(ConditionsRequest filtro)
        {
            var oDicContions = new Dictionary<string, object>();
            if (filtro != null)
            {
                foreach (var item in filtro.conditionValues)
                    oDicContions.Add(item.field, item.value);

                return Ok(BooleBox_ArchivosDAC.GetByParam(oDicContions));
            }
            else
                return BadRequest("Incorrect parameters.");
        }

        /// <summary>
        /// Obtiene todos los registros.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<BooleBox_ArchivosBE>))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(BooleBox_ArchivosDAC.GetAll());
        }

    }
}


using Newtonsoft.Json;
using PectraForms.WebApplication.BackEnd.API.Models;
using PectraForms.WebApplication.BackEnd.BusinessEntities;
using PectraForms.WebApplication.BackEnd.DataAccessComponents;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    [Authorize]
    public class AttachmentsController : ApiController
    {
        /// <summary>
        /// Obtiene registros según filtro.
        /// </summary>
        /// <param name="filtro">Condiciones de búsqueda separadas por coma. Ej.:campo1:valor1,campo2:valor2.</param>
        /// <returns></returns>
        [ResponseType(typeof(List<AdjuntosBE>))]
        [HttpGet]
        public IHttpActionResult GetByParam(ConditionsRequest filtro)
        {
            var oDicContions = new Dictionary<string, object>();
            if (filtro != null)
            {
                List<AttachmentModel> lstAttachmentsReturn = new List<AttachmentModel>();

                foreach (var item in filtro.conditionValues)
                    oDicContions.Add(item.field, item.value);

                List<AdjuntosBE> lstAdjuntos = AdjuntosDAC.GetByParam(oDicContions);
                if (lstAdjuntos != null)
                {
                    foreach (AdjuntosBE oAdjuntoBE in lstAdjuntos)
                    {
                        AttachmentModel oAttachmentReturn = new AttachmentModel();
                        oAttachmentReturn.IdAdjunto = oAdjuntoBE.IdAdjunto;
                        oAttachmentReturn.description = oAdjuntoBE.Descripcion;
                        oAttachmentReturn.filename = oAdjuntoBE.NombreArchivo;

                        lstAttachmentsReturn.Add(oAttachmentReturn);
                    }
                }
                return Ok(lstAttachmentsReturn);
            }
            else
                return BadRequest("Incorrect parameters.");
        }

        internal class AttachmentModel
        {
            public int IdAdjunto { get; set; }
            public string description { get; set; }
            public string filename { get; set; }
        }


        [HttpPost]
        public IHttpActionResult Upload()
        {
            int iNewIdAdjunto = 0;
            if (HttpContext.Current.Request.Files["ArchivoASubir"] != null)
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["ArchivoASubir"];

                if (postedFile.FileName.Trim().Length > 0 && postedFile.ContentLength > 0)
                {
                    byte[] oArchivo = new byte[0];
                    using (BinaryReader reader = new BinaryReader(postedFile.InputStream))
                    {
                        oArchivo = new byte[postedFile.ContentLength];
                        reader.Read(oArchivo, 0, oArchivo.Length);
                    }

                    AdjuntosBE oAdjuntos = new AdjuntosBE();
                    oAdjuntos.IdGrupo = HttpContext.Current.Request.Form["IdGrupo"];
                    oAdjuntos.Descripcion = HttpContext.Current.Request.Form["Descripcion"];
                    oAdjuntos.Archivo = oArchivo;
                    oAdjuntos.NombreArchivo = Path.GetFileName(postedFile.FileName);
                    oAdjuntos.Tipo = "Otros";
                    oAdjuntos.IdUsuario = string.Empty;
                    oAdjuntos.IdPerfil = string.Empty;
                    oAdjuntos.IdRelacion = 0;
                    AdjuntosDAC.Insert(oAdjuntos);

                    iNewIdAdjunto = oAdjuntos.IdAdjunto;
                }
            }
            return Ok(iNewIdAdjunto);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int idAdjunto)
        {
            AdjuntosDAC.Delete(idAdjunto);
            return Ok();
        }

        [HttpGet]
        public HttpResponseMessage Download(int idAdjunto)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            Dictionary<string, object> dicConditionValues = new Dictionary<string, object>();
            dicConditionValues.Add("IdAdjunto", idAdjunto);
            List<AdjuntosBE> lstAdjuntos = AdjuntosDAC.GetByParam(dicConditionValues);

            if (lstAdjuntos != null && lstAdjuntos.Any())
            {
                var stream = new MemoryStream(lstAdjuntos[0].Archivo);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = Uri.EscapeDataString(lstAdjuntos[0].NombreArchivo)
                    };
                result.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            }
            return result;
        }

    }

}

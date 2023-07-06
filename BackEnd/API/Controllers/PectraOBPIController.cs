using Newtonsoft.Json;
using PectraForms.WebApplication.BackEnd.API.Helpers;
using PectraForms.WebApplication.BackEnd.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PectraForms.WebApplication.BackEnd.API.Controllers
{
    public class PectraOBPIController : ApiController
    {
        /// <summary>
        /// Obtener el valor de un atributo.
        /// </summary>
        /// <param name="trxId">Id de transacción.</param>
        /// <param name="atrId">Id de atributo.</param>
        /// <returns></returns>
        /// <response code="200"></response>
        [ResponseType(typeof(PectraAttribute))]
        [Route("PectraOBPI/AttributeGetValue/{trxId}/{atrId}")]
        [HttpGet]
        public IHttpActionResult AttributeGetValue(string trxId, string atrId)
        {
            PectraAttribute oAtt = new PectraAttribute() { AtrId = atrId };
            oAtt.AtrValue = PectraOBPIHelper.AttributeGetValue(trxId, atrId).Trim();
            return Ok(oAtt);
        }

        // POST: api/InstanceLockUnLock
        /// <summary>
        /// Bloquea / desbloquea una instancia.
        /// </summary>
        [ResponseType(typeof(long))]
        [HttpPost]
        public IHttpActionResult InstanceLockUnLock(InstanceLockUnLockApiRequest request)
        {
            long iRet = PectraOBPIHelper.InstanceLockUnLock(request.trxId, request.lockState);
            return Ok(iRet);
        }

        /// <summary>
        /// Datos de entrada para hacer el InstanceLockUnLock
        /// </summary>
        public class InstanceLockUnLockApiRequest : BaseApiRequest
        {
            [JsonProperty(Order = -19)]
            public bool lockState { get; set; }
        }

        // POST: api/InstanceLockUnLockExt
        /// <summary>
        /// Bloquea / desbloquea una instancia.
        /// </summary>
        [ResponseType(typeof(long))]
        [HttpPost]
        public IHttpActionResult InstanceLockUnLockWithUsr(InstanceLockUnLockWithUsrApiRequest request)
        {
            long iRet = PectraOBPIHelper.InstanceLockUnLock(request.trxId, request.lockState, request.usrLock);
            return Ok(iRet);
        }

        /// <summary>
        /// Datos de entrada para hacer el InstanceLockUnLock con UsrLock
        /// </summary>
        public class InstanceLockUnLockWithUsrApiRequest : InstanceLockUnLockApiRequest
        {
            [JsonProperty(Order = -18)]
            public string usrLock { get; set; }
        }


        // POST: api/InstanceLockUnLockExt
        /// <summary>
        /// Bloquea / desbloquea una instancia.
        /// </summary>
        [ResponseType(typeof(long))]
        [HttpPost]
        public IHttpActionResult InstanceLockUnLockExt(InstanceLockUnLockExtApiRequest request)
        {
            long iRet = PectraOBPIHelper.InstanceLockUnLock(request.trxId, request.lockState, request.usrLock, request.pngId, request.versionId,
                request.subProId, request.actId, request.insId);
            return Ok(iRet);
        }

        /// <summary>
        /// Datos de entrada para hacer el InstanceLockUnLock extendido
        /// </summary>
        public class InstanceLockUnLockExtApiRequest : InstanceLockUnLockWithUsrApiRequest
        {
            public int pngId { get; set; }
            public int versionId { get; set; }
            public int subProId { get; set; }
            public int actId { get; set; }
            public int insId { get; set; }
        }

        // POST: api/ActivityEnd
        /// <summary>
        /// Finaliza una instancia.
        /// </summary>
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult ActivityEnd(ActivityEndApiRequest request)
        {
            string setAttributes = string.Empty;
            foreach (PectraAttribute attr in request.attributes)
                setAttributes = PectraOBPIHelper.AttributeSetValue(attr.AtrId, attr.AtrValue, setAttributes, attr.ExtendedValue);

            long trxId = long.Parse(request.trxId);

            return Ok(PectraOBPIHelper.ActivityEnd(trxId, setAttributes, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 3, false));
        }

        public class ActivityEndApiRequest : BaseApiRequest
        {
            [JsonProperty(Order = -19)]
            public List<PectraAttribute> attributes { get; set; }
        }


        // POST: api/ActivityEnd
        /// <summary>
        /// Finaliza una instancia.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult ActivityEndExt(ActivityEndExtApiRequest request)
        {
            string setAttributes = string.Empty;
            foreach (PectraAttribute attr in request.attributes)
                setAttributes = PectraOBPIHelper.AttributeSetValue(attr.AtrId, attr.AtrValue, setAttributes, attr.ExtendedValue);

            string szRet = "";

            long trxId = long.Parse(request.trxId);

            szRet = PectraOBPIHelper.ActivityEnd(trxId, setAttributes, request.trxIdExt, request.pngId, request.subProId, request.actId,
                request.insId, request.pngInsId, request.usrId, request.insOrigin, request.priority, request.attachmentsDocuments);
            return Ok(szRet);
        }

        /// <summary>
        /// Datos de entrada para hacer el ActivityEnd
        /// </summary>
        public class ActivityEndExtApiRequest : ActivityEndApiRequest
        {
            public long trxIdExt { get; set; }
            public long pngId { get; set; }
            public long subProId { get; set; }
            public long actId { get; set; }
            public long insId { get; set; }
            public long pngInsId { get; set; }
            public string usrId { get; set; }
            public string insOrigin { get; set; }
            public long priority { get; set; }
            public bool attachmentsDocuments { get; set; }
        }

        // POST: api/PackageDelOne
        /// <summary>
        /// Elimina un package.
        /// </summary>
        [ResponseType(typeof(long))]
        [HttpPost]
        public IHttpActionResult PackageDelOne(BaseApiRequest request)
        {
            bool bRet = PectraOBPIHelper.PackageDelOne(request.trxId);
            return Ok(bRet);
        }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Models
{
    public class BaseApiRequest
    {
        [JsonProperty(Order = -20)]
        public string trxId { get; set; }
    }

}
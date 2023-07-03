using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Models
{
    public class PectraAttribute
    {
        public string AtrId { get; set; }
        public string AtrValue { get; set; }
        public object ExtendedValue { get; set; }
    }
}
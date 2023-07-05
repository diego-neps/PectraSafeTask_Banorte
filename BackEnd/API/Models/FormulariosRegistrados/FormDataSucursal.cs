using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Models.FormulariosRegistrados
{
    public class FormDataSucursal
    {
        public string _Id { get; set; }
        public FieldsSucursal Fields { get; set; }
    }
    public class FieldsSucursal
    {
        public List<PageFormDataSucursal> Pages { get; set; }
    }

    public class PageFormDataSucursal
    {
        public List<ElementsSucursal> Elements { get; set; }
    }

    public class ElementsSucursal
    {
        public string FieldToLoad { get; set; }
        public string Value { get; set; }
    }

}
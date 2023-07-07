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
        //public string Value { get; set; }
        public List<ElementsSucursal> Elements { get; set; }

        [JsonProperty("value")]
        public object ValueObject { get; set; }

        public Dictionary<string, string> ValueDictionary { get {
                if (this.ValueObject == null) return null;

                string strValueObject = this.ValueObject.ToString();
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strValueObject);
                }
                catch (Exception)
                {
                    return null;
                }
            } }

        public string ValueText { get {
                if (this.ValueObject == null || this.ValueDictionary != null) return null;

                return this.ValueObject.ToString();
            } }

    }

}
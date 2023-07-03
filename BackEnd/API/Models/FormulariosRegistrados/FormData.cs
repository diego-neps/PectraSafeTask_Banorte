using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Models.FormulariosRegistrados
{
    public class FormData
    {
        public string _Id { get; set; }
        public FieldsFormData Fields { get; set; }
    }
    public class FieldsFormData
    {
        public List<PageFormData> Pages { get; set; }
    }

    public class PageFormData
    {
        public List<ElementsFormData> Elements { get; set; }
    }

    public class ElementsFormData
    {
        public List<Elements2ndLevelFormData> Elements { get; set; }
    }

    public class Elements2ndLevelFormData
    {
        public string FieldToLoad { get; set; }
        public string Value { get; set; }
    }
}
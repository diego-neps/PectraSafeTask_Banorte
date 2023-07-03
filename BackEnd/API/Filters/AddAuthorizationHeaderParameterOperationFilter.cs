using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace PectraForms.WebApplication.BackEnd.API.Filters
{
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "access token (Ej.: Bearer oITo...ANvfZQ)",
                required = true,
                type = "string"
            });

        }
    }
}
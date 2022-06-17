using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Mkh.Utils.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mkh.Host.Web.Swagger.Filters
{
    public class SwaggerIgnoreOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters().SelectMany(p => p.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<SwaggerIgnoreAttribute>() != null)).ToList();

            foreach (var property in ignoredProperties)
            {
                //只处理一级属性，复杂类型不处理了，费脑子~
                operation.Parameters = operation.Parameters.Where(p => !p.Name.Split('.')[0].StartsWith(property.Name, StringComparison.InvariantCulture)).ToList();
            }
        }
    }
}

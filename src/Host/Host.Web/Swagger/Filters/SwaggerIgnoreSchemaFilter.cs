using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Mkh.Utils.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mkh.Host.Web.Swagger.Filters
{
    /// <summary>
    /// 过滤输出参数中的忽略属性
    /// </summary>
    public class SwaggerIgnoreSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var ignoreDataMemberProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

            foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
            {
                var propertyToHide = schema.Properties.Keys.SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                if (propertyToHide != null)
                {
                    schema.Properties.Remove(propertyToHide);
                }
            }
        }
    }
}

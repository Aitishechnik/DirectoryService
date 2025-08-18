using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum = Enum.GetNames(context.Type)
                              .Select(n => new OpenApiString(n))
                              .Cast<IOpenApiAny>()
                              .ToList();
            schema.Type = "string";
            schema.Format = null;
        }
    }
}
using Swashbuckle.Swagger;
using System;
using System.Linq;
using System.Reflection;

//https://stackoverflow.com/questions/41005730/how-to-configure-swashbuckle-to-ignore-property-on-model
namespace medikeeper_api
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        #region ISchemaFilter Members

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema?.properties == null || type == null)
                return;

            var excludedProperties = type.GetProperties()
                                         .Where(t =>
                                                t.GetCustomAttribute<SwaggerExcludeAttribute>()
                                                != null);

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.properties.ContainsKey(excludedProperty.Name))
                    schema.properties.Remove(excludedProperty.Name);
            }
        }

        #endregion
    }
}
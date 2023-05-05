using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Linq;
using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class JsonSchemasValidatorService : ISchemasValidatorService
    {
        private readonly ISchemasStorageService _schemasStorage;
        public JsonSchemasValidatorService(ISchemasStorageService schemasStorage)
        {
            _schemasStorage = schemasStorage;
        }
        public bool ValidateBySchema(ControllerActionDescriptor descriptor, ApiVersion apiVersion, string content)
        {
            string version = apiVersion.ToString().Length == 1 ? apiVersion + ".0" : apiVersion.ToString();
            string relPath = $@"\V{version}\{descriptor.ControllerName}\{descriptor.ActionName}.json";
            var schema = _schemasStorage.SchemasData.FirstOrDefault(t => 
                t.Key.Contains(relPath, StringComparison.CurrentCultureIgnoreCase));

            JSchema jSchema = JSchema.Parse(schema.Value);
            JObject jContent = JObject.Parse(content);

            return jContent.IsValid(jSchema);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Linq;
using System.Text;
using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class JsonSchemasValidatorService : ISchemasValidatorService
    {
        private readonly ISchemasStorageService _schemasStorage;
        private readonly string _schemasPathTemplate;
        public JsonSchemasValidatorService(ISchemasStorageService schemasStorage, string schemasPathTemplate)
        {
            _schemasStorage = schemasStorage;
            _schemasPathTemplate = schemasPathTemplate;
        }
        public bool ContentIsValidBySchema(ControllerActionDescriptor descriptor, ApiVersion apiVersion, string content)
        {
            string version = apiVersion.ToString().Length == 1 ? apiVersion + ".0" : apiVersion.ToString();
            string relPath = InsertValuesInTemplate(version, descriptor.ControllerName, descriptor.ActionName);
            var schema = _schemasStorage.SchemasData.FirstOrDefault(t => 
                t.Key.Contains(relPath, StringComparison.CurrentCultureIgnoreCase));

            JSchema jSchema = JSchema.Parse(schema.Value);
            JObject jContent = JObject.Parse(content);

            return jContent.IsValid(jSchema);
        }
        private string InsertValuesInTemplate(string version, string controller, string action)
        {
            StringBuilder res = new StringBuilder(_schemasPathTemplate);
            res.Replace("{version}", version);
            res.Replace("{controller}", controller);
            res.Replace("{action}", action);
            return res.ToString();
        }
    }
}

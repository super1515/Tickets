using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Text;
using Tickets.WebAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Tickets.BAL.Options.Implementations;
/*
* 
* Сервис для валидации тела запроса JSON схемой
* 
*/
namespace Tickets.WebAPI.Services.Implementations
{
    public class JsonSchemasValidatorService : ISchemasValidatorService
    {
        private readonly IOptions<JsonSchemas> _schemasStorage;
        private readonly string _schemasPathTemplate;
        public JsonSchemasValidatorService(IOptions<JsonSchemas> schemasStorage, string schemasPathTemplate)
        {
            _schemasStorage = schemasStorage;
            _schemasPathTemplate = schemasPathTemplate;
        }
        public bool ContentIsValidBySchema(ControllerActionDescriptor descriptor, ApiVersion apiVersion, string content)
        {
            string version = apiVersion.ToString().Length == 1 ? apiVersion + ".0" : apiVersion.ToString();
            string relPath = InsertValuesInTemplate(version, descriptor.ControllerName, descriptor.ActionName);
            var schema = _schemasStorage.Value.GetBy(relPath)!.Data;
            JSchema jSchema = JSchema.Parse(schema);
            JObject jContent;
            try
            {
                jContent = JObject.Parse(content);
            }
            catch (JsonReaderException)
            {
                return false;
            }
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

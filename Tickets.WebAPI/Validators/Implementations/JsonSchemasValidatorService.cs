using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Json.Schema;
using System.Text.Json;
using Tickets.WebAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Tickets.BAL.Options.Implementations;
using System.Text;
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
            var schema = JsonSchema.FromText(_schemasStorage.Value.GetBy(relPath)!.Data);
            var json = JsonDocument.Parse(content);

            var result = schema.Validate(json);
            return result.IsValid;
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

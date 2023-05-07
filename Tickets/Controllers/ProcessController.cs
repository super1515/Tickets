using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Runtime.Versioning;
using System.Text;
using Tickets.Dto;
using Tickets.Services.Interfaces;

namespace Tickets.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class ProcessController : ControllerBase
    {
        private readonly ILogger<ProcessController> _logger;
        private readonly ISchemasValidatorService _schemasValidator;
        public ProcessController(ISchemasValidatorService schemasValidator, ILogger<ProcessController> logger)
        {
            _schemasValidator = schemasValidator;
            _logger = logger;
        }
        [HttpPost]
        public ActionResult<string> Sale(ApiVersion version, [FromBody] SaleRequestDto content)
        {
            Console.WriteLine(content.OperationType);
            return Ok(content);
            //return _schemasValidator.ContentIsValidBySchema(ControllerContext.ActionDescriptor, version, "{\r\n    \"operation_type\": \"refund\",\r\n    \"operation_time\": \"2022-01-01T03:25+03:00\",\r\n    \"operation_place\": \"Aeroflot\",\r\n    \"ticket_number\": \"5552139265672\"\r\n}\r\n").ToString();
        }
        [HttpGet]
        public ActionResult Refund([FromBody] RefundRequestDto content)
        {
            return Conflict();
        }
    }
}
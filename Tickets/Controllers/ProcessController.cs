using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Runtime.Versioning;
using System.Text;
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
        [HttpGet]
        public ActionResult<string> Sale(ApiVersion version, string content)
        {
            return _schemasValidator.ValidateBySchema(ControllerContext.ActionDescriptor, version, content).ToString();
        }
        [HttpGet]
        public ActionResult Refund()
        {
            return Ok(null);
        }
    }
}
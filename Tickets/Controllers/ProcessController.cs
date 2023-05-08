using Microsoft.AspNetCore.Mvc;
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
        private readonly IProcessService _process;
        public ProcessController(ISchemasValidatorService schemasValidator, ILogger<ProcessController> logger, IProcessService process)
        {
            _schemasValidator = schemasValidator;
            _logger = logger;
            _process = process;
        }
        [HttpPost]
        public async Task<ActionResult> Sale(ApiVersion version, [FromBody] SaleRequestDto content)
        {
            return await _process.CreateSegments(content);
            //return Ok();
            //return _schemasValidator.ContentIsValidBySchema(ControllerContext.ActionDescriptor, version, "{\r\n    \"operation_type\": \"refund\",\r\n    \"operation_time\": \"2022-01-01T03:25+03:00\",\r\n    \"operation_place\": \"Aeroflot\",\r\n    \"ticket_number\": \"5552139265672\"\r\n}\r\n").ToString();
        }
        [HttpGet]
        public async Task<ActionResult> Refund([FromBody] RefundRequestDto content)
        {
            return await _process.RefundSegments(content);
            //return Conflict();
        }
    }
}
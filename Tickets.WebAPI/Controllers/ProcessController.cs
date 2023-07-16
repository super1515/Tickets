using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tickets.BAL.Dto;
using Tickets.WebAPI.Filters;
using Tickets.BAL.Services.Interfaces;
using Tickets.BAL.Utility;
using Tickets.WebAPI.Models;

namespace Tickets.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [RequestSizeLimit(2048)]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _process;
        public ProcessController(IProcessService process)
        {
            _process = process;
        }
        [HttpPost]
        [ValidateWithJsonSchemeFilter(HttpStatusCode.BadRequest, null)]
        public async Task<ActionResult<int>> SaleAsync([FromBody] SaleRequestDto content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail(ResponseMessages.RequestInNotValidMsg));

            await _process.CreateSegmentsAsync(content);
            return Ok(ApiResponse<string>.Success(null, ResponseMessages.SaleSuccessMsg));
        }
        [HttpPost]
        [ValidateWithJsonSchemeFilter(HttpStatusCode.BadRequest, null)]
        public async Task<ActionResult> RefundAsync([FromBody] RefundRequestDto content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail(ResponseMessages.RequestInNotValidMsg));

            bool successRefund = await _process.RefundSegmentsAsync(content);
            if (!successRefund)
                return Conflict(ApiResponse<string>.Fail(ResponseMessages.RefundIsNotSuccessMsg));
            return Ok(ApiResponse<string>.Success(null, ResponseMessages.RefundSuccessMsg));
        }
    }
}
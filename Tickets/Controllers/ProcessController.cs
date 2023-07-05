using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tickets.Application.Dto;
using Tickets.WebAPI.Filters;
using Tickets.Infrastructure.Models;
using Tickets.Infrastructure.Services.Interfaces;
using Tickets.Application.Utility;

namespace Tickets.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [RequestSizeLimit(2048)]
    public class ProcessController : ControllerBase
    {
        private readonly IMappingService _mapper;
        private readonly IProcessService _process;
        public ProcessController(IMappingService mapper, IProcessService process)
        {
            _mapper = mapper;
            _process = process;
        }
        [HttpPost]
        [ValidateWithJsonSchemeFilter(HttpStatusCode.BadRequest, null)]
        public async Task<ActionResult<int>> SaleAsync([FromBody] SaleRequestDto content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail(ResponseMessages.RequestInNotValidMsg));

            await _process.CreateSegmentsAsync(_mapper.Map(content));
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
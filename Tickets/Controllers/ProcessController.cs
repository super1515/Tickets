using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tickets.Dto;
using Tickets.Filters;
using Tickets.Models;
using Tickets.Services.Interfaces;

namespace Tickets.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class ProcessController : ControllerBase
    {
        private readonly IMappingService _mapper;
        private readonly IProcessService _process;
        private const string requestInNotValidMsg = "Request is not valid!";
        private const string refundIsNotSuccessMsg = "Refund is not success!";
        private const string saleSuccessMsg = "Sale is success!";
        private const string refundSuccessMsg = "Refund is success!";
        public ProcessController(IMappingService mapper, IProcessService process)
        {
            _mapper = mapper;
            _process = process;
        }
        [HttpPost]
        [RequestSizeLimit(2048)]
        [ValidateWithJsonSchemeFilter(HttpStatusCode.BadRequest, " ")]
        public async Task<ActionResult<int>> SaleAsync([FromBody] SaleRequestDto content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail(requestInNotValidMsg));

            await _process.CreateSegmentsAsync(_mapper.Map(content));
            return Ok(ApiResponse<string>.Success(null, saleSuccessMsg));
        }
        [HttpPost]
        [RequestSizeLimit(2048)]
        [ValidateWithJsonSchemeFilter(HttpStatusCode.BadRequest, " ")]
        public async Task<ActionResult> RefundAsync([FromBody] RefundRequestDto content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.Fail(requestInNotValidMsg));
            }
            bool successRefund = await _process.RefundSegmentsAsync(content);
            if (!successRefund)
                return Conflict(ApiResponse<string>.Fail(refundIsNotSuccessMsg));
            return Ok(ApiResponse<string>.Success(null, refundSuccessMsg));
        }
    }
}
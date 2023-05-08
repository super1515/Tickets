using Microsoft.AspNetCore.Mvc;
using Tickets.Dto;
using Tickets.Models;

namespace Tickets.Services.Interfaces
{
    public interface IProcessService
    {
        public Task<ActionResult> CreateSegments(SaleRequestDto request);
        public Task<ActionResult> RefundSegments(RefundRequestDto request);
    }
}

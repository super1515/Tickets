using Microsoft.AspNetCore.Mvc;
using Tickets.Dto;
using Tickets.Models;

namespace Tickets.Services.Interfaces
{
    public interface IProcessService
    {
        public Task CreateSegmentsAsync(Segments[] request);
        public Task<bool> RefundSegmentsAsync(RefundRequestDto request);
    }
}

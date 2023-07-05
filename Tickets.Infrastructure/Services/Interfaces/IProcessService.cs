using Tickets.Application.Dto;
using Tickets.Infrastructure.Models;

namespace Tickets.Infrastructure.Services.Interfaces
{
    public interface IProcessService
    {
        public Task CreateSegmentsAsync(Segments[] request);
        public Task<bool> RefundSegmentsAsync(RefundRequestDto request);
    }
}

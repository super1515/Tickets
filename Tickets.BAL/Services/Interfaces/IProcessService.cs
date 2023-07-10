using Tickets.BAL.Dto;
using Tickets.DAL.Models;

namespace Tickets.BAL.Services.Interfaces
{
    public interface IProcessService
    {
        public Task CreateSegmentsAsync(Segments[] request);
        public Task<bool> RefundSegmentsAsync(RefundRequestDto request);
    }
}

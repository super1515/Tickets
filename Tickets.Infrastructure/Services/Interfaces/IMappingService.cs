using Tickets.Application.Dto;
using Tickets.Infrastructure.Models;

namespace Tickets.Infrastructure.Services.Interfaces
{
    public interface IMappingService
    {
        public Segments[] Map(SaleRequestDto request);
    }
}

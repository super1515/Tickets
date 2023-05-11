using Tickets.Dto;
using Tickets.Models;

namespace Tickets.Services.Interfaces
{
    public interface IMappingService
    {
        public Segments[] Map(SaleRequestDto request);
    }
}

using Tickets.BAL.Dto;
using Tickets.DAL.Models;

namespace Tickets.BAL.Services.Interfaces
{
    public interface IMappingService
    {
        public Segments[] Map(SaleRequestDto request);
    }
}

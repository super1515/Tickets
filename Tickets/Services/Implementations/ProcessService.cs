using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tickets.Dto;
using Tickets.Infrastructure;
using Tickets.Models;
using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProcessService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<ActionResult> CreateSegments(SaleRequestDto request)
        {
            Dto.Route[] routes = request.Routes.ToArray();
            Segments[] segments = new Segments[request.Routes.Count()];
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = _mapper.Map<Segments>(request);
                _mapper.Map(routes[i], segments[i]);
            }
            throw new NotImplementedException();
        }

        public Task<ActionResult> RefundSegments(RefundRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}

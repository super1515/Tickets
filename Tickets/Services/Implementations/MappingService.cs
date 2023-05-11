﻿using AutoMapper;
using Tickets.Dto;
using Tickets.Models;
using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class MappingService : IMappingService
    {
        private readonly IMapper _mapper;
        public MappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Segments[] Map(SaleRequestDto content)
        {
            Dto.Route[] routes = content.Routes.ToArray();
            Segments[] segments = new Segments[content.Routes.Count()];
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = _mapper.Map<Segments>(content);
                _mapper.Map(routes[i], segments[i]);
                segments[i].ArriveDatetimeTimezone = $"-{routes[i].ArriveDatetime.Offset.Hours}";
                segments[i].ArriveDatetime = routes[i].ArriveDatetime.UtcDateTime;
                segments[i].OperationTimeTimezone = $"-{content.OperationTime.Offset.Hours}";
                segments[i].OperationTime = content.OperationTime.UtcDateTime;
                segments[i].DepartDatetimeTimezone = $"-{routes[i].DepartDatetime.Offset.Hours}";
                segments[i].DepartDatetime = routes[i].DepartDatetime.UtcDateTime;
                segments[i].SerialNumber = (uint) i + 1;
            }
            return segments;
        }
    }
}

using AutoMapper;
using System;
using Tickets.Dto;
using Tickets.Models;

namespace Tickets.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<SaleRequestDto, Segments>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Passenger.Birthdate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Passenger.Name))
                .ForMember(dest => dest.DocNumber, opt => opt.MapFrom(src => src.Passenger.DocNumber))
                .ForMember(dest => dest.TicketNumber, opt => opt.MapFrom(src => src.Passenger.TicketNumber))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Passenger.Surname))
                .ForMember(dest => dest.DocType, opt => opt.MapFrom(src => src.Passenger.DocType))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Passenger.Gender))
                .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Passenger.Patronymic))
                .ForMember(dest => dest.PassengerType, opt => opt.MapFrom(src => src.Passenger.PassengerType))
                .ForMember(dest => dest.TicketType, opt => opt.MapFrom(src => src.Passenger.TicketType));
            CreateMap<Dto.Route, Segments>().ReverseMap();
        }
    }
}

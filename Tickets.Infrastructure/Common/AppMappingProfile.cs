using AutoMapper;
using Tickets.Application.Dto;
using Tickets.Infrastructure.Models; 

namespace Tickets.Infrastructure.Common
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
                .ForMember(dest => dest.TicketType, opt => opt.MapFrom(src => src.Passenger.TicketType))
                .ForMember(dest => dest.OperationTime, opt => opt.Ignore());
            CreateMap<Application.Dto.Route, Segments>()
                .ForMember(dest => dest.OperationTime, opt => opt.Ignore())
                .ForMember(dest => dest.ArriveDatetime, opt => opt.Ignore())
                .ForMember(dest => dest.DepartDatetime, opt => opt.Ignore());
        }
    }
}

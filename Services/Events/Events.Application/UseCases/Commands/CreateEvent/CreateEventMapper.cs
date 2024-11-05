using AutoMapper;
using Events.Application.Dtos;
using Events.Domain.Models;

namespace Events.Application.UseCases.Commands.CreateEvent
{
    public class CreateEventMapper : Profile
    {
        public CreateEventMapper()
        {
            CreateMap <CreateEventCommand, Event>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Event, CreateEventResponse>();
        }
    }
}

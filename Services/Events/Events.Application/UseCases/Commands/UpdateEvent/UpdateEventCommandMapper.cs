using AutoMapper;
using CommonFiles.Messaging;
using Events.Application.Dtos;
using Events.Domain.Models;

namespace Events.Application.UseCases.Commands.UpdateEvent
{
    public class UpdateEventCommandMapper : Profile
    {
        public UpdateEventCommandMapper()
        {
            CreateMap<UpdateEventCommand, Event>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .IncludeMembers(src => src.EventDto);

            CreateMap<EventRequestDto, Event>();

            CreateMap<Event, UpdateEventResponse>();

            CreateMap<UpdateEventCommand, EventUpdated>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(c => c.MessageTitle))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(c => c.MessageContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ParticipantsId, opt => opt.Ignore());
        }
    }
}

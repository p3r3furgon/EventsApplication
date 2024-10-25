using AutoMapper;
using CommonFiles.Messaging;
using Events.Domain.Models;

namespace Events.Application.UseCases.Commands.UpdateEvent
{
    public class UpdateEventMapper : Profile
    {
        public UpdateEventMapper()
        {
            CreateMap<UpdateEventCommand, Event>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Event, UpdateEventResponse>();

            CreateMap<UpdateEventCommand, EventUpdated>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(c => c.MessageTitle))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(c => c.MessageDescription))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ParticipantsId, opt => opt.Ignore());
        }
    }
}

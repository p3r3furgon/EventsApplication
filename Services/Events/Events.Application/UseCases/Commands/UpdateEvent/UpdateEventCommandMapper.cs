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
                .ForMember(dest => dest.Title, opt => opt.Condition(s => s.Title != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(s => s.Description != null))
                .ForMember(dest => dest.Category, opt => opt.Condition(s => s.Category != null))
                .ForMember(dest => dest.DateTime, opt => opt.Condition(s => s.DateTime != null))
                .ForMember(dest => dest.MaxParticipantNumber, opt => opt.Condition(s => s.MaxParticipantNumber != null))
                .ForMember(dest => dest.Place, opt => opt.Condition(s => s.Place != null));

            CreateMap<Event, UpdateEventResponse>();

            CreateMap<UpdateEventCommand, EventUpdated>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(c => c.MessageTitle))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(c => c.MessageContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ParticipantsId, opt => opt.Ignore());
        }
    }
}

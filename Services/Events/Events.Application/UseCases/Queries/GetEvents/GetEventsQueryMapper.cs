using AutoMapper;
using Events.Application.Dtos;
using Events.Domain.Models;

namespace Events.Application.UseCases.Queries.GetEvents
{
    public class GetEventsQueryMapper : Profile
    {
        public GetEventsQueryMapper()
            {
                CreateMap<Event, EventResponseDto>()
                    .ForMember(dest => dest.ParticipantsNumber, opt => opt.MapFrom(src => src.Participants != null ? src.Participants.Count : 0))
                    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image ?? string.Empty));
            }
    }
}

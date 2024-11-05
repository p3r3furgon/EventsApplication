using AutoMapper;
using Events.Application.Dtos;
using Events.Domain.Models;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public class GetEventParticipantsQueryMapper : Profile
    {
        public GetEventParticipantsQueryMapper()
        {
            CreateMap<Participant, ParticipantResponseDto>();
        }
    }
}

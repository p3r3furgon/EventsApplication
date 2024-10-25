using AutoMapper;
using Events.Domain.Models;

namespace Events.Application.UseCases.Queries.GetEventParticipants
{
    public class GetEventParticipantsQueryMapper : Profile
    {
        public GetEventParticipantsQueryMapper()
        {
                CreateMap<Participant, GetEventParticipantsQueryResponse>();
        }
    }
}

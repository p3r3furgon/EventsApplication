using AutoMapper;
using Events.Domain.Models;
using Events.Persistance.Entities;


namespace Events.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventEntity>().ReverseMap();
            CreateMap<Participant, ParticipantEntity>().ReverseMap();
        }
    }
}

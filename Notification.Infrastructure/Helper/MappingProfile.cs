using AutoMapper;
using Notifications.Domain.Models;
using Notifications.Persistance.Entities;

namespace Notifications.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap <Notification, NotificationEntity>().ReverseMap();
        }
    }
}
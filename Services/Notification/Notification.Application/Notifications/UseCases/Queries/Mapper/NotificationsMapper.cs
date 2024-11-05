using AutoMapper;
using Notifications.Application.Dtos;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.Mapper
{
    public class NotificationsMapper : Profile
    {
        public NotificationsMapper()
        {
            CreateMap<Notification, NotificationResponseDto>();
        }
    }
}

using AutoMapper;
using Notifications.Application.Dtos;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.CommonMappers
{
    public class NotificationsMapper : Profile
    {
        public NotificationsMapper()
        {
            CreateMap<Notification, NotificationResponseDto>();
        }
    }
}

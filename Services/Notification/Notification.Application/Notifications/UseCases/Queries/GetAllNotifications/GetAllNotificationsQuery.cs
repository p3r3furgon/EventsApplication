using MediatR;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications
{
    public record GetAllNotificationsQuery() : IRequest<GetAllNotificationsResponse>;

    public record GetAllNotificationsResponse(List<Notification> Notifications);
}

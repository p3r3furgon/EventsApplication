using MediatR;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications
{
    public record GetUserNotificationsQuery(Guid UserId) : IRequest<GetUserNotificationsResponse>;

    public record GetUserNotificationsResponse(List<Notification> Notifications);
}

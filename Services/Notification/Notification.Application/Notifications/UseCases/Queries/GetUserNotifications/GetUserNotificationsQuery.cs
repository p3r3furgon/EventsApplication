using CommonFiles.Pagination;
using MediatR;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications
{
    public record GetUserNotificationsQuery(Guid UserId, PaginationParams PaginationParams) : IRequest<GetUserNotificationsResponse>;

    public record GetUserNotificationsResponse(PagedResponse<Notification> Notifications);
}

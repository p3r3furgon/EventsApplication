using CommonFiles.Pagination;
using MediatR;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications
{
    public record GetAllNotificationsQuery(PaginationParams PaginationParams) : IRequest<GetAllNotificationsResponse>;

    public record GetAllNotificationsResponse(PagedResponse<Notification> Notifications);
}

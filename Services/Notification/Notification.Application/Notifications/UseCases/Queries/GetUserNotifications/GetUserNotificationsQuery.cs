using CommonFiles.Pagination;
using MediatR;
using Notifications.Application.Dtos;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications
{
    public record GetUserNotificationsQuery(Guid UserId, PaginationParams PaginationParams) : IRequest<GetUserNotificationsResponse>;

    public record GetUserNotificationsResponse(PagedResponse<NotificationResponseDto> Notifications);
}

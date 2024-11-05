using CommonFiles.Pagination;
using MediatR;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, GetUserNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        public GetUserNotificationsQueryHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<GetUserNotificationsResponse> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _notificationsRepository
                .GetByUserId(request.UserId, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var pagedResponse = new PagedResponse<Notification>(userNotifications, request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize);
            return new GetUserNotificationsResponse(pagedResponse);
        }
    }
}

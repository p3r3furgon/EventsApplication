using CommonFiles.Pagination;
using MediatR;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications
{
    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, GetAllNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;

        public GetAllNotificationsQueryHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<GetAllNotificationsResponse> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationsRepository
                .GetAll(request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var pagedResponse = new PagedResponse<Notification>(notifications, request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize);
            return new GetAllNotificationsResponse(pagedResponse);
        }
    }
}

using MediatR;
using Notifications.Domain.Interfaces;

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
            var userNotifications = await _notificationsRepository.GetByUserId(request.UserId);
            return new GetUserNotificationsResponse(userNotifications);
        }
    }
}

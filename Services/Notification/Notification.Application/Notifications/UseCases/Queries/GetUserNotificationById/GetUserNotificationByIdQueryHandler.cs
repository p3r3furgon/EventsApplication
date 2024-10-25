using MediatR;
using Notifications.Application.Exceptions;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotificationById
{
    public class GetUserNotificationByIdQueryHandler : IRequestHandler<GetUserNotificationByIdQuery, GetUserNotificationByIdResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;

        public GetUserNotificationByIdQueryHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<GetUserNotificationByIdResponse> Handle(GetUserNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _notificationsRepository.GetByUserId(request.UserId);
            var notification = userNotifications.Where(n => n.Id == request.NotificationId).FirstOrDefault();

            if (notification == null)
                throw new NotificationNotFoundException(request.NotificationId);

            return new GetUserNotificationByIdResponse(notification);
        }
    }
}

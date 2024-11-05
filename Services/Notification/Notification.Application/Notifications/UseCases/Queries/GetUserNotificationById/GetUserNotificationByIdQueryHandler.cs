﻿using MediatR;
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
            var notification = await _notificationsRepository.GetUserNotification(request.UserId, request.NotificationId);

            if (notification == null)
                throw new NotificationNotFoundException(request.NotificationId);

            return new GetUserNotificationByIdResponse(notification);
        }
    }
}

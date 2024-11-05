using AutoMapper;
using MediatR;
using Notifications.Application.Dtos;
using Notifications.Application.Exceptions;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotificationById
{
    public class GetUserNotificationByIdQueryHandler : IRequestHandler<GetUserNotificationByIdQuery, GetUserNotificationByIdResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IMapper _mapper;

        public GetUserNotificationByIdQueryHandler(INotificationsRepository notificationsRepository, IMapper mapper)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<GetUserNotificationByIdResponse> Handle(GetUserNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsRepository.GetUserNotification(request.UserId, request.NotificationId);

            if (notification == null)
                throw new NotificationNotFoundException(request.NotificationId);

            var notificationDto = _mapper.Map<NotificationResponseDto>(notification);
            return new GetUserNotificationByIdResponse(notificationDto);
        }
    }
}

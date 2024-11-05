using CommonFiles.Interfaces;
using MediatR;
using Notifications.Application.Exceptions;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, DeleteNotificationResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteNotificationCommandHandler(INotificationsRepository notificationsRepository, IUnitOfWork unitOfWork)
        {
            _notificationsRepository = notificationsRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteNotificationResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var userNotification = await _notificationsRepository.GetUserNotification(request.UserId, request.NotificationId);

            if (userNotification == null)
                throw new NotificationNotFoundException(request.NotificationId);

            await _notificationsRepository.Delete(request.NotificationId);
            await _unitOfWork.Save(cancellationToken);
            return new DeleteNotificationResponse(userNotification.Id);
        }
    }
}

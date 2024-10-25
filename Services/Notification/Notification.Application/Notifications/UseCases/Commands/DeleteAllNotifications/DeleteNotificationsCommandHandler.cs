using MediatR;
using Notifications.Application.Exceptions;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteAllNotifications
{
    public class DeleteNotificationsCommandHandler : IRequestHandler<DeleteNotificationsCommand, DeleteNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        public DeleteNotificationsCommandHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }
        public async Task<DeleteNotificationsResponse> Handle(DeleteNotificationsCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationsRepository.GetAll();
            if (notifications.Count == 0)
                throw new NotificationNotFoundException("There are no any notifications");

            await _notificationsRepository.DeleteAll();

            return new DeleteNotificationsResponse("Success");
        }
    }
}

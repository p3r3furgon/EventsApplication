using CommonFiles.Interfaces;
using CommonFiles.Messaging;
using MassTransit;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.Consumers
{
    public class EventsConsumer : IConsumer<EventUpdated>
    {
        private readonly INotificationsRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EventsConsumer(INotificationsRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<EventUpdated> context)
        {
            var message = context.Message;

            foreach(var userId in message.ParticipantsId)
            { 
                var notification = Notification.Create(Guid.NewGuid(), message.Title, message.Content, message.DateTime, userId);
                await _notificationRepository.Create(notification);
                CancellationToken cancellationToken = new CancellationToken();
                await _unitOfWork.Save(cancellationToken);
            }
        }
    }
}

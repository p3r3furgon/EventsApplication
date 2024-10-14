using CommonFiles.Messaging;
using MassTransit;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.Consumers
{
    public class EventsConsumer : IConsumer<EventUpdated>
    {
        private readonly INotificationsRepository _notificationRepository;
        public EventsConsumer(INotificationsRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task Consume(ConsumeContext<EventUpdated> context)
        {
            var message = context.Message;

            foreach(var userId in message.ParticipantsId)
            { 
                var notification = Notification.Create(Guid.NewGuid(), message.Title, message.Message, message.DateTime, userId);
                var id = await _notificationRepository.Create(notification);
            }
        }
    }
}

using MassTransit;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.Consumers
{
    public class EventsConsumer : IConsumer<EventUpdated.EventUpdated>
    {
        private readonly INotificationsRepository _notificationRepository;
        public EventsConsumer(INotificationsRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task Consume(ConsumeContext<EventUpdated.EventUpdated> context)
        {
            var message = context.Message;
            if (message.ParticipantsId == null)
                return;
            foreach(var userId in message.ParticipantsId)
            {
                var notification = Notification.Create(Guid.NewGuid(), message.Title, message.Message, message.DateTime, userId);
                await _notificationRepository.Create(notification);
            }
        }
    }
}

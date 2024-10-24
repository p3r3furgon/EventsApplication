﻿namespace Notifications.Persistance.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
    }
}

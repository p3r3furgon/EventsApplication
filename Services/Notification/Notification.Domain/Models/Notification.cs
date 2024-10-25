namespace Notifications.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
    

        private Notification(Guid id, string title, string message, DateTime dateTime, Guid userId)
        {
            Id = id;
            Title = title;
            Message = message;
            DateTime = dateTime;
            UserId = userId;
        }

        public static Notification Create(Guid id, string title, string message, DateTime dateTime, Guid userId)
        {
            if (string.IsNullOrEmpty(title))
                throw new Exception("Title is required");
            if (string.IsNullOrEmpty(message))
                throw new Exception("Message is required");
            if (message.Length > 1000)
                throw new Exception("Message can not be longer than 1000 symbols");

            return new Notification(id, title, message, dateTime, userId);
        }
    }
}

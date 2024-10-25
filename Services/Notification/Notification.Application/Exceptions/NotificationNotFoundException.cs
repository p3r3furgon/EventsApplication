namespace Notifications.Application.Exceptions
{
    public class NotificationNotFoundException : Exception
    {
        public NotificationNotFoundException(string message) : base(message)
        {
        }

        public NotificationNotFoundException(Guid id) : base($"Notification №({id}) was not found.")
        {
        }
    }
}

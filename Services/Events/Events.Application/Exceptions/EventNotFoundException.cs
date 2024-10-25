namespace Events.Application.Exceptions
{
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string message) : base(message)
        {
        }

        public EventNotFoundException(Guid id) : base($"Event №:({id}) was not found.")
        {
        }
    }
}

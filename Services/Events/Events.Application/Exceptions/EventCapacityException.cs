namespace Events.Application.Exceptions
{
    public class EventCapacityException : Exception
    {
        public EventCapacityException(int currentParticipants, int? newMaxCapacity)
            : base($"Cannot set max capacity to {newMaxCapacity}, as there are already {currentParticipants} participants.") { }
    }
}

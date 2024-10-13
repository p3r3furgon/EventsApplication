namespace EventUpdated
{
    public class EventUpdated()
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public List<Guid> ParticipantsId { get; set; } = new List<Guid>();
    }
}

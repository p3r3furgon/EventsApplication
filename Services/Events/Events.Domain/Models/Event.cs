namespace Events.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public int MaxParticipantNumber { get; set; }
        public List<Participant> Participants { get; set; } = new List<Participant>();
        public string? Image { get; set; }
    
        public Event() { }

        private Event(Guid id, string title, string? description, DateTime dateTime,
            string place, string? category, int maxParticipantNumber, string? image)
        {
            Id = id;
            Title = title;
            Description = description;
            DateTime = dateTime;
            Place = place;
            Category = category;
            MaxParticipantNumber = maxParticipantNumber;
            Image = image;
        }

        public static Event Create(Guid id, string title, string? description, DateTime dateTime,
            string place, string? category, int maxParticipantNumber, string? image)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(place);

            return new Event(id, title, description, dateTime, place, category, maxParticipantNumber, image);
        }

    }
}

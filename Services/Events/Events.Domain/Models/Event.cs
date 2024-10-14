using Microsoft.AspNetCore.Http;

namespace Events.Domain.Models
{
    public class Event
    {
        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public string? Description { get; } = string.Empty;
        public DateTime DateTime { get; }
        public string Place { get; } = string.Empty;
        public string? Category { get; } = string.Empty;
        public int MaxParticipantNumber { get; }
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public string? Image { get; } = string.Empty;
    

        private Event(Guid id, string title, string? description, DateTime dateTime,
            string place, string? category, int maxParticipantNumber, List<Participant> participants, string? image)
        {
            Id = id;
            Title = title;
            Description = description;
            DateTime = dateTime;
            Place = place;
            Category = category;
            MaxParticipantNumber = maxParticipantNumber;
            Participants = participants;
            Image = image;
        }

        public static Event Create(string title, string? description, DateTime dateTime,
            string place, string? category, int maxParticipantNumber, List<Participant> participants, string? image)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(place);

            return new Event(Guid.NewGuid(), title, description, dateTime, place, category, maxParticipantNumber, participants, image);
        }

    }
}

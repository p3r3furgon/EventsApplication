using Microsoft.AspNetCore.Http;

namespace Events.Application.Dtos
{
    public class EventRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public int MaxParticipantNumber { get; set; }
        public IFormFile? Image { get; set; }
    }
}

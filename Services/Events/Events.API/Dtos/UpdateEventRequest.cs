namespace Events.API.Dtos
{
    public class UpdateEventRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DateTime { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int? MaxparticipantNumber { get; set; }
        public IFormFile? Image { get; set; }
    }
}

namespace Events.API.Dtos
{
    public class UpdateEventRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Place { get; set; } 
        public string? Category { get; set; } 
        public int? MaxparticipantNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string? MessageTitle { get; set; } 
        public string? MessageDescription { get; set; }
    }
}

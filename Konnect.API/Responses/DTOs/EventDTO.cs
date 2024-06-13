using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Responses.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int Day { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string? PeriodStart { get; set; }
        public string? PeriodEnd { get; set; }
        public string Location { get; set; }
        public string SubjectClass { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLoopPerDay { get; set; }
        public string Category { get; set; }
    }
}

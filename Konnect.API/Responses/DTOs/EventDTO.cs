namespace UTCClassSupport.API.Responses.DTOs
{
    public class EventDTO
    {
        public int Code { get; set; }
        public int Day { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Location { get; set; }
        public string SubjectClass { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLoopPerDay { get; set; }
        public string Category { get; set; }
    }
}

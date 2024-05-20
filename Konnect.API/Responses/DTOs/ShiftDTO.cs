namespace UTCClassSupport.API.Responses.DTOs
{
    public class ShiftDTO
    {
        public int Code { get; set; }
        public int Day { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Location { get; set; }
        public string SubjectClass { get; set; }
    }
}

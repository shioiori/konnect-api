namespace UTCClassSupport.API.Requests
{
  public class EventRequest
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Location { get; set; }
  }
}

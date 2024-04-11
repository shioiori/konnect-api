namespace UTCClassSupport.API.Responses
{
  public class GetNewsResponse : Response
  {
    public IEnumerable<NewsDTO> News { get; set; }
  }

  public class NewsDTO
  {
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public string GroupId { get; set; }
  }
}

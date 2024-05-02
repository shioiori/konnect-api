namespace UTCClassSupport.API.Requests
{
  public class BulletinRequest
  {
    public string Content {  get; set; }
  }

  public class CommentRequest
  {
    public string PostId { get; set; }
    public string Content { get; set; }
  }
}

using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Requests
{
  public class BulletinRequest
  {
    public string Content {  get; set; }
  }

  public class CommentRequest
  {
    public string Content { get; set; }
  }

  public class ChangePostStateRequest
  {
    public string PostId { get; set; }
    public ApproveState State { get; set; }
    public string? Comment { get; set; }
  }
}

namespace UTCClassSupport.API.Responses.DTOs
{

  public class PostDTO
  {
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public string GroupId { get; set; }
    public int ReplyCount { get; set; }
    public List<CommentDTO> Comments { get; set; }
    public UserDTO User { get; set; }
  }

  public class CommentDTO
  {
    public string Id { get; set; }
    public string PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public UserDTO User { get; set; }
  }
}

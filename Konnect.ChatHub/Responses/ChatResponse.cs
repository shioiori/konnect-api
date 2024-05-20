namespace Konnect.ChatHub.Responses
{
  public class ChatResponse
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserResponse CreatedBy { get; set; }
    public string Avatar { get; set; } 
    public ICollection<UserResponse> Users { get; set; }
    public ICollection<MessageResponse> Messages { get; set; }
    public GroupResponse Group { get; set; }
  }

  public class MessageResponse
  {
    public string Id { get; set; }
    public string Text { get; set; }
    public string IsImage { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserResponse CreatedBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class UserResponse
  {
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
  }

  public class GroupResponse
  {
    public string Id { get; set; }
    public string Name { get; set; }
  }
}

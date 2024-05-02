using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
  public class ChatResponse : Response
  {
    public string RoomId { get; set; }
    public string RoomName { get; set; }
    public string? Avatar { get; set; }
    public List<UserDTO> Users { get; set; }
    //public List<User>
  }

  public class MessageResponse
  {
    public string Id { get; set; }
    public string Content { get; set; }
    public string SenderId { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime Date { get; set; }
  }
}

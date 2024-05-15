using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.ChatAPI.Models
{
  public class Chat
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedDate { get; set; }
    public Message? LastMessage { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<User> Joinners { get; set; }
  }
}

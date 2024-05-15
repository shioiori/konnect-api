using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.ChatAPI.Models
{
  public class Message
  {
    public string Id { get; set; }
    public string Content { get; set; }
    public bool IsImage { get; set; }
    public string ChatId { get; set; }
    public DateTime CreatedDate { get; set; }
    public User Owner { get; set; }
  }
}

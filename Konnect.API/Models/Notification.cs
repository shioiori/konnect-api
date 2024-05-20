using System.ComponentModel.DataAnnotations.Schema;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Models
{
  [Table("notifications")]
  public class Notification
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Content { get; set; }
    public NotificationAction Action { get; set; }
    public NotificationRange Range { get; set; }
    // object of action, e.g: post, comment, etc...
    public string? ObjectId { get; set; }
    public string? GroupId { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public bool IsSeen { get; set; } = false;
    public virtual Group? Group { get; set; }
    public virtual User? User { get; set; }
  }
}

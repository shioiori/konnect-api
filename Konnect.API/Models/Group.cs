using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class Group
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Name { get; set; }
    public bool AllowOut { get; set; }
    public bool AllowInvite { get; set; } 
    public virtual ICollection<Bulletin> Bulletins { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
  }
}

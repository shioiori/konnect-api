using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("groups")]
  public class Group
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public string Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("allow_out")]
    public bool AllowOut { get; set; }
    [Column("allow_invite")]
    public bool AllowInvite { get; set; } 
    public virtual ICollection<Bulletin> Bulletins { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
  }
}

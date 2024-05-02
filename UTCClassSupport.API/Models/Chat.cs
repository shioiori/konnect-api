using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("chats")]
  public class Chat
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public string Id { get; set; }
    [Column("name")] 
    public string Name { get; set; }
    [Column("content")]
    public string? Avatar { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    public virtual ICollection<User> Joinners { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
  }
}

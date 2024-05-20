using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("messages")]
  public class Message
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [Column("id")]
    public string Id { get; set; }
    [Column("content")]
    public string Content { get; set; }
    [Column("file_id")]
    public string? FileId { get; set; }
    [Column("chat_id")]
    public string ChatId { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    public virtual MessageFile File { get; set; }
    public virtual Chat Chat { get; set; }
  }
}

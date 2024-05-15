using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("comments")]
  public class Comment
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public string Id { get; set; }
    [Column("post_id")]
    public string PostId { get; set; }
    [Column("content")]
    public string Content { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    public virtual Bulletin Post { get; set; }
  }
}

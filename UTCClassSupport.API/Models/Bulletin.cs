using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("bulletins")]
  public class Bulletin
  {
    [Column("id")]
    public int Id { get; set; }
    [Column("content")]
    public string Content { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    [Column("last_modified_date")]
    public DateTime? LastModifiedDate { get; set; }
    [Column("last_modified_by")]
    public string? LastModifiedBy { get; set; }
    [Column("group_id")]
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }
  }
}

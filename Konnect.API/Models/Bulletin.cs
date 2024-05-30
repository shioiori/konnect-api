using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class Bulletin
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public string GroupId { get; set; }
    public int Approved { get; set; }
    public virtual Group Group { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
  }
}

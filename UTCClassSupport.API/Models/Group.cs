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
    public virtual ICollection<Bulletin> Bulletins { get; set; }
    public virtual ICollection<ShareFolder> Folders { get; set; }
  }
}

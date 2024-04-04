using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("share_folders")]
  public class ShareFolder : Folder
  {
    [Column("group_id")]
    public string GroupId { get; set; }
    public virtual ICollection<ShareFile> Files { get; set; }
    public virtual Group Group { get; set; }
  }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("share_files")]
  public class ShareFile : File
  {
    [Column("folder_id")]
    public string FolderId { get; set; }
    [Column("approved")]
    public bool Approved { get; set; }
    public virtual ShareFolder Folder { get; set; }
  }
}

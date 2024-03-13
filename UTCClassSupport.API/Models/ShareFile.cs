using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("share_files")]
  public class ShareFile : File
  {
    [Column("group_id")]
    public int GroupId { get; set; }
  }
}

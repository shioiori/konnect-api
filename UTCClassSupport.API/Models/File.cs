using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class File
  {
    [Column("id")]
    public int Id { get; set; }
    [Column("url")]
    public string Url { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
  }
}

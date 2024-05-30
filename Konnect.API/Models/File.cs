using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class File
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string? Url { get; set; } = String.Empty;
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
  }
}

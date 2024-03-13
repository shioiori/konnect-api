using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("timetables")]
  public class Timetable : File
  {
    public virtual ICollection<Shift> Shifts { get; set; }
  }
}

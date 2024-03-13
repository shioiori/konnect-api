using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("shifts")]
  public class Shift
  {
    [Column("id")]
    public int Id { get; set; }
    /// <summary>
    /// from one to four
    /// </summary>
    [Column("code")]
    public int Code { get; set; }
    [Column("timetable_id")]
    public int TimetableId { get; set; }
    /// <summary>
    /// day in week (mon, tue, vv...)
    /// </summary>
    [Column("day")]
    public int Day { get; set; }
    [Column("from")]
    public DateTime From { get; set; }
    [Column("to")]
    public DateTime To { get; set; }
    [Column("location")]
    public string Location { get; set; }
    [Column("subject")]
    public string Subject { get; set; }
    public virtual Timetable Timetable { get; set; }
  }
}

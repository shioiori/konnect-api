using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("shifts")]
  public class Shift
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    /// <summary>
    /// from one to four
    /// </summary>
    [Column("code")]
    public int Code { get; set; }
    [Column("timetable_id")]
    public string TimetableId { get; set; }
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
    [Column("subject_code")]
    public string SubjectCode { get; set; }
    [Column("subject_class")]
    public string SubjectClass { get; set; }
    [Column("credit")]
    public int Credit { get; set; }
    public virtual Timetable Timetable { get; set; }
    public virtual Schedule? Schedule { get; set; }
  }
}

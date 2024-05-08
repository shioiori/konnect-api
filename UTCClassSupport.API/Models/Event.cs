using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("events")]
  public class Event
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    [Column("timetable_id")]
    public string TimetableId { get; set; }
    /// <summary>
    /// day in week (mon, tue, vv...)
    /// </summary>
    [Column("day")]
    public int? Day { get; set; }
    [Column("from")]
    public DateTime From { get; set; }
    [Column("to")]
    public DateTime To { get; set; }
    [Column("is_loop_per_day")]
    public bool IsLoopPerDay { get; set; }
    [Column("period_start")]
    public TimeSpan? PeriodStart { get; set; }
    [Column("period_end")]
    public TimeSpan? PeriodEnd { get; set; }
    [Column("title")]
    public string Title { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("location")]
    public string? Location { get; set; }
    [Column("subject")]
    public string? Subject { get; set; }
    [Column("subject_code")]
    public string? SubjectCode { get; set; }
    [Column("subject_class")]
    public string? SubjectClass { get; set; }
    [Column("credit")]
    public int? Credit { get; set; }
    public virtual Timetable Timetable { get; set; }
    public virtual Schedule? Schedule { get; set; }
  }
}

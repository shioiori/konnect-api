using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
  public class DeleteTimetableResponse : Response
  {
  }

  public class SynchronizeTimetableWithGoogleCalendarResponse : Response
  {
  }

  public class UpdateRemindTimetableResponse : Response
  {

  }

  public class GetUserTimetableResponse : Response
  {
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsSynchronize { get; set; }
    public int RemindTime { get; set; }
    public List<ShiftDTO> Events { get; set; }
  }

  public class SetScheduleTimetableResponse : Response
  {
  }
}

using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
    public class GetUserTimetableResponse
  {
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsSynchronize { get; set; }
    public int RemindTime { get; set; }
    public List<ShiftDTO> Events { get; set; }
  }
}

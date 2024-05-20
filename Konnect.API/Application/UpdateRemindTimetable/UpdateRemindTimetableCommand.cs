using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateTimetable
{
  public class UpdateRemindTimetableCommand : UserData, IRequest<UpdateRemindTimetableResponse>
  {
    public int RemindTime { get; set; }
  }
}

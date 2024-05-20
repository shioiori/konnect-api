using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateTimetable
{
    public class UpdateRemindTimetableCommand : UserInfo, IRequest<UpdateRemindTimetableResponse>
  {
    public int RemindTime { get; set; }
  }
}

using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ScheduleTimetableRemind
{
  public class SetScheduleTimetableCommand : UserData, IRequest<SetScheduleTimetableResponse>
  {
    public int Minutes { get; set; }
  }
}

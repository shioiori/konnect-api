using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ScheduleTimetableRemind
{
  public class SetScheduleTimetableCommand : BaseRequest, IRequest<SetScheduleTimetableResponse>
  {
    public int Minutes { get; set; }
  }
}

using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.OutGroup
{
  public class OutGroupCommand : UserData, IRequest<Response>
  {
    public string CurrentGroupId { get; set; }
  }
}

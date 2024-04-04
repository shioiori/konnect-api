using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ChangeNewsState
{
  public class ChangeNewsStateCommand : IRequest<ChangeNewsStateResponse>
  {
    public string PostId { get; set; }
    public ApproveProcess State { get; set; }
  }
}
